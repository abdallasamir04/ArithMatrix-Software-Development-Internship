namespace ContactManagement.Validation;

/// <summary>
/// Represents the outcome of validating user input.
///
/// WHY this class exists instead of throwing an exception for invalid
/// input: invalid user input (an empty name, a malformed email) is an
/// entirely expected, routine event in a console application - the
/// user will mistype things regularly. Exceptions in C# are relatively
/// expensive and are best reserved for truly exceptional situations
/// (a corrupted file, a disk that ran out of space). Modeling "this
/// input was invalid" as a normal return value keeps the validation
/// path fast, easy to unit test (no try/catch required in tests), and
/// easy to read at the call site.
/// </summary>
public class ValidationResult
{
    /// <summary>True when every rule passed.</summary>
    public bool IsValid { get; }

    /// <summary>
    /// Human-readable error messages describing what is wrong.
    /// Empty when <see cref="IsValid"/> is true.
    /// </summary>
    public IReadOnlyList<string> Errors { get; }

    private ValidationResult(bool isValid, IReadOnlyList<string> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    /// <summary>Creates a successful result with no errors.</summary>
    public static ValidationResult Success() => new(true, Array.Empty<string>());

    /// <summary>Creates a failed result carrying one or more error messages.</summary>
    public static ValidationResult Failure(IEnumerable<string> errors) => new(false, errors.ToList());
}
