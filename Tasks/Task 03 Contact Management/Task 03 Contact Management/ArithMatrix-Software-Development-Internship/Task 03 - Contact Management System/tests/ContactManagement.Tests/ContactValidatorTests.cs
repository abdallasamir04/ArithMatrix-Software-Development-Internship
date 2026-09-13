using ContactManagement.Validation;
using Xunit;

namespace ContactManagement.Tests;

public class ContactValidatorTests
{
    [Fact]
    public void Validate_WithAllValidFields_ReturnsSuccess()
    {
        var result = ContactValidator.Validate("John Doe", "01001234567", "john@example.com");

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithEmptyName_ReturnsFailure()
    {
        var result = ContactValidator.Validate("", "01001234567", "john@example.com");

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Full name"));
    }

    [Fact]
    public void Validate_WithWhitespaceOnlyName_ReturnsFailure()
    {
        var result = ContactValidator.Validate("   ", "01001234567", "john@example.com");

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Full name"));
    }

    [Theory]
    [InlineData("01001234567")]
    [InlineData("+201001234567")]
    [InlineData("0100 123 4567")]
    [InlineData("0100-123-4567")]
    public void Validate_WithReasonablyFormattedPhone_ReturnsSuccess(string phone)
    {
        var result = ContactValidator.Validate("John Doe", phone, "john@example.com");

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("123")]
    [InlineData("")]
    public void Validate_WithInvalidPhone_ReturnsFailure(string phone)
    {
        var result = ContactValidator.Validate("John Doe", phone, "john@example.com");

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing-at-sign.com")]
    [InlineData("no-domain@")]
    [InlineData("")]
    public void Validate_WithInvalidEmail_ReturnsFailure(string email)
    {
        var result = ContactValidator.Validate("John Doe", "01001234567", email);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void NormalizePhoneForComparison_RemovesFormattingCharacters()
    {
        var a = ContactValidator.NormalizePhoneForComparison("0100 123 4567");
        var b = ContactValidator.NormalizePhoneForComparison("0100-123-4567");
        var c = ContactValidator.NormalizePhoneForComparison("01001234567");

        Assert.Equal(a, b);
        Assert.Equal(b, c);
    }

    [Fact]
    public void NormalizeEmailForComparison_IsCaseInsensitive()
    {
        var a = ContactValidator.NormalizeEmailForComparison("John@Example.com");
        var b = ContactValidator.NormalizeEmailForComparison(" john@example.com ");

        Assert.Equal(a, b);
    }

    [Fact]
    public void NormalizeName_TrimsWhitespaceButKeepsInternalSpacing()
    {
        var result = ContactValidator.NormalizeName("  John   Doe  ");

        Assert.Equal("John   Doe", result);
    }
}
