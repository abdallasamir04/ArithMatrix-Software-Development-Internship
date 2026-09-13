namespace NumberGuessingGame.Models;

/// <summary>
/// Represents the difficulty level of the game.
/// Each difficulty maps to a specific numeric range.
/// </summary>
public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

/// <summary>
/// Holds the configuration for a single game session:
/// the inclusive minimum and maximum values of the guessing range,
/// and the difficulty level that produced that range.
///
/// This type is intentionally immutable (all properties are get-only,
/// assigned only through the constructor) because a game's range must
/// not change once the secret number has been generated from it.
/// </summary>
public sealed class GameSettings
{
    public int Minimum { get; }
    public int Maximum { get; }
    public Difficulty Difficulty { get; }

    /// <summary>
    /// Creates a new, validated game configuration.
    /// </summary>
    /// <param name="minimum">Inclusive lower bound of the guessing range.</param>
    /// <param name="maximum">Inclusive upper bound of the guessing range.</param>
    /// <param name="difficulty">The difficulty level associated with this range.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the range is invalid (minimum is not strictly less than maximum).
    /// </exception>
    public GameSettings(int minimum, int maximum, Difficulty difficulty)
    {
        if (minimum >= maximum)
        {
            throw new ArgumentException(
                $"Minimum ({minimum}) must be strictly less than Maximum ({maximum}).");
        }

        Minimum = minimum;
        Maximum = maximum;
        Difficulty = difficulty;
    }

    /// <summary>
    /// Returns the predefined settings for a given difficulty level.
    /// Easy = 1-50, Medium = 1-100, Hard = 1-500.
    /// These specific ranges are an engineering choice (not an official
    /// AVIP requirement) made to give clearly distinct difficulty curves.
    /// </summary>
    public static GameSettings ForDifficulty(Difficulty difficulty) => difficulty switch
    {
        Difficulty.Easy => new GameSettings(1, 50, Difficulty.Easy),
        Difficulty.Medium => new GameSettings(1, 100, Difficulty.Medium),
        Difficulty.Hard => new GameSettings(1, 500, Difficulty.Hard),
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, "Unknown difficulty level.")
    };
}
