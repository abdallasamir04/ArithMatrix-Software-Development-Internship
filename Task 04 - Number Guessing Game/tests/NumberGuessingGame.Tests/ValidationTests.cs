using NumberGuessingGame.Models;
using NumberGuessingGame.Services;
using Xunit;

namespace NumberGuessingGame.Tests;

/// <summary>
/// Tests focused on validation-related behavior: attempt counting policy,
/// out-of-range guesses, and range/settings validation. These tests
/// document and lock in the exact rules described in the README's
/// "Attempt Counting" and "Range Validation" sections.
/// </summary>
public class ValidationTests
{
    [Fact]
    public void ValidGuess_IncrementsAttemptCount()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);

        engine.EvaluateGuess(10);

        Assert.Equal(1, engine.Attempts);
    }

    [Fact]
    public void MultipleValidGuesses_AccumulateAttemptCount()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);

        engine.EvaluateGuess(10);
        engine.EvaluateGuess(20);
        engine.EvaluateGuess(30);

        Assert.Equal(3, engine.Attempts);
    }

    [Fact]
    public void OutOfRangeGuess_IsRejectedButStillCounted()
    {
        // Policy: a guess outside the configured range is still a valid,
        // deliberate integer guess (unlike unparsable text), so it counts
        // as an attempt but does not resolve the game.
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);

        GuessResult result = engine.EvaluateGuess(999);

        Assert.Equal(GuessOutcome.OutOfRange, result.Outcome);
        Assert.Equal(1, engine.Attempts);
        Assert.False(engine.IsOver);
    }

    [Fact]
    public void MinimumBoundary_IsAValidCorrectGuess()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 1);

        GuessResult result = engine.EvaluateGuess(1);

        Assert.Equal(GuessOutcome.Correct, result.Outcome);
    }

    [Fact]
    public void MaximumBoundary_IsAValidCorrectGuess()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 100);

        GuessResult result = engine.EvaluateGuess(100);

        Assert.Equal(GuessOutcome.Correct, result.Outcome);
    }

    [Fact]
    public void InvalidRange_MinimumEqualsMaximum_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new GameSettings(50, 50, Difficulty.Medium));
    }

    [Fact]
    public void InvalidRange_MinimumGreaterThanMaximum_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new GameSettings(100, 1, Difficulty.Medium));
    }

    [Theory]
    [InlineData(Difficulty.Easy, 1, 50)]
    [InlineData(Difficulty.Medium, 1, 100)]
    [InlineData(Difficulty.Hard, 1, 500)]
    public void ForDifficulty_ReturnsExpectedRange(Difficulty difficulty, int expectedMin, int expectedMax)
    {
        GameSettings settings = GameSettings.ForDifficulty(difficulty);

        Assert.Equal(expectedMin, settings.Minimum);
        Assert.Equal(expectedMax, settings.Maximum);
    }
}
