namespace NumberGuessingGame.UI;

/// <summary>
/// Small helper around reading integers and menu choices from the
/// console safely.
///
/// Why this exists: without it, every place that needs a number from
/// the user would have to repeat the same "read line, trim, TryParse,
/// loop on failure" logic. Centralizing it here avoids duplication and
/// keeps Program.cs / ConsoleMenu focused on the game flow rather than
/// low-level parsing.
///
/// This class intentionally does NOT know anything about game rules
/// (ranges, secret numbers, attempts). It only knows how to get a
/// syntactically valid integer or menu choice out of the console.
/// </summary>
public static class ConsoleInputHelper
{
    /// <summary>
    /// Prompts the user until they enter text that parses as an integer.
    /// Handles empty input, whitespace-only input, non-numeric text, and
    /// numbers too large/small to fit in an <see cref="int"/> (overflow),
    /// without ever throwing an exception or crashing the application.
    /// </summary>
    /// <param name="prompt">The message shown to the user before reading.</param>
    /// <returns>A successfully parsed integer.</returns>
    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                Console.WriteLine("Input cannot be empty. Please enter a whole number.");
                continue;
            }

            // int.TryParse never throws, even for text like "abc" or a
            // number far too large for an int (e.g. "999999999999999").
            // This is why TryParse is preferred over int.Parse for any
            // input that comes from a user rather than trusted code:
            // int.Parse would throw a FormatException/OverflowException
            // that we would then have to catch, using exceptions for
            // perfectly ordinary, expected input mistakes.
            if (int.TryParse(line.Trim(), out int value))
            {
                return value;
            }

            Console.WriteLine("That is not a valid whole number. Please try again.");
        }
    }

    /// <summary>
    /// Prompts the user to choose a menu option, restricted to the
    /// inclusive range [minChoice, maxChoice].
    /// </summary>
    public static int ReadMenuChoice(string prompt, int minChoice, int maxChoice)
    {
        while (true)
        {
            int choice = ReadInt(prompt);

            if (choice < minChoice || choice > maxChoice)
            {
                Console.WriteLine($"Please enter a number between {minChoice} and {maxChoice}.");
                continue;
            }

            return choice;
        }
    }
}
