using System.Text.RegularExpressions;

namespace ContactManagement.Validation;

/// <summary>
/// Validates raw user input for a contact and provides normalization
/// helpers used both for storage and for duplicate comparison.
///
/// WHY separate Validation from Normalization (two different questions):
///   - Validation answers: "Is this input acceptable at all?"
///     (e.g. is the email at least shaped like an email?)
///   - Normalization answers: "How should equivalent input be
///     represented consistently?"
///     (e.g. "John@Example.com" and " john@example.com " should be
///     treated as the same email for duplicate detection, even though
///     we still display/store what the user actually typed.)
///
/// Keeping these concerns in one static class (rather than scattering
/// trimming/casing logic across the codebase) means every place that
/// needs to compare two contacts uses the exact same rules.
/// </summary>
public static class ContactValidator
{
    private const int MaxNameLength = 100;
    private const int MaxEmailLength = 254;
    private const int MaxPhoneLength = 20;
    private const int MinPhoneDigits = 7;
    private const int MaxPhoneDigits = 15;

    // A deliberately practical (not RFC-5322-perfect) email pattern:
    // something@something.something, no spaces. Attempting to fully
    // implement the official email specification is not worth the
    // complexity for a console contact book; this catches the vast
    // majority of real mistakes (missing "@", missing domain, spaces).
    private static readonly Regex EmailPattern = new(
        @"^[^\s@]+@[^\s@]+\.[^\s@]+$",
        RegexOptions.Compiled);

    /// <summary>
    /// Validates the three required fields of a contact.
    /// Does NOT check for duplicates - that is a business rule that
    /// depends on the other contacts already stored, so it belongs in
    /// ContactService, not in this pure-input validator.
    /// </summary>
    public static ValidationResult Validate(string? fullName, string? phone, string? email)
    {
        var errors = new List<string>();

        ValidateFullName(fullName, errors);
        ValidatePhone(phone, errors);
        ValidateEmail(email, errors);

        return errors.Count == 0 ? ValidationResult.Success() : ValidationResult.Failure(errors);
    }

    private static void ValidateFullName(string? fullName, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            errors.Add("Full name is required and cannot be empty or whitespace only.");
            return;
        }

        // Deliberately does NOT restrict to English letters only.
        // Names may legitimately contain spaces, hyphens, apostrophes,
        // accented characters, or non-Latin scripts (e.g. Arabic).
        if (fullName.Trim().Length > MaxNameLength)
        {
            errors.Add($"Full name is too long (maximum {MaxNameLength} characters).");
        }
    }

    private static void ValidatePhone(string? phone, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            errors.Add("Phone number is required and cannot be empty or whitespace only.");
            return;
        }

        var trimmed = phone.Trim();

        if (trimmed.Length > MaxPhoneLength)
        {
            errors.Add($"Phone number is too long (maximum {MaxPhoneLength} characters).");
            return;
        }

        // Accept digits plus a small set of common formatting
        // characters: leading "+", spaces, and hyphens. Reject
        // anything else (letters, symbols) as obviously invalid.
        if (!Regex.IsMatch(trimmed, @"^\+?[0-9 \-]+$"))
        {
            errors.Add("Phone number contains invalid characters. Use digits, spaces, '-' and an optional leading '+'.");
            return;
        }

        var digitCount = trimmed.Count(char.IsDigit);
        if (digitCount < MinPhoneDigits || digitCount > MaxPhoneDigits)
        {
            errors.Add($"Phone number must contain between {MinPhoneDigits} and {MaxPhoneDigits} digits.");
        }
    }

    private static void ValidateEmail(string? email, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("Email address is required and cannot be empty or whitespace only.");
            return;
        }

        var trimmed = email.Trim();

        if (trimmed.Length > MaxEmailLength)
        {
            errors.Add($"Email address is too long (maximum {MaxEmailLength} characters).");
            return;
        }

        if (!EmailPattern.IsMatch(trimmed))
        {
            errors.Add("Email address is not in a valid format (expected something like 'name@domain.com').");
        }
    }

    /// <summary>
    /// Normalizes a name for storage: trims leading/trailing
    /// whitespace but preserves internal spacing and casing exactly
    /// as the user intended (e.g. "Al Sayed" stays "Al Sayed").
    /// </summary>
    public static string NormalizeName(string fullName) => fullName.Trim();

    /// <summary>
    /// Normalizes an email for storage: trims whitespace only.
    /// The original casing is preserved for display purposes.
    /// </summary>
    public static string NormalizeEmailForStorage(string email) => email.Trim();

    /// <summary>
    /// Normalizes an email for comparison/duplicate detection:
    /// trimmed and lower-cased, since email addresses are
    /// conventionally treated as case-insensitive.
    /// </summary>
    public static string NormalizeEmailForComparison(string email) => email.Trim().ToLowerInvariant();

    /// <summary>
    /// Normalizes a phone number for storage: trims whitespace only,
    /// preserving the formatting the user chose (e.g. "0100-123-4567").
    /// </summary>
    public static string NormalizePhoneForStorage(string phone) => phone.Trim();

    /// <summary>
    /// Normalizes a phone number for comparison/duplicate detection by
    /// stripping everything except digits and a leading '+'. This lets
    /// "0100 123 4567", "0100-123-4567" and "01001234567" be recognized
    /// as the same number, which is what a real user would expect.
    ///
    /// This is intentionally simple: it does not attempt full
    /// international phone number parsing (country codes, area code
    /// rules, etc.) because that would require a heavyweight library
    /// and is not required for this internship task. It only removes
    /// spaces and hyphens for the purpose of comparison.
    /// </summary>
    public static string NormalizePhoneForComparison(string phone)
    {
        var digitsOnly = new string(phone.Where(char.IsDigit).ToArray());
        return digitsOnly;
    }
}
