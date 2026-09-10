namespace ContactManagement.Services;

/// <summary>
/// Represents the outcome of a business operation (add / edit /
/// delete a contact) that can fail for an expected business reason
/// (validation failed, duplicate found, contact not found).
///
/// WHY a result object instead of exceptions for these cases: a
/// duplicate phone number or an invalid email is not a programming
/// error - it is a completely normal outcome that the UI needs to
/// react to (show a message, let the user try again). Using
/// exceptions for routine, expected outcomes is considered poor
/// practice in .NET because exceptions are meant for exceptional,
/// unanticipated situations (see PersistenceException, which IS used
/// for genuine I/O failures). Returning a result keeps ContactService
/// easy to unit test: a test simply asserts on IsSuccess/ErrorMessage
/// instead of wrapping calls in try/catch.
/// </summary>
public class ServiceResult
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    protected ServiceResult(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult Success() => new(true, null);
    public static ServiceResult Failure(string errorMessage) => new(false, errorMessage);
}

/// <summary>
/// Same as <see cref="ServiceResult"/> but also carries a value on
/// success (for example, the newly created Contact including its
/// generated Id).
/// </summary>
public class ServiceResult<T> : ServiceResult
{
    public T? Value { get; }

    private ServiceResult(bool isSuccess, string? errorMessage, T? value)
        : base(isSuccess, errorMessage)
    {
        Value = value;
    }

    public static ServiceResult<T> Success(T value) => new(true, null, value);
    public static new ServiceResult<T> Failure(string errorMessage) => new(false, errorMessage, default);
}
