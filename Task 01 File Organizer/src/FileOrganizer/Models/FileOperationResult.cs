namespace FileOrganizer.Models;

/// <summary>
/// Outcome states for a single file's processing attempt.
/// </summary>
public enum OperationStatus
{
    Planned,
    Moved,
    Skipped,
    Error
}

/// <summary>
/// The result of processing a single file: what was decided, what happened
/// (or would happen, in dry-run mode), and any error encountered.
/// This is the unit the logger and the final summary are built from.
/// </summary>
public sealed record FileOperationResult
{
    public required string SourcePath { get; init; }
    public required string DestinationPath { get; init; }
    public required FileCategory Category { get; init; }
    public required OperationStatus Status { get; init; }
    public bool ConflictResolved { get; init; }
    public string? ErrorMessage { get; init; }
}
