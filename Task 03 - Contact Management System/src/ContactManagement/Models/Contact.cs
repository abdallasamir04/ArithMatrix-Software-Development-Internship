namespace ContactManagement.Models;

/// <summary>
/// Represents a single contact stored by the application.
/// This is a pure data model: it holds state only and contains no
/// business rules (validation, duplicate checks, etc.). Keeping the
/// model "dumb" makes it safe to serialize directly to and from JSON
/// and keeps business rules concentrated in ContactValidator and
/// ContactService instead of being spread across the codebase.
/// </summary>
public class Contact
{
    /// <summary>
    /// Unique identifier of the contact.
    ///
    /// Design decision: a simple auto-incrementing integer is used
    /// instead of a Guid.
    ///   - An int is easy for a human to type at a console prompt
    ///     ("Enter contact ID: 3") which matters a lot for a console UI.
    ///   - A Guid (e.g. "3fa85f64-5717-4562-b3fc-2c963f66afa6") is
    ///     harder to read, harder to type, and offers no real benefit
    ///     here because this application is single-user and local, so
    ///     there is no risk of ID collisions across machines the way
    ///     there would be in a distributed system.
    ///   - IDs are generated as (current maximum Id in the file) + 1,
    ///     so a deleted ID is never reused as long as at least one
    ///     contact with a higher ID still exists. If all contacts are
    ///     deleted, ID generation restarts at 1, which is acceptable
    ///     for a small local address book.
    /// </summary>
    public int Id { get; set; }

    /// <summary>Full name of the contact, as entered by the user (trimmed).</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Phone number exactly as the user typed it (trimmed only).
    /// The application stores the human-readable form the user typed
    /// and separately normalizes it only for comparison purposes
    /// (see ContactValidator.NormalizePhone). This preserves the
    /// user's preferred formatting while still detecting duplicates.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Email address, trimmed. Comparison for duplicates is
    /// case-insensitive (see ContactValidator.NormalizeEmail) even
    /// though the stored value preserves the original casing.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
