namespace ContactManagement.Persistence;

/// <summary>
/// Thrown when contact data cannot be reliably loaded from or saved
/// to storage - for example a malformed JSON file, or the data file
/// being unreadable because of file-system permissions.
///
/// WHY a dedicated exception type instead of letting raw exceptions
/// (JsonException, IOException, UnauthorizedAccessException, ...)
/// escape from the repository: the calling code (ConsoleMenu /
/// Program.cs) should not need to know about every possible
/// underlying failure type from System.Text.Json or System.IO. It
/// only needs to know "persistence failed, here is a human-readable
/// reason". The original exception is preserved as InnerException so
/// technical details are not lost for debugging.
///
/// This is deliberately NOT used for "the file does not exist yet" -
/// that is a completely normal situation on first run and is handled
/// by returning an empty list, not by throwing.
/// </summary>
public class PersistenceException : Exception
{
    public PersistenceException(string message) : base(message)
    {
    }

    public PersistenceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
