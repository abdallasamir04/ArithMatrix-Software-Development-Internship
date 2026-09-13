using System.Text;
using System.Text.Json;
using ContactManagement.Models;

namespace ContactManagement.Persistence;

/// <summary>
/// Implements <see cref="IContactRepository"/> using a single JSON
/// file on disk, read and written with System.Text.Json (the modern,
/// built-in .NET JSON library - no third-party package such as
/// Newtonsoft.Json is needed for a data shape this simple).
///
/// JSON serialization/deserialization, explained:
///   C# object (List&lt;Contact&gt;)
///        |  JsonSerializer.Serialize
///        v
///   JSON text (a string like: [ { "id": 1, "fullName": "..." } ])
///        |  File write
///        v
///   contacts.json on disk
///
///   contacts.json on disk
///        |  File read
///        v
///   JSON text
///        |  JsonSerializer.Deserialize
///        v
///   C# object (List&lt;Contact&gt;)
///
/// Serialization is the process of turning an in-memory .NET object
/// into a text representation (JSON) that can be written to a file.
/// Deserialization is the reverse: turning that text back into real
/// .NET objects with the correct types.
/// </summary>
public class JsonContactRepository : IContactRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonContactRepository(string filePath)
    {
        _filePath = filePath;

        // WriteIndented: makes the JSON file human-readable, which
        // matters because the AVIP task explicitly wants the storage
        // file to be easy to inspect and demonstrate on GitHub.
        //
        // PropertyNamingPolicy = CamelCase: produces "fullName" /
        // "phone" / "email" instead of the C# property names
        // "FullName" / "Phone" / "Email", matching the conceptual
        // JSON structure shown in the AVIP task description.
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public List<Contact> LoadAll()
    {
        EnsureDirectoryExists();

        if (!File.Exists(_filePath))
        {
            // First run, or the file was deleted: this is a normal
            // situation, not an error. Start with an empty address book.
            return new List<Contact>();
        }

        string jsonText;
        try
        {
            jsonText = File.ReadAllText(_filePath, Encoding.UTF8);
        }
        catch (IOException ex)
        {
            throw new PersistenceException(
                $"Could not read the data file at '{_filePath}'. Check that it is not open in another program and that you have permission to read it.",
                ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new PersistenceException(
                $"You do not have permission to read the data file at '{_filePath}'.",
                ex);
        }

        if (string.IsNullOrWhiteSpace(jsonText))
        {
            // An empty file is treated the same as "no contacts yet",
            // not as an error - a user could have created an empty
            // file by accident, and refusing to start over that is
            // unnecessarily unfriendly.
            return new List<Contact>();
        }

        try
        {
            var contacts = JsonSerializer.Deserialize<List<Contact>>(jsonText, _jsonOptions);
            return contacts ?? new List<Contact>();
        }
        catch (JsonException ex)
        {
            // Deliberate design decision: on malformed JSON we throw
            // rather than silently starting from an empty list. If we
            // silently treated a corrupted file as "no contacts", the
            // very next save would overwrite the corrupted file with
            // an empty one - permanently destroying whatever data the
            // user had, without them ever being told. Throwing lets
            // Program.cs / ConsoleMenu show a clear message and stop,
            // giving the user a chance to inspect or back up the file
            // before anything is overwritten.
            throw new PersistenceException(
                $"The data file at '{_filePath}' contains invalid JSON and could not be read. " +
                "The file was left untouched. Fix or remove it manually, then restart the application.",
                ex);
        }
    }

    public void SaveAll(IReadOnlyList<Contact> contacts)
    {
        EnsureDirectoryExists();

        string jsonText;
        try
        {
            jsonText = JsonSerializer.Serialize(contacts, _jsonOptions);
        }
        catch (Exception ex) when (ex is NotSupportedException)
        {
            throw new PersistenceException("Contacts could not be converted to JSON.", ex);
        }

        // Safe write strategy: write to a temporary file first, then
        // atomically replace the real file. WHY: if the application
        // is interrupted (crash, power loss) while writing directly
        // to contacts.json, the file could be left half-written and
        // unreadable, destroying all previously saved contacts. By
        // writing to a temp file and only replacing the real file
        // once the write has fully succeeded, contacts.json is never
        // observed in a partially written state.
        var tempFilePath = _filePath + ".tmp";

        try
        {
            File.WriteAllText(tempFilePath, jsonText, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

            if (File.Exists(_filePath))
            {
                File.Replace(tempFilePath, _filePath, destinationBackupFileName: null);
            }
            else
            {
                File.Move(tempFilePath, _filePath);
            }
        }
        catch (IOException ex)
        {
            throw new PersistenceException(
                $"Could not save changes to '{_filePath}'. Check that it is not open in another program and that the disk is not full.",
                ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new PersistenceException(
                $"You do not have permission to write to '{_filePath}'.",
                ex);
        }
        finally
        {
            // Clean up the temp file if something went wrong after it
            // was created but before it replaced the real file.
            if (File.Exists(tempFilePath))
            {
                try { File.Delete(tempFilePath); } catch { /* best effort cleanup */ }
            }
        }
    }

    private void EnsureDirectoryExists()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
