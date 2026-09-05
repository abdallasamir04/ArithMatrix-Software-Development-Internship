using FileOrganizer.Logging;
using FileOrganizer.Models;

namespace FileOrganizer.Services;

/// <summary>
/// Orchestrates a single organizing run: validates directories, enumerates
/// files, classifies each one, resolves destination conflicts, and either
/// performs the move (live mode) or only records what would happen (dry-run).
///
/// This is the only class that touches the filesystem for mutation
/// (Directory.CreateDirectory / File.Move); FileClassifier and
/// ConflictResolver are pure/deterministic helpers it delegates to.
/// Keeping the "decide" logic (classifier, resolver) separate from the
/// "act" logic (this class) is what makes both sides independently
/// unit-testable without needing real directories for the decision logic.
/// </summary>
public sealed class FileOrganizerService
{
    private readonly FileClassifier _classifier;
    private readonly ConflictResolver _conflictResolver;

    public FileOrganizerService(FileClassifier classifier, ConflictResolver conflictResolver)
    {
        _classifier = classifier;
        _conflictResolver = conflictResolver;
    }

    /// <summary>
    /// Runs the organizer end-to-end and returns one <see cref="FileOperationResult"/>
    /// per file discovered in the source directory (top-level only, unless
    /// <see cref="OrganizerOptions.Recursive"/> is set).
    /// </summary>
    public IReadOnlyList<FileOperationResult> Organize(OrganizerOptions options, OrganizerLogger logger)
    {
        var results = new List<FileOperationResult>();

        // --- Guard: source must exist -----------------------------------
        if (!Directory.Exists(options.SourceDirectory))
        {
            throw new DirectoryNotFoundException(
                $"Source directory does not exist: '{options.SourceDirectory}'.");
        }

        // --- Guard: target must not be the same as, or an ancestor-relative
        // subfolder scenario that would cause the tool to reprocess its own
        // output. We normalize both paths and compare, and we also refuse
        // when target is nested inside source, because newly created
        // category folders (Images/, Documents/, ...) would otherwise sit
        // inside the very directory being scanned and could be picked up
        // on a subsequent run or, if --recursive is used, the same run. ---
        var normalizedSource = NormalizeForComparison(options.SourceDirectory);
        var normalizedTarget = NormalizeForComparison(options.TargetDirectory);

        if (string.Equals(normalizedSource, normalizedTarget, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Source and target directories must not be the same directory.");
        }

        if (IsSubPathOf(normalizedTarget, normalizedSource))
        {
            throw new InvalidOperationException(
                "Target directory must not be located inside the source directory, " +
                "otherwise the organizer could reprocess the files it just moved.");
        }

        // --- Prepare target -----------------------------------------------
        if (!options.DryRun)
        {
            Directory.CreateDirectory(options.TargetDirectory);
        }

        var searchOption = options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

        // Directory.EnumerateFiles never returns directory entries themselves,
        // which is how "ignore directories" (functional requirement) is satisfied
        // for free: we only ever enumerate files, never subdirectories as items.
        IEnumerable<string> filePaths;
        try
        {
            filePaths = Directory.EnumerateFiles(options.SourceDirectory, "*", searchOption).ToList();
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or IOException)
        {
            throw new IOException($"Unable to read source directory '{options.SourceDirectory}': {ex.Message}", ex);
        }

        foreach (var sourcePath in filePaths)
        {
            var result = ProcessFile(sourcePath, options);
            results.Add(result);
            logger.LogOperation(result);
        }

        return results;
    }

    private FileOperationResult ProcessFile(string sourcePath, OrganizerOptions options)
    {
        try
        {
            var fileName = Path.GetFileName(sourcePath);
            var category = _classifier.Classify(fileName);
            var categoryFolder = Path.Combine(options.TargetDirectory, category.ToString());
            var desiredDestination = Path.Combine(categoryFolder, fileName);

            // In dry-run mode we still want realistic conflict resolution, but
            // we must not consult the real filesystem for files that a PRIOR
            // step in this same dry-run "would have" created (since nothing
            // was actually created). File.Exists is sufficient here because
            // dry-run never creates files, so every check reflects only
            // what is genuinely already on disk before this run started.
            var (resolvedDestination, conflictResolved) = _conflictResolver.Resolve(desiredDestination);

            if (options.DryRun)
            {
                return new FileOperationResult
                {
                    SourcePath = sourcePath,
                    DestinationPath = resolvedDestination,
                    Category = category,
                    Status = OperationStatus.Planned,
                    ConflictResolved = conflictResolved
                };
            }

            Directory.CreateDirectory(categoryFolder);

            // File.Move (without overwrite) throws IOException if the destination
            // already exists. Because ConflictResolver already guaranteed
            // 'resolvedDestination' was free at the time we checked, this call
            // succeeds in the overwhelming majority of cases; the try/catch below
            // still guards against the narrow race where another process creates
            // the same file between our check and this move.
            File.Move(sourcePath, resolvedDestination, overwrite: false);

            return new FileOperationResult
            {
                SourcePath = sourcePath,
                DestinationPath = resolvedDestination,
                Category = category,
                Status = OperationStatus.Moved,
                ConflictResolved = conflictResolved
            };
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or PathTooLongException)
        {
            return new FileOperationResult
            {
                SourcePath = sourcePath,
                DestinationPath = string.Empty,
                Category = FileCategory.Others,
                Status = OperationStatus.Error,
                ErrorMessage = ex.Message
            };
        }
    }

    private static string NormalizeForComparison(string path) =>
        Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));

    /// <summary>
    /// Returns true if <paramref name="candidate"/> is the same directory as, or
    /// nested inside, <paramref name="root"/>.
    /// </summary>
    private static bool IsSubPathOf(string candidate, string root)
    {
        var rootWithSeparator = root + Path.DirectorySeparatorChar;
        return candidate.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase);
    }
}
