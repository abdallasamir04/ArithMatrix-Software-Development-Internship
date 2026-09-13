namespace FileOrganizer.Services;

/// <summary>
/// Computes a filename that is guaranteed not to collide with any file
/// currently present at the destination folder, without ever overwriting
/// an existing file.
///
/// Algorithm (deterministic):
///   1. Try the original file name as-is.
///   2. If it exists, insert "_1" right before the LAST extension:
///        report.pdf              -> report_1.pdf
///        project.final.report.pdf -> project.final.report_1.pdf
///   3. If "_1" also exists, try "_2", then "_3", and so on, until a
///      free name is found.
///
/// Using Path.GetFileNameWithoutExtension / Path.GetExtension is what makes
/// step 2 correct for multi-dot names: .NET's own definition of "extension"
/// is "everything after the LAST dot", so GetFileNameWithoutExtension on
/// "project.final.report.pdf" correctly returns "project.final.report" and
/// GetExtension returns ".pdf" — the suffix is appended to the former, not
/// blindly inserted after the first dot.
/// </summary>
public sealed class ConflictResolver
{
    /// <summary>
    /// A function that reports whether a given path already exists.
    /// Injected as a delegate (instead of calling File.Exists directly)
    /// purely so unit tests can simulate "already occupied" paths without
    /// touching the real filesystem.
    /// </summary>
    public Func<string, bool> PathExists { get; }

    public ConflictResolver() : this(File.Exists)
    {
    }

    public ConflictResolver(Func<string, bool> pathExists)
    {
        PathExists = pathExists;
    }

    /// <summary>
    /// Given a desired full destination path, returns a path that does not
    /// currently exist, resolving conflicts deterministically as described
    /// in the class summary. The second return value indicates whether a
    /// conflict was actually encountered (useful for logging/tests).
    /// </summary>
    public (string ResolvedPath, bool ConflictResolved) Resolve(string desiredFullPath)
    {
        if (!PathExists(desiredFullPath))
        {
            return (desiredFullPath, false);
        }

        var directory = Path.GetDirectoryName(desiredFullPath) ?? string.Empty;
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(desiredFullPath);
        var extension = Path.GetExtension(desiredFullPath);

        var attempt = 1;
        string candidate;

        do
        {
            var candidateFileName = $"{nameWithoutExtension}_{attempt}{extension}";
            candidate = Path.Combine(directory, candidateFileName);
            attempt++;
        }
        while (PathExists(candidate));

        return (candidate, true);
    }
}
