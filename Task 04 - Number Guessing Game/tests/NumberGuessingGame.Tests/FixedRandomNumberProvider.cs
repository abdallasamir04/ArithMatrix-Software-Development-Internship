using NumberGuessingGame.Services;

namespace NumberGuessingGame.Tests;

/// <summary>
/// Test double for <see cref="IRandomNumberProvider"/> that always
/// returns the same, pre-configured value, regardless of the requested
/// range. Used to make tests that exercise the constructor path which
/// goes through IRandomNumberProvider fully deterministic.
/// </summary>
internal sealed class FixedRandomNumberProvider : IRandomNumberProvider
{
    private readonly int _fixedValue;

    public FixedRandomNumberProvider(int fixedValue)
    {
        _fixedValue = fixedValue;
    }

    public int Next(int minInclusive, int maxInclusive) => _fixedValue;
}
