using ContactManagement.Persistence;
using ContactManagement.Services;
using ContactManagement.UI;

namespace ContactManagement;

/// <summary>
/// Application entry point and composition root.
///
/// "Composition root" is the one place in the application where all
/// the concrete pieces are actually wired together: this is the only
/// file that knows the storage is JSON (JsonContactRepository) and
/// the UI is a console menu (ConsoleMenu). Every other class depends
/// only on abstractions (IContactRepository) or is a plain data/
/// business class with no knowledge of the others. This is a
/// lightweight, manual form of Dependency Injection: instead of using
/// a DI framework (unnecessary for a project this size), the
/// dependencies are simply "new"-ed up once, here, and passed into
/// the classes that need them via their constructors.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
		// Force the console to use UTF-8 encoding to support Arabic and special characters
		Console.OutputEncoding = System.Text.Encoding.UTF8;

        var dataFilePath = ResolveDataFilePath(args);

        Console.WriteLine($"Using data file: {dataFilePath}");
        Console.WriteLine();

        IContactRepository repository = new JsonContactRepository(dataFilePath);

        ContactService contactService;
        try
        {
            contactService = new ContactService(repository);
        }
        catch (PersistenceException ex)
        {
            // A malformed/unreadable data file is treated as fatal at
            // startup rather than silently continuing with an empty
            // in-memory list - continuing would risk the very next
            // save overwriting the user's real (but corrupted) file
            // with an empty one, permanently losing their data.
            Console.WriteLine("A problem occurred while loading contacts:");
            Console.WriteLine(ex.Message);
            Console.WriteLine("The application will now exit without making any changes.");
            return;
        }

        var menu = new ConsoleMenu(contactService);
        menu.Run();
    }

    /// <summary>
    /// [OPTIONAL ENHANCEMENT] Determines the data file path.
    /// By default, contacts are stored at "data/contacts.json"
    /// relative to the application's working directory, which keeps
    /// the project fully portable - no personal or machine-specific
    /// absolute paths are ever hard-coded. Optionally, a custom path
    /// can be supplied with "--data <path>", for example when
    /// running automated demonstrations against a separate file.
    /// </summary>
    private static string ResolveDataFilePath(string[] args)
    {
        for (var i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "--data")
            {
                return args[i + 1];
            }
        }

        return Path.Combine("data", "contacts.json");
    }
}
