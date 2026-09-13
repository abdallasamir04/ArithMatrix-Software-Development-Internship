using NumberGuessingGame.Models;

namespace NumberGuessingGame.Services;

/// <summary>
/// Contains all the RULES of the Number Guessing Game:
/// generating (or receiving) the secret number, evaluating guesses,
/// counting attempts, and knowing whether the game is over.
///
/// This class deliberately knows nothing about the console. It never
/// calls Console.WriteLine or Console.ReadLine. That separation is what
/// makes it possible to unit-test the entire game engine without a
/// real terminal, and it is why this class lives in "Services" rather
/// than "UI".
///
/// What this class is NOT responsible for:
/// - Printing anything.
/// - Reading or parsing user input (that is the UI layer's job -
///   by the time a guess reaches EvaluateGuess, it is already a valid int).
/// - Deciding what happens with invalid text input.
/// </summary>
public sealed class NumberGuessingEngine
{
    private readonly GameSettings _settings;
    private readonly int _secretNumber;
    private int _attempts;
    private bool _isOver;

    /// <summary>
    /// Number of valid guesses made so far.
    /// </summary>
    public int Attempts => _attempts;

    /// <summary>
    /// True once the player has guessed the secret number.
    /// </summary>
    public bool IsOver => _isOver;

    /// <summary>
    /// Starts a new game: generates a secret number inside the
    /// configured range using the supplied random provider.
    /// </summary>
    /// <param name="settings">The validated range/difficulty for this game.</param>
    /// <param name="randomProvider">
    /// Source of randomness. In production this is <see cref="RandomNumberProvider"/>;
    /// in tests it can be a fake that returns a fixed value.
    /// </param>
    public NumberGuessingEngine(GameSettings settings, IRandomNumberProvider randomProvider)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(randomProvider);

        _settings = settings;
        _secretNumber = randomProvider.Next(settings.Minimum, settings.Maximum);
        _attempts = 0;
        _isOver = false;
    }

    /// <summary>
    /// Alternate constructor used directly by tests: lets a test supply
    /// the secret number explicitly instead of going through a random
    /// provider. This keeps guess-evaluation tests simple and obviously
    /// deterministic, without needing a fake random object for every test.
    /// </summary>
    /// <param name="settings">The validated range/difficulty for this game.</param>
    /// <param name="knownSecretNumber">
    /// The exact secret number to use. Must fall within the settings' range.
    /// </param>
    public NumberGuessingEngine(GameSettings settings, int knownSecretNumber)
    {
        ArgumentNullException.ThrowIfNull(settings);

        if (knownSecretNumber < settings.Minimum || knownSecretNumber > settings.Maximum)
        {
            throw new ArgumentOutOfRangeException(
                nameof(knownSecretNumber),
                knownSecretNumber,
                $"Secret number must be between {settings.Minimum} and {settings.Maximum}.");
        }

        _settings = settings;
        _secretNumber = knownSecretNumber;
        _attempts = 0;
        _isOver = false;
    }

    /// <summary>
    /// Evaluates one already-parsed guess.
    ///
    /// Attempt-counting policy (see README "Attempt Counting" section
    /// for the full justification): a guess that reaches this method is,
    /// by definition, valid numeric input. It always increments the
    /// attempt counter - even when it is outside the configured range -
    /// because the player did make a deliberate, countable guess.
    /// Only text the UI layer could not parse at all (e.g. "abc") never
    /// reaches this method and is therefore never counted.
    /// </summary>
    /// <param name="guess">A parsed integer guess.</param>
    /// <returns>The outcome of this guess plus the running attempt count.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if called after the game has already been won.
    /// </exception>
    public GuessResult EvaluateGuess(int guess)
    {
        if (_isOver)
        {
            throw new InvalidOperationException("The game is already over; no further guesses are allowed.");
        }

        if (guess < _settings.Minimum || guess > _settings.Maximum)
        {
            _attempts++;
            return new GuessResult(GuessOutcome.OutOfRange, _attempts);
        }

        _attempts++;

        if (guess < _secretNumber)
        {
            return new GuessResult(GuessOutcome.TooLow, _attempts);
        }

        if (guess > _secretNumber)
        {
            return new GuessResult(GuessOutcome.TooHigh, _attempts);
        }

        _isOver = true;
        return new GuessResult(GuessOutcome.Correct, _attempts);
    }

    /// <summary>
    /// Builds the final summary once the game is over.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the game has not been won yet.
    /// </exception>
    public GameResult BuildResult()
    {
        if (!_isOver)
        {
            throw new InvalidOperationException("Cannot build a result before the game has been won.");
        }

        return new GameResult(_secretNumber, _attempts, isWon: true);
    }
}
