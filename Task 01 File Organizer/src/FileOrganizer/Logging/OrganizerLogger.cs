using System.Text;
using FileOrganizer.Models;

namespace FileOrganizer.Logging;

/// <summary>
/// A small, dependency-free logger tailored to this application's needs.
///
/// We deliberately do NOT bring in Microsoft.Extensions.Logging here: that
/// package is designed for applications with multiple log providers, log
/// levels, scopes, and DI-based configuration. For a single-purpose console
/// tool that only ever needs to (a) print to the console and (b) optionally
/// mirror the same text to a log file, a small dedicated class is simpler,
/// has zero external dependencies, and is easier for a learner to read
/// top-to-bottom than framework configuration would be. This is an explicit
/// "avoid unnecessary third-party dependencies" decision.
/// </summary>
public sealed class OrganizerLogger
{
    private readonly StringBuilder _buffer = new();
    private readonly string? _logFilePath;

    public OrganizerLogger(string? logFilePath)
    {
        _logFilePath = logFilePath;
    }

    /// <summary>Writes a line to the console and records it for the optional log file.</summary>
    public void WriteLine(string message = "")
    {
        Console.WriteLine(message);
        _buffer.AppendLine(message);
    }

    public void LogHeader(OrganizerOptions options)
    {
        WriteLine(new string('=', 50));
        WriteLine("FILE ORGANIZER");
        WriteLine(new string('=', 50));
        WriteLine($"Mode: {(options.DryRun ? "DRY RUN" : "LIVE")}");
        WriteLine($"Source: {options.SourceDirectory}");
        WriteLine($"Target: {options.TargetDirectory}");
        WriteLine();
    }

    public void LogOperation(FileOperationResult result)
    {
        switch (result.Status)
        {
            case OperationStatus.Planned:
                WriteLine($"[DRY-RUN] {Path.GetFileName(result.SourcePath)} -> {result.Category}{Path.DirectorySeparatorChar}{Path.GetFileName(result.DestinationPath)}");
                break;

            case OperationStatus.Moved:
                var conflictNote = result.ConflictResolved ? " (renamed to avoid conflict)" : string.Empty;
                WriteLine($"[MOVED]   {Path.GetFileName(result.SourcePath)} -> {result.Category}{Path.DirectorySeparatorChar}{Path.GetFileName(result.DestinationPath)}{conflictNote}");
                break;

            case OperationStatus.Skipped:
                WriteLine($"[SKIPPED] {result.SourcePath}: {result.ErrorMessage}");
                break;

            case OperationStatus.Error:
                WriteLine($"[ERROR]   {result.SourcePath}: {result.ErrorMessage}");
                break;
        }
    }

    public void LogSummary(int scanned, int organized, int errors)
    {
        WriteLine();
        WriteLine("Summary:");
        WriteLine($"Files scanned: {scanned}");
        WriteLine($"Files organized: {organized}");
        WriteLine($"Errors: {errors}");
        WriteLine(new string('=', 50));
    }

    /// <summary>
    /// Flushes the accumulated log text to disk, if a log file path was configured.
    /// Called once at the end of the run so a single write happens rather than
    /// many small appends.
    /// </summary>
    public void FlushToFile()
    {
        if (string.IsNullOrWhiteSpace(_logFilePath))
        {
            return;
        }

        var directory = Path.GetDirectoryName(_logFilePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(_logFilePath, _buffer.ToString());
    }
}
