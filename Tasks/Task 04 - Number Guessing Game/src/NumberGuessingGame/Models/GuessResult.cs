namespace NumberGuessingGame.Models;

/// <summary>
/// The possible outcomes of evaluating a single, already-parsed guess
/// against the secret number.
///
/// Note: "invalid input" (non-numeric text, empty input) is handled
/// separately at the UI/parsing layer and is NOT part of this enum,
/// because an unparsable string is not a guess yet - it never reaches
/// the game engine. This keeps the game engine free of string-handling
/// concerns (Separation of Concerns).
/// </summary>
public enum GuessOutcome
{
    TooLow,
    TooHigh,
    Correct,
    OutOfRange
}

/// <summary>
/// The result of evaluating one guess: what happened, and how many
/// attempts have been made so far (including this one, if it counted).
/// Immutable value returned by <see cref="Services.NumberGuessingEngine"/>.
/// </summary>
public sealed class GuessResult
{
    public GuessOutcome Outcome { get; }
    public int AttemptsSoFar { get; }

    public GuessResult(GuessOutcome outcome, int attemptsSoFar)
    {
        Outcome = outcome;
        AttemptsSoFar = attemptsSoFar;
    }
}
