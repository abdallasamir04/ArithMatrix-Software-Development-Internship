using FileOrganizer.Models;

namespace FileOrganizer.Configuration;

/// <summary>
/// Parses raw <c>string[] args</c> from Main into a validated <see cref="OrganizerOptions"/>.
///
/// Supported flags:
///   --source &lt;path&gt;       (required, or via --config)
///   --target &lt;path&gt;       (required, or via --config)
///   --dry-run             (optional flag, no value)
///   --recursive           (optional flag, no value)
///   --log &lt;path&gt;          (optional, path to a log file)
///   --config &lt;path&gt;       (optional, a simple "key=value" text file; CLI flags override it)
///
/// Design choice: we do NOT take a dependency on a third-party argument-parsing
/// package (e.g. System.CommandLine) because the surface area here is tiny
/// (five flags) and a hand-written parser keeps the project dependency-free,
/// easy to read for a learner, and easy to unit test without process-level
/// argument plumbing.
/// </summary>
public static class CommandLineOptions
{
    /// <summary>
    /// Attempts to parse <paramref name="args"/> into an <see cref="OrganizerOptions"/>.
    /// Returns false and populates <paramref name="error"/> if parsing/validation fails.
    /// This method does NOT touch the filesystem beyond reading an optional config file;
    /// directory existence is validated later by the service layer.
    /// </summary>
    public static bool TryParse(string[] args, out OrganizerOptions? options, out string? error)
    {
        options = null;
        error = null;

        var values = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < args.Length; i++)
        {
            var token = args[i];

            switch (token)
            {
                case "--dry-run":
                    values["dry-run"] = "true";
                    break;

                case "--recursive":
                    values["recursive"] = "true";
                    break;

                case "--source":
                case "--target":
                case "--log":
                case "--config":
                    var key = token[2..]; // strip "--"
                    if (i + 1 >= args.Length)
                    {
                        error = $"Missing value for '{token}'.";
                        return false;
                    }
                    values[key] = args[++i];
                    break;

                default:
                    error = $"Unrecognized argument: '{token}'.";
                    return false;
            }
        }

        // If a config file was supplied, load it first, then let explicit
        // CLI flags (already in 'values') take precedence by only filling
        // in keys that are still missing.
        if (values.TryGetValue("config", out var configPath) && !string.IsNullOrWhiteSpace(configPath))
        {
            if (!TryLoadConfigFile(configPath, out var fileValues, out error))
            {
                return false;
            }

            foreach (var (key, value) in fileValues!)
            {
                values.TryAdd(key, value);
            }
        }

        if (!values.TryGetValue("source", out var source) || string.IsNullOrWhiteSpace(source))
        {
            error = "Missing required option: --source <path> (or 'source=' in the config file).";
            return false;
        }

        if (!values.TryGetValue("target", out var target) || string.IsNullOrWhiteSpace(target))
        {
            error = "Missing required option: --target <path> (or 'target=' in the config file).";
            return false;
        }

        var dryRun = IsFlagEnabled(values, "dry-run");
        var recursive = IsFlagEnabled(values, "recursive");
        values.TryGetValue("log", out var logPath);

        options = new OrganizerOptions
        {
            SourceDirectory = Path.GetFullPath(source),
            TargetDirectory = Path.GetFullPath(target),
            DryRun = dryRun,
            Recursive = recursive,
            LogFilePath = string.IsNullOrWhiteSpace(logPath) ? null : Path.GetFullPath(logPath)
        };

        return true;
    }

    /// <summary>
    /// Determines whether a boolean flag is "on". CLI flags (e.g. <c>--dry-run</c>)
    /// are stored with the literal value "true" when present. Config-file entries
    /// may instead spell out "dry-run=false" to explicitly turn a flag off even
    /// though the key is present, so an empty/"true"/"1" value counts as enabled
    /// and "false"/"0" counts as disabled.
    /// </summary>
    private static bool IsFlagEnabled(Dictionary<string, string?> values, string key)
    {
        if (!values.TryGetValue(key, out var value))
        {
            return false;
        }

        return string.IsNullOrEmpty(value)
            || value.Equals("true", StringComparison.OrdinalIgnoreCase)
            || value.Equals("1", StringComparison.Ordinal);
    }

    /// <summary>
    /// Loads a minimal "key=value" configuration file, one entry per line.
    /// Blank lines and lines starting with '#' are ignored. Recognized keys
    /// mirror the CLI flag names without the leading "--" (source, target,
    /// dry-run, recursive, log).
    /// </summary>
    private static bool TryLoadConfigFile(string path, out Dictionary<string, string?>? values, out string? error)
    {
        values = null;
        error = null;

        if (!File.Exists(path))
        {
            error = $"Config file not found: '{path}'.";
            return false;
        }

        var result = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawLine in File.ReadAllLines(path))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0)
            {
                error = $"Invalid line in config file (expected key=value): '{rawLine}'.";
                return false;
            }

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim();
            result[key] = value;
        }

        values = result;
        return true;
    }
}
