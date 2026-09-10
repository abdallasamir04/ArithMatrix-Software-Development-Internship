using ContactManagement.Models;

namespace ContactManagement.Persistence;

/// <summary>
/// Defines how contacts are loaded from and saved to durable storage,
/// without exposing HOW that storage works.
///
/// WHY this interface exists (and is not "over-engineering" here):
///   - ContactService (business logic) should not need to know or
///     care whether contacts live in a JSON file, a SQLite database,
///     or purely in memory. It only needs "give me all contacts" and
///     "save this list of contacts".
///   - This makes ContactService testable WITHOUT touching the real
///     file system: unit tests can use an InMemoryContactRepository
///     test double instead of JsonContactRepository, so tests run
///     fast and never risk corrupting a real data file.
///   - If we ever needed to swap JSON for SQLite (see "Future
///     Improvements" in the README), only a new class implementing
///     this interface would be needed - ContactService would not
///     change at all.
///
/// What would happen if we removed this abstraction? ContactService
/// would call System.Text.Json and File I/O directly. The application
/// would still work, but unit-testing business rules (duplicate
/// detection, validation flow) would require touching real files in
/// every test, which is slower and risks test pollution. For a
/// project this size the interface is a small, well-justified cost.
/// </summary>
public interface IContactRepository
{
    /// <summary>
    /// Loads every stored contact. Must return an empty list (never
    /// null and never throw) when no contacts have been saved yet.
    /// </summary>
    List<Contact> LoadAll();

    /// <summary>
    /// Persists the given list as the complete, authoritative set of
    /// contacts, replacing whatever was previously stored.
    /// </summary>
    void SaveAll(IReadOnlyList<Contact> contacts);
}
