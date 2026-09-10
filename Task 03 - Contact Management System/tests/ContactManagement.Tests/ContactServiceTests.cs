using ContactManagement.Services;
using ContactManagement.Tests.TestDoubles;
using Xunit;

namespace ContactManagement.Tests;

public class ContactServiceTests
{
    private static ContactService CreateService() => new(new InMemoryContactRepository());

    // ----- Create -----

    [Fact]
    public void AddContact_WithValidData_Succeeds()
    {
        var service = CreateService();

        var result = service.AddContact("John Doe", "01001234567", "john@example.com");

        Assert.True(result.IsSuccess);
        Assert.Equal("John Doe", result.Value!.FullName);
        Assert.Equal(1, result.Value.Id);
    }

    [Fact]
    public void AddContact_WithEmptyName_IsRejected()
    {
        var service = CreateService();

        var result = service.AddContact("", "01001234567", "john@example.com");

        Assert.False(result.IsSuccess);
        Assert.Empty(service.GetAll());
    }

    [Fact]
    public void AddContact_WithInvalidPhone_IsRejected()
    {
        var service = CreateService();

        var result = service.AddContact("John Doe", "abc", "john@example.com");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void AddContact_WithInvalidEmail_IsRejected()
    {
        var service = CreateService();

        var result = service.AddContact("John Doe", "01001234567", "not-an-email");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void AddContact_GeneratesSequentialIds()
    {
        var service = CreateService();

        var first = service.AddContact("John Doe", "01001234567", "john@example.com");
        var second = service.AddContact("Jane Smith", "01009876543", "jane@example.com");

        Assert.Equal(1, first.Value!.Id);
        Assert.Equal(2, second.Value!.Id);
    }

    // ----- Duplicate handling -----

    [Fact]
    public void AddContact_WithDuplicatePhone_IsRejected()
    {
        var service = CreateService();
        service.AddContact("John Doe", "01001234567", "john@example.com");

        var result = service.AddContact("Someone Else", "01001234567", "different@example.com");

        Assert.False(result.IsSuccess);
        Assert.Contains("phone", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AddContact_WithDuplicateEmail_IsRejected()
    {
        var service = CreateService();
        service.AddContact("John Doe", "01001234567", "john@example.com");

        var result = service.AddContact("Someone Else", "01009999999", "john@example.com");

        Assert.False(result.IsSuccess);
        Assert.Contains("email", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AddContact_WithEquivalentlyFormattedDuplicatePhone_IsDetected()
    {
        var service = CreateService();
        service.AddContact("John Doe", "0100 123 4567", "john@example.com");

        var result = service.AddContact("Someone Else", "0100-123-4567", "different@example.com");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void UpdateContact_DoesNotDetectItselfAsDuplicate()
    {
        var service = CreateService();
        var added = service.AddContact("John Doe", "01001234567", "john@example.com");

        var result = service.UpdateContact(added.Value!.Id, "John Doe", "01001234567", "john@example.com");

        Assert.True(result.IsSuccess);
    }

    // ----- Read -----

    [Fact]
    public void GetAll_ReturnsEveryStoredContact()
    {
        var service = CreateService();
        service.AddContact("John Doe", "01001234567", "john@example.com");
        service.AddContact("Jane Smith", "01009876543", "jane@example.com");

        var all = service.GetAll();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public void GetById_WithExistingId_ReturnsContact()
    {
        var service = CreateService();
        var added = service.AddContact("John Doe", "01001234567", "john@example.com");

        var found = service.GetById(added.Value!.Id);

        Assert.NotNull(found);
        Assert.Equal("John Doe", found!.FullName);
    }

    [Fact]
    public void GetById_WithNonexistentId_ReturnsNull()
    {
        var service = CreateService();

        var found = service.GetById(999);

        Assert.Null(found);
    }

    // ----- Update -----

    [Fact]
    public void UpdateContact_WithValidData_PersistsChanges()
    {
        var service = CreateService();
        var added = service.AddContact("John Doe", "01001234567", "john@example.com");

        var result = service.UpdateContact(added.Value!.Id, "John Doe", "01001234567", "new-email@example.com");

        Assert.True(result.IsSuccess);
        Assert.Equal("new-email@example.com", service.GetById(added.Value.Id)!.Email);
    }

    [Fact]
    public void UpdateContact_WithInvalidData_IsRejected()
    {
        var service = CreateService();
        var added = service.AddContact("John Doe", "01001234567", "john@example.com");

        var result = service.UpdateContact(added.Value!.Id, "", "01001234567", "john@example.com");

        Assert.False(result.IsSuccess);
        // Original data must remain untouched.
        Assert.Equal("John Doe", service.GetById(added.Value.Id)!.FullName);
    }

    [Fact]
    public void UpdateContact_ChangingToAnotherContactsPhone_IsRejected()
    {
        var service = CreateService();
        service.AddContact("John Doe", "01001234567", "john@example.com");
        var second = service.AddContact("Jane Smith", "01009876543", "jane@example.com");

        var result = service.UpdateContact(second.Value!.Id, "Jane Smith", "01001234567", "jane@example.com");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void UpdateContact_ChangingToAnotherContactsEmail_IsRejected()
    {
        var service = CreateService();
        service.AddContact("John Doe", "01001234567", "john@example.com");
        var second = service.AddContact("Jane Smith", "01009876543", "jane@example.com");

        var result = service.UpdateContact(second.Value!.Id, "Jane Smith", "01009876543", "john@example.com");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void UpdateContact_WithNonexistentId_ReturnsFailure()
    {
        var service = CreateService();

        var result = service.UpdateContact(999, "John Doe", "01001234567", "john@example.com");

        Assert.False(result.IsSuccess);
    }

    // ----- Delete -----

    [Fact]
    public void DeleteContact_WithExistingId_RemovesContact()
    {
        var service = CreateService();
        var added = service.AddContact("John Doe", "01001234567", "john@example.com");

        var result = service.DeleteContact(added.Value!.Id);

        Assert.True(result.IsSuccess);
        Assert.Null(service.GetById(added.Value.Id));
    }

    [Fact]
    public void DeleteContact_WithNonexistentId_ReturnsFailure()
    {
        var service = CreateService();

        var result = service.DeleteContact(999);

        Assert.False(result.IsSuccess);
    }

    // ----- Search -----

    [Fact]
    public void Search_ByName_IsCaseInsensitiveAndPartial()
    {
        var service = CreateService();
        service.AddContact("John Doe", "01001234567", "john@example.com");

        var results = service.Search(SearchField.Name, "john");

        Assert.Single(results);
    }

    [Fact]
    public void Search_ByPhone_IgnoresFormattingDifferences()
    {
        var service = CreateService();
        service.AddContact("John Doe", "0100-123-4567", "john@example.com");

        var results = service.Search(SearchField.Phone, "01001234567");

        Assert.Single(results);
    }

    [Fact]
    public void Search_ByEmail_IsCaseInsensitive()
    {
        var service = CreateService();
        service.AddContact("John Doe", "01001234567", "John@Example.com");

        var results = service.Search(SearchField.Email, "john@example.com");

        Assert.Single(results);
    }

    [Fact]
    public void Search_WithNoMatches_ReturnsEmptyList()
    {
        var service = CreateService();
        service.AddContact("John Doe", "01001234567", "john@example.com");

        var results = service.Search(SearchField.Name, "Nonexistent");

        Assert.Empty(results);
    }

    [Fact]
    public void Search_AllFields_MatchesAcrossNamePhoneAndEmail()
    {
        var service = CreateService();
        service.AddContact("John Doe", "01001234567", "john@example.com");
        service.AddContact("Jane Smith", "01009876543", "jane@example.com");

        var results = service.Search(SearchField.All, "01009876543");

        Assert.Single(results);
        Assert.Equal("Jane Smith", results[0].FullName);
    }

    // ----- IDs -----

    [Fact]
    public void DeletedId_IsNotReusedWhileHigherIdsExist()
    {
        var service = CreateService();
        var first = service.AddContact("John Doe", "01001234567", "john@example.com");
        var second = service.AddContact("Jane Smith", "01009876543", "jane@example.com");

        service.DeleteContact(first.Value!.Id);
        var third = service.AddContact("Mark Lee", "01005555555", "mark@example.com");

        Assert.NotEqual(first.Value.Id, third.Value!.Id);
        Assert.True(third.Value.Id > second.Value!.Id);
    }
}
