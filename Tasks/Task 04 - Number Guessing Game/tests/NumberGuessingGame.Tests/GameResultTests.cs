using NumberGuessingGame.Models;
using Xunit;

namespace NumberGuessingGame.Tests;

/// <summary>
/// Tests for the <see cref="GameResult"/> model in isolation - simple
/// checks that the constructor stores its values correctly. Kept
/// separate from GameLogicTests because it targets the model's own
/// contract rather than the engine's behavior.
/// </summary>
public class GameResultTests
{
    [Fact]
    public void Constructor_StoresAllValuesCorrectly()
    {
        var result = new GameResult(secretNumber: 42, attempts: 5, isWon: true);

        Assert.Equal(42, result.SecretNumber);
        Assert.Equal(5, result.Attempts);
        Assert.True(result.IsWon);
    }
}
