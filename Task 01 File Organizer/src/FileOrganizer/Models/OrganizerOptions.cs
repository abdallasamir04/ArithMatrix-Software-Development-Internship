namespace FileOrganizer.Models;

/// <summary>
/// Immutable, validated set of options that control a single organizing run.
/// Built once (from CLI args and/or a config file) and then passed down
/// to the service layer. Using a record gives us value-based equality and
/// a concise declaration, which is convenient for unit tests.
/// </summary>
public sealed record OrganizerOptions
{
    /// <summary>Absolute path of the directory whose files will be scanned.</summary>
    public required string SourceDirectory { get; init; }

    /// <summary>Absolute path of the directory under which category subfolders are created.</summary>
    public required string TargetDirectory { get; init; }

    /// <summary>
    /// When true, no filesystem-mutating operation is performed.
    /// The application still scans, classifies, and calculates destinations,
    /// but only reports what WOULD happen.
    /// </summary>
    public bool DryRun { get; init; }

    /// <summary>
    /// When true, subdirectories inside the source directory are scanned as well.
    /// Official requirement only asks to "ignore directories" (i.e. don't treat
    /// a directory itself as a file to move), so this defaults to false to keep
    /// behavior simple and predictable; it is an optional enhancement.
    /// </summary>
    public bool Recursive { get; init; }

    /// <summary>Optional path to write the run log to, in addition to the console.</summary>
    public string? LogFilePath { get; init; }
}
