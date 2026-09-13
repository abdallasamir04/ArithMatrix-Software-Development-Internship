using ContactManagement.Models;
using ContactManagement.Persistence;
using Xunit;

namespace ContactManagement.Tests;

/// <summary>
/// These tests exercise the real JsonContactRepository against the
/// real file system - but ONLY inside a temporary directory created
/// fresh for each test and deleted afterwards (see IDisposable
/// below). This guarantees tests never read or write any real data
/// file belonging to the person running them, and never leave files
/// behind on disk.
/// </summary>
public class JsonContactRepositoryTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly string _dataFilePath;

    public JsonContactRepositoryTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "ContactManagementTests_" + Guid.NewGuid());
        _dataFilePath = Path.Combine(_tempDirectory, "contacts.json");
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }

    [Fact]
    public void LoadAll_WhenFileDoesNotExist_ReturnsEmptyList()
    {
        var repository = new JsonContactRepository(_dataFilePath);

        var contacts = repository.LoadAll();

        Assert.Empty(contacts);
    }

    [Fact]
    public void LoadAll_WhenDirectoryDoesNotExist_ReturnsEmptyListAndDoesNotThrow()
    {
        var nestedPath = Path.Combine(_tempDirectory, "nested", "contacts.json");
        var repository = new JsonContactRepository(nestedPath);

        var contacts = repository.LoadAll();

        Assert.Empty(contacts);
    }

    [Fact]
    public void SaveAll_ThenLoadAll_RoundTripsContactsCorrectly()
    {
        var repository = new JsonContactRepository(_dataFilePath);
        var contacts = new List<Contact>
        {
            new() { Id = 1, FullName = "John Doe", Phone = "01001234567", Email = "john@example.com" },
            new() { Id = 2, FullName = "Jane Smith", Phone = "01009876543", Email = "jane@example.com" }
        };

        repository.SaveAll(contacts);
        var loaded = repository.LoadAll();

        Assert.Equal(2, loaded.Count);
        Assert.Equal("John Doe", loaded[0].FullName);
        Assert.Equal("jane@example.com", loaded[1].Email);
    }

    [Fact]
    public void LoadAll_WhenFileIsEmpty_ReturnsEmptyList()
    {
        Directory.CreateDirectory(_tempDirectory);
        File.WriteAllText(_dataFilePath, string.Empty);
        var repository = new JsonContactRepository(_dataFilePath);

        var contacts = repository.LoadAll();

        Assert.Empty(contacts);
    }

    [Fact]
    public void LoadAll_WhenFileContainsMalformedJson_ThrowsPersistenceExceptionAndLeavesFileUntouched()
    {
        Directory.CreateDirectory(_tempDirectory);
        File.WriteAllText(_dataFilePath, "{ this is not valid json ][");
        var repository = new JsonContactRepository(_dataFilePath);

        Assert.Throws<PersistenceException>(() => repository.LoadAll());

        // The malformed file must not have been silently replaced.
        Assert.Equal("{ this is not valid json ][", File.ReadAllText(_dataFilePath));
    }

    [Fact]
    public void SaveAll_CreatesDataDirectoryIfMissing()
    {
        var nestedPath = Path.Combine(_tempDirectory, "nested", "contacts.json");
        var repository = new JsonContactRepository(nestedPath);

        repository.SaveAll(new List<Contact>
        {
            new() { Id = 1, FullName = "John Doe", Phone = "01001234567", Email = "john@example.com" }
        });

        Assert.True(File.Exists(nestedPath));
    }

    [Fact]
    public void SaveAll_DoesNotLeaveTemporaryFileBehind()
    {
        var repository = new JsonContactRepository(_dataFilePath);

        repository.SaveAll(new List<Contact>
        {
            new() { Id = 1, FullName = "John Doe", Phone = "01001234567", Email = "john@example.com" }
        });

        Assert.False(File.Exists(_dataFilePath + ".tmp"));
    }
}
