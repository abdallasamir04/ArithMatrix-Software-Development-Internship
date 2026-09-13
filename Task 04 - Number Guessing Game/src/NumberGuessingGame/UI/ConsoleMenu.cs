using NumberGuessingGame.Models;
using NumberGuessingGame.Services;

namespace NumberGuessingGame.UI;

/// <summary>
/// Owns everything printed to and read from the console: the main menu,
/// the instructions screen, difficulty selection, and the guessing loop
/// itself. It drives a <see cref="NumberGuessingEngine"/> instance but
/// contains none of the game rules - it only decides what to say to the
/// player based on the outcome the engine returns.
/// </summary>
public sealed class ConsoleMenu
{
    private readonly IRandomNumberProvider _randomProvider;

    public ConsoleMenu(IRandomNumberProvider randomProvider)
    {
        _randomProvider = randomProvider;
    }

    public void Run()
    {
        bool exit = false;

        while (!exit)
        {
            PrintTitle();
            Console.WriteLine("1. Start Game");
            Console.WriteLine("2. Instructions");
            Console.WriteLine("3. Exit");
            Console.WriteLine();

            int choice = ConsoleInputHelper.ReadMenuChoice("Choose an option: ", 1, 3);

            switch (choice)
            {
                case 1:
                    PlayGame();
                    break;
                case 2:
                    ShowInstructions();
                    break;
                case 3:
                    exit = true;
                    Console.WriteLine("Thanks for playing. Goodbye!");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void PrintTitle()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("       NUMBER GUESSING GAME");
        Console.WriteLine("========================================");
    }

    // [OPTIONAL ENHANCEMENT] Instructions screen - not required by AVIP,
    // added to improve first-time user experience.
    private static void ShowInstructions()
    {
        Console.WriteLine();
        Console.WriteLine("How to play:");
        Console.WriteLine("1. Select a difficulty.");
        Console.WriteLine("2. The computer chooses a secret number in that range.");
        Console.WriteLine("3. Enter your guess.");
        Console.WriteLine("4. You will be told whether your guess is too high or too low.");
        Console.WriteLine("5. Continue until you find the correct number.");
        Console.WriteLine("6. Your attempts will be counted and shown at the end.");
    }

    private GameSettings ChooseDifficulty()
    {
        Console.WriteLine();
        Console.WriteLine("Select difficulty:");
        Console.WriteLine("1. Easy   (1-50)");
        Console.WriteLine("2. Medium (1-100)");
        Console.WriteLine("3. Hard   (1-500)");
        Console.WriteLine();

        int choice = ConsoleInputHelper.ReadMenuChoice("Choice: ", 1, 3);

        Difficulty difficulty = choice switch
        {
            1 => Difficulty.Easy,
            2 => Difficulty.Medium,
            _ => Difficulty.Hard
        };

        return GameSettings.ForDifficulty(difficulty);
    }

    private void PlayGame()
    {
        GameSettings settings = ChooseDifficulty();
        var engine = new NumberGuessingEngine(settings, _randomProvider);

        Console.WriteLine();
        Console.WriteLine($"I have selected a number between {settings.Minimum} and {settings.Maximum}.");
        Console.WriteLine();

        while (!engine.IsOver)
        {
            int guess = ConsoleInputHelper.ReadInt("Enter your guess: ");
            GuessResult result = engine.EvaluateGuess(guess);

            switch (result.Outcome)
            {
                case GuessOutcome.TooLow:
                    Console.WriteLine("Too low!");
                    break;
                case GuessOutcome.TooHigh:
                    Console.WriteLine("Too high!");
                    break;
                case GuessOutcome.OutOfRange:
                    Console.WriteLine(
                        $"Please guess a number between {settings.Minimum} and {settings.Maximum}.");
                    break;
                case GuessOutcome.Correct:
                    Console.WriteLine("Correct!");
                    break;
            }
        }

        GameResult finalResult = engine.BuildResult();
        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("GAME OVER");
        Console.WriteLine("========================================");
        Console.WriteLine($"Secret Number: {finalResult.SecretNumber}");
        Console.WriteLine($"Attempts: {finalResult.Attempts}");
    }
}
