namespace ContactManagement.UI;

/// <summary>
/// Small collection of console input helpers.
///
/// WHY this exists as its own class instead of scattering
/// Console.ReadLine() calls with manual parsing throughout
/// ConsoleMenu: every place that needs "read an integer from the
/// user" would otherwise repeat the same try/parse/retry logic. This
/// class exists purely to keep ConsoleMenu focused on the *flow* of
/// each screen, not the mechanics of reading and re-prompting for
/// input. It has no business rules of its own - it never validates
/// whether a phone number or email is acceptable business data; it
/// only guarantees "you will get back a syntactically usable value
/// (a non-null string, a real int) or the user explicitly cancelled".
/// </summary>
public static class ConsoleInputHelper
{
    /// <summary>
    /// Reads a line of text. Returns an empty string (never null) if
    /// the user just presses Enter, or if input was somehow
    /// unavailable (e.g. redirected input reached end-of-stream).
    /// </summary>
    public static string ReadLine()
    {
        return Console.ReadLine() ?? string.Empty;
    }

    /// <summary>
    /// Prompts repeatedly until the user enters a valid integer, or
    /// types an empty line to cancel. Returns null if the user
    /// cancelled.
    /// </summary>
    public static int? ReadOptionalInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadLine().Trim();

            if (input.Length == 0)
            {
                return null;
            }

            if (int.TryParse(input, out var value))
            {
                return value;
            }

            Console.WriteLine("Please enter a whole number, or press Enter to cancel.");
        }
    }

    /// <summary>
    /// Prompts for a yes/no confirmation. Only "y"/"yes" (case
    /// insensitive) counts as confirmed; everything else, including
    /// an empty line, is treated as "no" - this makes destructive
    /// actions like Delete safe by default.
    /// </summary>
    public static bool ReadYesNo(string prompt)
    {
        Console.Write(prompt);
        var input = ReadLine().Trim().ToLowerInvariant();
        return input is "y" or "yes";
    }

    /// <summary>Reads a raw string prompt without any validation (validation is ContactValidator's job).</summary>
    public static string ReadString(string prompt)
    {
        Console.Write(prompt);
        return ReadLine();
    }
}
