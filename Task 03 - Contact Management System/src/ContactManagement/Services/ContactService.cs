using ContactManagement.Models;
using ContactManagement.Persistence;
using ContactManagement.Validation;

namespace ContactManagement.Services;

/// <summary>
/// Contains ALL business rules for managing contacts: creating,
/// reading, updating, deleting, searching, and enforcing the
/// duplicate policy. This is the only class in the project that
/// combines validation results with persistence - the UI layer
/// (ConsoleMenu) never talks to IContactRepository or
/// ContactValidator directly, it only talks to ContactService.
///
/// Duplicate policy (documented once, here, as the single source of
/// truth - the README simply restates this):
///   A contact is considered a duplicate of another contact if EITHER
///   its normalized email matches, OR its normalized phone matches,
///   an already-stored contact. When editing a contact, that same
///   contact is excluded from the comparison (otherwise every edit
///   would immediately "duplicate itself").
///
/// Why keep the whole contact list in memory (List&lt;Contact&gt;)
/// rather than re-reading the file on every single operation? For an
/// address book with a realistically small number of contacts (tens
/// to low thousands), holding them in memory is simpler, faster, and
/// avoids subtle bugs from partially re-reading the file mid-operation.
/// Every mutation (add/update/delete) immediately calls
/// SaveAll on the repository, so nothing is ever left unsaved.
/// </summary>
public class ContactService
{
    private readonly IContactRepository _repository;
    private readonly List<Contact> _contacts;
    private int _nextId;

    public ContactService(IContactRepository repository)
    {
        _repository = repository;
        _contacts = _repository.LoadAll();
        _nextId = _contacts.Count == 0 ? 1 : _contacts.Max(c => c.Id) + 1;
    }

    /// <summary>Returns every stored contact (a defensive copy, so callers cannot mutate internal state directly).</summary>
    public List<Contact> GetAll() => new(_contacts);

    /// <summary>Returns the contact with the given Id, or null if none exists.</summary>
    public Contact? GetById(int id) => _contacts.FirstOrDefault(c => c.Id == id);

    /// <summary>
    /// Validates, checks duplicates, creates and persists a new
    /// contact. Returns the created contact (including its generated
    /// Id) on success, or a failure result describing what went wrong.
    /// </summary>
    public ServiceResult<Contact> AddContact(string fullNameInput, string phoneInput, string emailInput)
    {
        var validation = ContactValidator.Validate(fullNameInput, phoneInput, emailInput);
        if (!validation.IsValid)
        {
            return ServiceResult<Contact>.Failure(string.Join(" ", validation.Errors));
        }

        var normalizedEmailForCompare = ContactValidator.NormalizeEmailForComparison(emailInput);
        var normalizedPhoneForCompare = ContactValidator.NormalizePhoneForComparison(phoneInput);

        var duplicate = FindDuplicate(normalizedEmailForCompare, normalizedPhoneForCompare, excludeId: null);
        if (duplicate is not null)
        {
            return ServiceResult<Contact>.Failure(BuildDuplicateMessage(duplicate, normalizedEmailForCompare, normalizedPhoneForCompare));
        }

        var contact = new Contact
        {
            Id = _nextId,
            FullName = ContactValidator.NormalizeName(fullNameInput),
            Phone = ContactValidator.NormalizePhoneForStorage(phoneInput),
            Email = ContactValidator.NormalizeEmailForStorage(emailInput)
        };

        _contacts.Add(contact);
        _nextId++;
        Persist();

        return ServiceResult<Contact>.Success(contact);
    }

    /// <summary>
    /// Validates, checks duplicates (excluding the contact being
    /// edited), updates and persists an existing contact.
    /// </summary>
    public ServiceResult UpdateContact(int id, string fullNameInput, string phoneInput, string emailInput)
    {
        var existing = GetById(id);
        if (existing is null)
        {
            return ServiceResult.Failure($"No contact found with Id {id}.");
        }

        var validation = ContactValidator.Validate(fullNameInput, phoneInput, emailInput);
        if (!validation.IsValid)
        {
            return ServiceResult.Failure(string.Join(" ", validation.Errors));
        }

        var normalizedEmailForCompare = ContactValidator.NormalizeEmailForComparison(emailInput);
        var normalizedPhoneForCompare = ContactValidator.NormalizePhoneForComparison(phoneInput);

        // Excluding the current contact's own Id is what prevents an
        // edit from being flagged as a "duplicate of itself" when the
        // user does not change the phone or email.
        var duplicate = FindDuplicate(normalizedEmailForCompare, normalizedPhoneForCompare, excludeId: id);
        if (duplicate is not null)
        {
            return ServiceResult.Failure(BuildDuplicateMessage(duplicate, normalizedEmailForCompare, normalizedPhoneForCompare));
        }

        existing.FullName = ContactValidator.NormalizeName(fullNameInput);
        existing.Phone = ContactValidator.NormalizePhoneForStorage(phoneInput);
        existing.Email = ContactValidator.NormalizeEmailForStorage(emailInput);

        Persist();

        return ServiceResult.Success();
    }

    /// <summary>Removes the contact with the given Id, if it exists, and persists the change.</summary>
    public ServiceResult DeleteContact(int id)
    {
        var existing = GetById(id);
        if (existing is null)
        {
            return ServiceResult.Failure($"No contact found with Id {id}.");
        }

        _contacts.Remove(existing);
        Persist();

        return ServiceResult.Success();
    }

    /// <summary>
    /// Searches contacts by the given field. Name and email matches
    /// are case-insensitive partial (substring) matches; phone
    /// matches are partial matches against the normalized (digits
    /// only) representation, so formatting differences do not prevent
    /// a match.
    /// </summary>
    public List<Contact> Search(SearchField field, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new List<Contact>();
        }

        var trimmedQuery = query.Trim();

        // LINQ's Where() filters a sequence down to only the elements
        // that satisfy a condition (a predicate), returning a new
        // lazy sequence; ToList() then forces evaluation into a
        // concrete list. This is equivalent to a manual loop such as:
        //
        //   var results = new List<Contact>();
        //   foreach (var c in _contacts)
        //       if (MatchesName(c, trimmedQuery)) results.Add(c);
        //
        // LINQ is preferred here because the intent ("give me the
        // contacts where the name contains the query") is expressed
        // directly, without the bookkeeping of a manual loop.
        return field switch
        {
            SearchField.Name => _contacts.Where(c => MatchesName(c, trimmedQuery)).ToList(),
            SearchField.Phone => _contacts.Where(c => MatchesPhone(c, trimmedQuery)).ToList(),
            SearchField.Email => _contacts.Where(c => MatchesEmail(c, trimmedQuery)).ToList(),
            SearchField.All => _contacts
                .Where(c => MatchesName(c, trimmedQuery) || MatchesPhone(c, trimmedQuery) || MatchesEmail(c, trimmedQuery))
                .ToList(),
            _ => new List<Contact>()
        };
    }

    private static bool MatchesName(Contact contact, string query) =>
        contact.FullName.Contains(query, StringComparison.OrdinalIgnoreCase);

    private static bool MatchesEmail(Contact contact, string query) =>
        contact.Email.Contains(query, StringComparison.OrdinalIgnoreCase);

    private static bool MatchesPhone(Contact contact, string query)
    {
        var normalizedStoredPhone = ContactValidator.NormalizePhoneForComparison(contact.Phone);
        var normalizedQuery = ContactValidator.NormalizePhoneForComparison(query);

        // If the query has no digits at all (e.g. the user typed only
        // letters into the phone search), fall back to a plain
        // substring match against the stored value so the search
        // never crashes and never silently "matches everything".
        if (string.IsNullOrEmpty(normalizedQuery))
        {
            return contact.Phone.Contains(query, StringComparison.OrdinalIgnoreCase);
        }

        return normalizedStoredPhone.Contains(normalizedQuery, StringComparison.Ordinal);
    }

    /// <summary>
    /// Looks for an existing contact whose normalized email or phone
    /// matches the given normalized values.
    /// LINQ's FirstOrDefault() returns the first element matching the
    /// predicate, or null (the default for a reference type) if none
    /// match - this is exactly the "does a duplicate exist?" question,
    /// expressed as a single expression instead of a manual loop with
    /// a "found" flag.
    /// </summary>
    private Contact? FindDuplicate(string normalizedEmail, string normalizedPhone, int? excludeId)
    {
        return _contacts.FirstOrDefault(c =>
            c.Id != excludeId &&
            (ContactValidator.NormalizeEmailForComparison(c.Email) == normalizedEmail ||
             ContactValidator.NormalizePhoneForComparison(c.Phone) == normalizedPhone));
    }

    private static string BuildDuplicateMessage(Contact duplicate, string normalizedEmail, string normalizedPhone)
    {
        var matchedOnEmail = ContactValidator.NormalizeEmailForComparison(duplicate.Email) == normalizedEmail;
        var matchedOnPhone = ContactValidator.NormalizePhoneForComparison(duplicate.Phone) == normalizedPhone;

        if (matchedOnEmail && matchedOnPhone)
        {
            return $"A contact with this email and phone number already exists ('{duplicate.FullName}', Id {duplicate.Id}).";
        }

        if (matchedOnEmail)
        {
            return $"A contact with this email address already exists ('{duplicate.FullName}', Id {duplicate.Id}).";
        }

        return $"A contact with this phone number already exists ('{duplicate.FullName}', Id {duplicate.Id}).";
    }

    private void Persist() => _repository.SaveAll(_contacts);
}
