namespace NumberGuessingGame.Services;

/// <summary>
/// Production implementation of <see cref="IRandomNumberProvider"/>,
/// backed by <see cref="System.Random"/>.
///
/// Important detail about the underlying API:
/// System.Random.Next(int minValue, int maxValue) treats maxValue as
/// EXCLUSIVE. To make our own Next(minInclusive, maxInclusive) behave
/// with an INCLUSIVE upper bound (so that, for range 1-100, both 1 and
/// 100 are actually reachable), we must call the underlying API with
/// (maxInclusive + 1). Forgetting the "+1" is the classic off-by-one
/// mistake in guessing-game implementations - it silently makes the
/// maximum value impossible to generate.
/// </summary>
public sealed class RandomNumberProvider : IRandomNumberProvider
{
    private readonly Random _random = new();

    public int Next(int minInclusive, int maxInclusive)
    {
        if (minInclusive > maxInclusive)
        {
            throw new ArgumentException(
                $"minInclusive ({minInclusive}) cannot be greater than maxInclusive ({maxInclusive}).");
        }

        // System.Random.Next(min, max) excludes 'max', so we add 1
        // to make our own contract inclusive on both ends.
        return _random.Next(minInclusive, maxInclusive + 1);
    }
}
