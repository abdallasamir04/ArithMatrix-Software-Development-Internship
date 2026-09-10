using ContactManagement.Models;
using ContactManagement.Services;

namespace ContactManagement.UI;

/// <summary>
/// Owns the interactive console loop: displaying the menu, reading
/// the user's choice, and dispatching to the corresponding screen.
///
/// WHY the UI is kept "thin" (it only reads input, calls
/// ContactService, and prints results - it never validates data
/// itself and never touches the JSON file): this separation is what
/// makes ContactService independently testable. A unit test can
/// exercise "adding a duplicate contact is rejected" without ever
/// starting a console, reading a line, or printing anything, because
/// none of that logic lives here. If the console UI were replaced by
/// a web UI tomorrow, ContactService would not need to change.
/// </summary>
public class ConsoleMenu
{
    private readonly ContactService _contactService;

    public ConsoleMenu(ContactService contactService)
    {
        _contactService = contactService;
    }

    public void Run()
    {
        var running = true;
        while (running)
        {
            PrintMainMenu();
            var choice = ConsoleInputHelper.ReadString("Choose an option: ").Trim();

            switch (choice)
            {
                case "1": AddContact(); break;
                case "2": ViewContact(); break;
                case "3": ListContacts(); break;
                case "4": SearchContacts(); break;
                case "5": EditContact(); break;
                case "6": DeleteContact(); break;
                case "0": running = false; break;
                default:
                    Console.WriteLine("Invalid option. Please choose a number from the menu.");
                    break;
            }

            if (running)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine("Goodbye!");
    }

    private static void PrintMainMenu()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("     CONTACT MANAGEMENT SYSTEM");
        Console.WriteLine("========================================");
        Console.WriteLine("1. Add Contact");
        Console.WriteLine("2. View Contact");
        Console.WriteLine("3. List Contacts");
        Console.WriteLine("4. Search Contacts");
        Console.WriteLine("5. Edit Contact");
        Console.WriteLine("6. Delete Contact");
        Console.WriteLine("0. Exit");
        Console.WriteLine();
    }

    private void AddContact()
    {
        Console.WriteLine("--- Add Contact ---");
        var fullName = ConsoleInputHelper.ReadString("Full Name: ");
        var phone = ConsoleInputHelper.ReadString("Phone: ");
        var email = ConsoleInputHelper.ReadString("Email: ");

        var result = _contactService.AddContact(fullName, phone, email);

        if (result.IsSuccess)
        {
            Console.WriteLine($"Contact added successfully (Id {result.Value!.Id}).");
        }
        else
        {
            Console.WriteLine($"Could not add contact: {result.ErrorMessage}");
        }
    }

    private void ViewContact()
    {
        Console.WriteLine("--- View Contact ---");
        var id = ConsoleInputHelper.ReadOptionalInt("Enter contact Id (or press Enter to cancel): ");
        if (id is null)
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        var contact = _contactService.GetById(id.Value);
        if (contact is null)
        {
            Console.WriteLine($"Contact not found (Id {id.Value}).");
            return;
        }

        PrintContact(contact);
    }

    private void ListContacts()
    {
        Console.WriteLine("--- All Contacts ---");
        var contacts = _contactService.GetAll();
        PrintContactTable(contacts);
    }

    private void SearchContacts()
    {
        Console.WriteLine("--- Search Contacts ---");
        Console.WriteLine("1. Search by name");
        Console.WriteLine("2. Search by phone");
        Console.WriteLine("3. Search by email");
        Console.WriteLine("4. Search all fields"); // [ENGINEERING ENHANCEMENT]
        var choice = ConsoleInputHelper.ReadString("Choose a search type: ").Trim();

        SearchField field;
        switch (choice)
        {
            case "1": field = SearchField.Name; break;
            case "2": field = SearchField.Phone; break;
            case "3": field = SearchField.Email; break;
            case "4": field = SearchField.All; break;
            default:
                Console.WriteLine("Invalid search type.");
                return;
        }

        var query = ConsoleInputHelper.ReadString("Search text: ");
        var results = _contactService.Search(field, query);
        PrintContactTable(results);
    }

    private void EditContact()
    {
        Console.WriteLine("--- Edit Contact ---");
        var id = ConsoleInputHelper.ReadOptionalInt("Enter contact Id to edit (or press Enter to cancel): ");
        if (id is null)
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        var existing = _contactService.GetById(id.Value);
        if (existing is null)
        {
            Console.WriteLine($"Contact not found (Id {id.Value}).");
            return;
        }

        Console.WriteLine("Current values:");
        PrintContact(existing);
        Console.WriteLine();
        Console.WriteLine("Enter new values, or press Enter on a field to keep its current value.");

        var fullName = ReadWithDefault("Full Name", existing.FullName);
        var phone = ReadWithDefault("Phone", existing.Phone);
        var email = ReadWithDefault("Email", existing.Email);

        var result = _contactService.UpdateContact(id.Value, fullName, phone, email);

        if (result.IsSuccess)
        {
            Console.WriteLine("Contact updated successfully.");
        }
        else
        {
            Console.WriteLine($"Could not update contact: {result.ErrorMessage}");
        }
    }

    private void DeleteContact()
    {
        Console.WriteLine("--- Delete Contact ---");
        var id = ConsoleInputHelper.ReadOptionalInt("Enter contact Id to delete (or press Enter to cancel): ");
        if (id is null)
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        var existing = _contactService.GetById(id.Value);
        if (existing is null)
        {
            Console.WriteLine($"Contact not found (Id {id.Value}).");
            return;
        }

        PrintContact(existing);
        var confirmed = ConsoleInputHelper.ReadYesNo($"Are you sure you want to delete \"{existing.FullName}\"? (y/n): ");

        if (!confirmed)
        {
            Console.WriteLine("Deletion cancelled.");
            return;
        }

        var result = _contactService.DeleteContact(id.Value);
        Console.WriteLine(result.IsSuccess
            ? "Contact deleted successfully."
            : $"Could not delete contact: {result.ErrorMessage}");
    }

    /// <summary>
    /// Reads a new value for a field during editing, treating an
    /// empty line as "keep the current value".
    /// UX decision: this is far friendlier than forcing the user to
    /// retype every field just to change one of them, and it avoids
    /// accidentally erasing a field the user did not intend to touch.
    /// </summary>
    private static string ReadWithDefault(string label, string currentValue)
    {
        var input = ConsoleInputHelper.ReadString($"{label} [{currentValue}]: ");
        return input.Trim().Length == 0 ? currentValue : input;
    }

    private static void PrintContact(Contact contact)
    {
        Console.WriteLine($"Id:    {contact.Id}");
        Console.WriteLine($"Name:  {contact.FullName}");
        Console.WriteLine($"Phone: {contact.Phone}");
        Console.WriteLine($"Email: {contact.Email}");
    }

    private static void PrintContactTable(List<Contact> contacts)
    {
        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts found.");
            return;
        }

        Console.WriteLine($"{"Id",-5} {"Name",-25} {"Phone",-18} {"Email",-30}");
        Console.WriteLine(new string('-', 78));

        foreach (var contact in contacts)
        {
            Console.WriteLine($"{contact.Id,-5} {contact.FullName,-25} {contact.Phone,-18} {contact.Email,-30}");
        }
    }
}
