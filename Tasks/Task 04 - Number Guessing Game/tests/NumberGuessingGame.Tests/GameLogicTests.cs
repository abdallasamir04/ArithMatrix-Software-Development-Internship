using NumberGuessingGame.Models;
using NumberGuessingGame.Services;
using Xunit;

namespace NumberGuessingGame.Tests;

/// <summary>
/// Tests the core rules of <see cref="NumberGuessingEngine"/>: comparing
/// guesses to the secret number and detecting the correct guess.
/// All tests use the "known secret number" constructor so the secret
/// value is fixed and the test result never depends on chance.
/// </summary>
public class GameLogicTests
{
    [Fact]
    public void GuessBelowSecretNumber_ReturnsTooLow()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);

        GuessResult result = engine.EvaluateGuess(25);

        Assert.Equal(GuessOutcome.TooLow, result.Outcome);
    }

    [Fact]
    public void GuessAboveSecretNumber_ReturnsTooHigh()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);

        GuessResult result = engine.EvaluateGuess(75);

        Assert.Equal(GuessOutcome.TooHigh, result.Outcome);
    }

    [Fact]
    public void CorrectGuess_ReturnsCorrect()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);

        GuessResult result = engine.EvaluateGuess(50);

        Assert.Equal(GuessOutcome.Correct, result.Outcome);
    }

    [Fact]
    public void CorrectGuess_MarksGameAsOver()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);

        engine.EvaluateGuess(50);

        Assert.True(engine.IsOver);
    }

    [Fact]
    public void GuessAfterGameIsOver_ThrowsInvalidOperationException()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);
        engine.EvaluateGuess(50);

        Assert.Throws<InvalidOperationException>(() => engine.EvaluateGuess(10));
    }

    [Fact]
    public void BuildResult_BeforeGameIsWon_ThrowsInvalidOperationException()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);

        Assert.Throws<InvalidOperationException>(() => engine.BuildResult());
    }

    [Fact]
    public void GameCompletion_ProducesCorrectGameResult()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var engine = new NumberGuessingEngine(settings, knownSecretNumber: 50);

        engine.EvaluateGuess(25); // too low
        engine.EvaluateGuess(75); // too high
        engine.EvaluateGuess(50); // correct

        GameResult result = engine.BuildResult();

        Assert.Equal(50, result.SecretNumber);
        Assert.Equal(3, result.Attempts);
        Assert.True(result.IsWon);
    }

    [Fact]
    public void Constructor_WithSecretNumberOutsideRange_ThrowsArgumentOutOfRangeException()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new NumberGuessingEngine(settings, knownSecretNumber: 200));
    }

    [Fact]
    public void RandomProviderConstructor_UsesValueFromProvider()
    {
        var settings = new GameSettings(1, 100, Difficulty.Medium);
        var fixedProvider = new FixedRandomNumberProvider(42);
        var engine = new NumberGuessingEngine(settings, fixedProvider);

        GuessResult result = engine.EvaluateGuess(42);

        Assert.Equal(GuessOutcome.Correct, result.Outcome);
    }
}
