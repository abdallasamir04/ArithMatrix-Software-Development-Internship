namespace NumberGuessingGame.Services;

/// <summary>
/// Abstraction over "give me a random integer in a range".
///
/// Why this interface exists:
/// The game engine needs a secret number, but automated tests cannot
/// rely on an unpredictable value - a test that says "generate a random
/// number and hope it equals X" is not reliable. By hiding the actual
/// random source behind this one-method interface, production code can
/// use a real <see cref="System.Random"/>-backed implementation, while
/// tests can use a fake that always returns a known, fixed number.
/// This is the simplest possible seam for testable randomness - no
/// dependency-injection framework is needed for a single interface.
/// </summary>
public interface IRandomNumberProvider
{
    /// <summary>
    /// Returns a random integer that is greater than or equal to
    /// <paramref name="minInclusive"/> and less than or equal to
    /// <paramref name="maxInclusive"/> (both bounds inclusive).
    /// </summary>
    int Next(int minInclusive, int maxInclusive);
}
