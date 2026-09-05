using FileOrganizer.Configuration;
using FileOrganizer.Logging;
using FileOrganizer.Models;
using FileOrganizer.Services;

// Exit codes: 0 = success, 1 = bad usage / invalid arguments, 2 = runtime failure.
const int ExitSuccess = 0;
const int ExitInvalidUsage = 1;
const int ExitRuntimeError = 2;

if (!CommandLineOptions.TryParse(args, out var options, out var parseError))
{
    Console.Error.WriteLine($"Argument error: {parseError}");
    Console.Error.WriteLine();
    PrintUsage();
    return ExitInvalidUsage;
}

var logger = new OrganizerLogger(options!.LogFilePath);
logger.LogHeader(options);

var classifier = new FileClassifier();
var conflictResolver = new ConflictResolver();
var service = new FileOrganizerService(classifier, conflictResolver);

try
{
    var results = service.Organize(options, logger);

    var scanned = results.Count;
    var organized = results.Count(r => r.Status is OperationStatus.Moved or OperationStatus.Planned);
    var errors = results.Count(r => r.Status == OperationStatus.Error);

    logger.LogSummary(scanned, organized, errors);
    logger.FlushToFile();

    return errors > 0 ? ExitRuntimeError : ExitSuccess;
}
catch (Exception ex) when (ex is DirectoryNotFoundException or InvalidOperationException or IOException)
{
    // Expected, "user fixable" failures (bad paths, source==target, etc.):
    // report cleanly without a stack trace.
    Console.Error.WriteLine($"Error: {ex.Message}");
    logger.FlushToFile();
    return ExitRuntimeError;
}

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run -- --source <path> --target <path> [--dry-run] [--recursive] [--log <path>]");
    Console.WriteLine("  dotnet run -- --config <path-to-config-file>");
    Console.WriteLine();
    Console.WriteLine("Examples:");
    Console.WriteLine("  dotnet run -- --source \"C:\\Users\\Me\\Downloads\" --target \"C:\\Users\\Me\\Organized\"");
    Console.WriteLine("  dotnet run -- --source \"C:\\Users\\Me\\Downloads\" --target \"C:\\Users\\Me\\Organized\" --dry-run");
}
