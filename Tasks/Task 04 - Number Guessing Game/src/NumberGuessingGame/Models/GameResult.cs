namespace NumberGuessingGame.Models;

/// <summary>
/// The final summary of a completed (won) game.
/// Produced once by the game engine when the player guesses correctly,
/// and displayed by the UI layer at the end of the session.
/// </summary>
public sealed class GameResult
{
    public int SecretNumber { get; }
    public int Attempts { get; }
    public bool IsWon { get; }

    public GameResult(int secretNumber, int attempts, bool isWon)
    {
        SecretNumber = secretNumber;
        Attempts = attempts;
        IsWon = isWon;
    }
}
