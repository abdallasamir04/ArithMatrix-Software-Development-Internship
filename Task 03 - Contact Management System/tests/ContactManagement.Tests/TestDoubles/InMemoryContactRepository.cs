using ContactManagement.Models;
using ContactManagement.Persistence;

namespace ContactManagement.Tests.TestDoubles;

/// <summary>
/// A test double implementing <see cref="IContactRepository"/> purely
/// in memory. This is what makes ContactServiceTests possible without
/// touching the real file system: ContactService only depends on the
/// IContactRepository abstraction, so tests can substitute this class
/// for JsonContactRepository. No mocking framework is needed for
/// something this simple - a plain hand-written class is easier to
/// read than a generated mock.
/// </summary>
public class InMemoryContactRepository : IContactRepository
{
    private List<Contact> _storage = new();

    public List<Contact> LoadAll() => _storage.Select(Clone).ToList();

    public void SaveAll(IReadOnlyList<Contact> contacts)
    {
        _storage = contacts.Select(Clone).ToList();
    }

    private static Contact Clone(Contact c) => new()
    {
        Id = c.Id,
        FullName = c.FullName,
        Phone = c.Phone,
        Email = c.Email
    };
}
