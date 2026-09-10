<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:0F2027,100:00C6FF&height=200&section=header&text=Contact%20Management%20System&fontSize=48&fontColor=FFFFFF&animation=fadeIn&fontAlignY=" />
</p>

<p align="center">
  <img src="https://readme-typing-svg.demolab.com?font=Fira+Code&pause=1200&color=00C6FF&center=true&vCenter=true&width=600&lines=Task+03+%7C+ArithMatrix+AVIP+2026;CRUD+Console+Application+with+JSON+Persistence" />
</p>

<div align="center">

[![Status](https://img.shields.io/badge/Status-Complete-success?style=flat-square)]()
[![Language](https://img.shields.io/badge/Language-C%23-239120?style=flat-square&logo=csharp&logoColor=white)]()
[![Framework](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)]()
[![Tests](https://img.shields.io/badge/Tests-49/49%20Passed-brightgreen?style=flat-square&logo=xunit)]()
[![Internship](https://img.shields.io/badge/AVIP-2026-blueviolet?style=flat-square)]()

**ArithMatrix Virtual Internship Program 2026 — Software Development Track**

</div>

<br />

## About The Project

The **Contact Management System** is a robust, command-line interface (CLI) application built with C# and .NET 8. It provides a complete CRUD (Create, Read, Update, Delete) solution for managing personal contacts with persistent JSON storage, comprehensive validation, and intelligent duplicate detection.

Designed with clean architecture and separation of concerns, this application features a layered design pattern with distinct Models, Validation, Persistence, Services, and UI layers—ensuring maintainability, testability, and professional software engineering practices.

---

## The Problem

- **Manual Contact Management**: Keeping track of contacts in scattered notes, emails, or paper lists is inefficient and error-prone.
- **Data Loss Risk**: Without persistent storage, contacts are lost when applications close or crash.
- **Duplicate Entries**: Accidentally adding the same contact multiple times creates confusion and data inconsistency.
- **No Validation**: Accepting invalid emails, phone numbers, or empty names corrupts data quality.
- **Lack of Search**: Finding specific contacts in a large list becomes tedious without filtering capabilities.

**The Solution**: A deterministic, user-friendly CLI application that manages contacts with full CRUD operations, automatic JSON persistence, input validation, duplicate detection (by phone or email), and multi-field search capabilities—all backed by 49 automated tests.

---

## Key Features

- **Complete CRUD Operations**: Add, view, list, search, edit, and delete contacts with clear confirmation messages.
- **Persistent JSON Storage**: All contacts are automatically saved to `data/contacts.json` and survive application restarts.
- **Comprehensive Validation**: Validates names (non-empty, ≤100 chars), phone numbers (7-15 digits, flexible formatting), and emails (practical pattern matching).
- **Intelligent Duplicate Detection**: Prevents adding contacts with duplicate phone numbers or emails (case-insensitive, formatting-insensitive).
- **Multi-Field Search**: Search by name, phone, email, or all fields simultaneously with case-insensitive partial matching.
- **Safe Edit Operations**: Editing a contact excludes itself from duplicate checks and allows keeping existing values via empty input.
- **Confirmation-Based Deletion**: Requires explicit `y/n` confirmation before deleting contacts to prevent accidental data loss.
- **Malformed JSON Protection**: Treats corrupted JSON files as fatal errors and refuses to overwrite them, protecting user data.
- **Automated Testing**: Backed by a comprehensive suite of 49 xUnit tests covering validation, business logic, persistence, and edge cases.

---

## Contact Fields & Validation

| Field | Validation Rules | Valid Examples | Invalid Examples |
| :--- | :--- | :--- | :--- |
| **Full Name** | Required, non-whitespace, ≤100 chars, any script/characters | `John Doe`, `أحمد محمد`, `Mary-Jane O'Brien` | `""`, `"   "` |
| **Phone** | Required, 7-15 digits, allows spaces/hyphens/leading `+` | `01001234567`, `+201001234567`, `0100 123 4567` | `"abc"`, `"123"` |
| **Email** | Required, ≤254 chars, practical `x@y.z` pattern | `john@example.com`, `user.name@domain.org` | `"notanemail"`, `"test@.com"` |

---

## How It Works

### Architecture Flow

```
User Input → ConsoleMenu → ContactService → ContactValidator
                                    ↓
                            IContactRepository
                                    ↓
                         JsonContactRepository
                                    ↓
                            contacts.json (JSON file)
```

### Runtime Process

1. **Startup**: `Program.cs` resolves the data file path (`data/contacts.json` or `--data <path>` override).
2. **Load**: `JsonContactRepository` loads existing contacts from JSON (or returns empty list if file missing/empty).
3. **Menu Loop**: `ConsoleMenu` displays the main menu and waits for user input.
4. **Operation Dispatch**: Based on user choice, dispatches to Add/View/List/Search/Edit/Delete operations.
5. **Validation**: `ContactValidator` checks input validity and normalizes values for comparison.
6. **Business Logic**: `ContactService` applies duplicate rules, generates IDs, and mutates the in-memory list.
7. **Persistence**: After every successful mutation, `SaveAll()` writes the complete list back to JSON using a temp-file-then-replace strategy.
8. **Confirmation**: Clear success/failure messages are displayed, then the menu loops back.
9. **Exit**: User selects "0. Exit" → application terminates gracefully.

---

## CLI Usage

### Standard Run
```bash
dotnet run --project src\ContactManagement\ContactManagement.csproj
```

### Custom Data File Path
```bash
dotnet run --project src\ContactManagement\ContactManagement.csproj -- --data "custom\path\contacts.json"
```

### Interactive Menu Operations

**Main Menu:**
```
========================================
     CONTACT MANAGEMENT SYSTEM
========================================
1. Add Contact
2. View Contact
3. List Contacts
4. Search Contacts
5. Edit Contact
6. Delete Contact
0. Exit

Choose an option:
```

**Add Contact Example:**
```
Full Name: Ahmed Mohamed
Phone: 01001234567
Email: ahmed@example.com

Contact added successfully (Id 1).
```

**Search Example:**
```
Search by:
1. Name
2. Phone
3. Email
4. All Fields

Choose search field: 1
Enter search query: Ahmed

Found 1 contact(s):
Id    Name              Phone          Email
1     Ahmed Mohamed     01001234567    ahmed@example.com
```

**Edit Contact Example:**
```
Enter contact ID to edit: 1
Current: Ahmed Mohamed | 01001234567 | ahmed@example.com

New Full Name (press Enter to keep current): 
New Phone (press Enter to keep current): 01112345678
New Email (press Enter to keep current): 

Contact updated successfully.
```

**Delete Confirmation:**
```
Are you sure you want to delete "Ahmed Mohamed"? (y/n): y

Contact deleted successfully.
```

---

## Duplicate Handling

The application enforces a strict **no-duplicate policy** for phone numbers and emails:

- **Detection Method**: Normalizes phone numbers (strips all non-digits) and emails (lowercase + trim) for comparison.
- **Formatting-Insensitive**: `0100 123 4567`, `0100-123-4567`, and `01001234567` are all recognized as the same phone number.
- **Case-Insensitive Emails**: `John@Example.com` and `john@example.com` are treated as duplicates.
- **Edit Self-Exclusion**: When editing a contact, the contact's own ID is excluded from duplicate checks (prevents flagging itself).
- **Cross-Contact Protection**: Changing Contact A's phone to match Contact B's phone is rejected.

**Example Rejection:**
```
Could not add contact: A contact with this phone number already exists ('John Doe', Id 1).
```

---

## JSON Storage Format

**Location**: `data/contacts.json` (relative to working directory)

**Structure**:
```json
[
  {
    "id": 1,
    "fullName": "Ahmed Mohamed",
    "phone": "01001234567",
    "email": "ahmed@example.com"
  },
  {
    "id": 2,
    "fullName": "Fatima Hassan",
    "phone": "01112345678",
    "email": "fatima@example.com"
  }
]
```

**Persistence Guarantees**:
- **Safe Writes**: Uses temp-file-then-replace strategy to prevent corruption on crash.
- **UTF-8 Encoding**: Supports Arabic, accented characters, and international scripts.
- **Indented JSON**: Human-readable formatting with camelCase property names.
- **Missing File Handling**: Creates directory and file automatically on first save.
- **Malformed JSON**: Throws `PersistenceException` and exits without touching the corrupted file (protects user data).

---

## Project Structure

```
Task 03 Contact Management/
├── src/
│   └── ContactManagement/
│       ├── Models/
│       │   ── Contact.cs                    # Contact data model
│       ├── Validation/
│       │   ├── ContactValidator.cs           # Input validation & normalization
│       │   └── ValidationResult.cs           # Validation success/failure wrapper
│       ├── Persistence/
│       │   ├── IContactRepository.cs         # Storage abstraction interface
│       │   ├── JsonContactRepository.cs      # JSON file implementation
│       │   └── PersistenceException.cs       # Custom exception for I/O errors
│       ├── Services/
│       │   ├── ContactService.cs             # Business logic & CRUD operations
│       │   ├── ServiceResult.cs              # Business operation result wrapper
│       │   └── SearchField.cs                # Enum for search field selection
│       ├── UI/
│       │   ├── ConsoleMenu.cs                # Interactive menu loop
│       │   └── ConsoleInputHelper.cs         # Input reading utilities
│       └── Program.cs                        # Application entry point & composition root
├── tests/
│   ── ContactManagement.Tests/
│       ├── ContactValidatorTests.cs          # Validation rule tests
│       ├── ContactServiceTests.cs            # Business logic tests
│       ├── JsonContactRepositoryTests.cs     # Persistence tests
│       ├── TestDoubles/
│       │   └── InMemoryContactRepository.cs  # In-memory test double
│       ── ContactManagement.Tests.csproj
── data/
│   └── contacts.json                         # Runtime data (not committed)
── examples/
│   └── sample_contacts.json                  # Fictional sample data
├── screenshots/                              # Visual proof of execution
├── ContactManagement.sln                     # Visual Studio Solution
── README.md                                 # This documentation
├── GUIDE.md                                  # Comprehensive technical guide
└── .gitignore                                # Git ignore rules
```

---

## Testing & Quality Assurance

The project includes a comprehensive automated test suite built with **xUnit**, utilizing temporary directories and in-memory test doubles to ensure isolation and reproducibility.

- **Total Tests**: 49
- **Passed**: 49 ✅
- **Failed**: 0
- **Skipped**: 0

**Test Coverage Includes:**

| Category | Test Count | Coverage |
| :--- | :--- | :--- |
| **Validation** | 9 tests | Empty names, invalid phones/emails, normalization, case-insensitivity |
| **CRUD Operations** | 25 tests | Add/View/List/Search/Edit/Delete with success and failure paths |
| **Duplicate Detection** | 7 tests | Phone/email duplicates, formatting variations, edit self-exclusion |
| **Persistence** | 7 tests | Missing files, empty files, malformed JSON, round-trip serialization |
| **Edge Cases** | 1 test | ID stability (deleted IDs not reused while higher IDs exist) |

**Run Tests Locally:**
```bash
dotnet test
```

**Sample Test Output:**
```
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.4.5+1caef2f33e (64-bit .NET 8.0.13)
[xUnit.net 00:00:01.47]   Discovering: ContactManagement.Tests
[xUnit.net 00:00:01.57]   Discovered:  ContactManagement.Tests
[xUnit.net 00:00:01.57]   Starting:    ContactManagement.Tests
[xUnit.net 00:00:01.81]   Finished:    ContactManagement.Tests
  ContactManagement.Tests test succeeded (5.1s)

Test summary: total: 49, failed: 0, succeeded: 49, skipped: 0, duration: 5.1s
```

---

## Screenshots

| Main Menu | First Run (Empty) |
| :---: | :---: |
| ![Main Menu](screenshots/first%20run%20without%20any%20thing.png) | ![Empty State](screenshots/first%20run%20without%20any%20thing.png) |

| Add Contact with JSON | List Contacts |
| :---: | :---: |
| ![Add Contact](screenshots/adding%20the%20data%20with%20json.png) | ![List Contacts](screenshots/show%20contacts.png) |

| Running Tests | 
| :---: |
| ![Tests](screenshots/running%20tests.png) |

---

## Installation & Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS version)
- Git

### 1. Clone the Repository
```bash
git clone https://github.com/abdallasamir04/ArithMatrix-Software-Development-Internship.git
```

### 2. Navigate to Task 03 Directory
```bash
cd "ArithMatrix-Software-Development-Internship/Task 03 Contact Management"
```

### 3. Restore Dependencies & Build
```bash
dotnet restore
dotnet build
```

### 4. Run Tests
```bash
dotnet test
```

### 5. Execute the Application
```bash
dotnet run --project src\ContactManagement\ContactManagement.csproj
```

### 6. (Optional) Use Custom Data File
```bash
dotnet run --project src\ContactManagement\ContactManagement.csproj -- --data "mydata\contacts.json"
```

---

## Technology Stack

| Technology | Version | Purpose |
| :--- | :--- | :--- |
| **C#** | 12 | Modern language features (nullable reference types, pattern matching, records) |
| **.NET** | 8.0 (LTS) | Long-term support runtime with security updates |
| **System.Text.Json** | Built-in | High-performance JSON serialization/deserialization |
| **xUnit** | 2.4.2 | Modern unit testing framework |
| **Microsoft.NET.Test.Sdk** | 17.6.0 | Test platform infrastructure |

**No External Dependencies**: The main application uses zero NuGet packages—only the .NET 8 shared framework libraries.

---

## Architecture & Design Patterns

### Layered Architecture

The application follows a clean, layered architecture with clear separation of concerns:

1. **Models Layer** (`Contact.cs`): Pure data structure with no behavior.
2. **Validation Layer** (`ContactValidator.cs`): Stateless validation and normalization logic.
3. **Persistence Layer** (`IContactRepository`, `JsonContactRepository.cs`): Abstracts storage details behind an interface.
4. **Services Layer** (`ContactService.cs`): Contains all business logic, duplicate rules, and CRUD operations.
5. **UI Layer** (`ConsoleMenu.cs`, `ConsoleInputHelper.cs`): Thin presentation layer with zero business logic.

### Key Design Decisions

**Why `IContactRepository` Interface?**
- Enables unit testing `ContactService` without touching the real filesystem.
- Allows swapping storage implementations (e.g., SQLite) without changing business logic.
- Justified purely by testability—no over-engineering.

**Why `ServiceResult<T>` Instead of Exceptions?**
- Expected failures (invalid input, duplicates) are routine business outcomes, not exceptional conditions.
- Using exceptions for routine validation is considered poor .NET practice.
- `PersistenceException` is reserved for genuine I/O failures (malformed JSON, disk errors).

**Why `int` IDs Instead of GUIDs?**
- Console users must type IDs manually—`3` is far more usable than `3fa85f64-5717-4562-b3fc-2c963f66afa6`.
- Single-user local app has no distributed-ID collision risk.

**Why JSON Instead of SQLite?**
- Zero setup complexity—just a text file.
- Human-readable and Git-diffable.
- Perfectly sufficient for tens to low thousands of contacts.
- Teaches serialization/deserialization directly.

---

## Engineering Notes

This project demonstrates several core software engineering principles:

- **Separation of Concerns**: Each layer has a single, well-defined responsibility. UI never touches validation or persistence directly.
- **Dependency Inversion**: `ContactService` depends on the `IContactRepository` abstraction, not the concrete `JsonContactRepository` implementation.
- **Defensive Programming**: Explicit guards prevent data loss (malformed JSON protection, delete confirmations, no-overwrite guarantees).
- **Normalization vs. Validation**: Validation asks "is this acceptable?" while normalization asks "how should equivalent values compare?" Storage preserves user formatting; comparison uses normalized forms.
- **Modern C# Features**: Utilizes nullable reference types, pattern matching (`switch` expressions), LINQ (`Where`, `FirstOrDefault`, `Max`), and file-scoped namespaces.
- **Testability**: Business logic is fully testable without UI or filesystem dependencies via `InMemoryContactRepository`.

---

## Edge Cases Handled

| Edge Case | Behavior |
| :--- | :--- |
| **No contacts exist** | List/Search print "No contacts found." — never crash. |
| **Invalid menu option** | "Invalid option." — loop continues. |
| **Non-numeric ID entered** | Re-prompts until valid int or empty line — never throws `FormatException`. |
| **Edit/Delete nonexistent ID** | "Contact not found (Id X)." |
| **Cancel delete** | Explicit `n` (or anything but `y`/`yes`) → "Deletion cancelled." |
| **Edit without changing anything** | Empty input keeps current value; duplicate check excludes self. |
| **Malformed JSON file** | Fatal error message, app exits, file left byte-for-byte untouched. |
| **Missing data directory** | Created automatically on first save. |
| **Search with no matches** | Returns empty list with "No contacts found." message. |
| **Duplicate phone with different formatting** | Correctly detected as duplicate (e.g., `0100-123-4567` vs `0100 123 4567`). |

---

## Internship Context

This project was developed as part of the:  
**ArithMatrix Virtual Internship Program (AVIP) 2026**  
*Software Development Track*  
**Task 03 — Contact Management System**

It fulfills all official AVIP requirements:
- ✅ CRUD operations (Add, View, List, Search, Edit, Delete)
- ✅ Contact fields (Full Name, Phone, Email)
- ✅ Persistent storage (JSON file)
- ✅ Search/filter by Name, Phone, Email
- ✅ Clear validation rules
- ✅ Duplicate handling (phone and email)
- ✅ GitHub repository with professional README
- ✅ Sample data (fictional contacts only)
- ✅ Automated testing (49/49 tests passing)
- ✅ Screenshots demonstrating CRUD operations

---

## Project Status

- [x] Complete CRUD implementation
- [x] JSON persistence with safe writes
- [x] Comprehensive input validation
- [x] Duplicate detection (phone + email)
- [x] Multi-field search with normalization
- [x] Edit with self-exclusion from duplicates
- [x] Delete confirmation safety
- [x] Malformed JSON protection
- [x] Automated test suite (49/49 passed)
- [x] Comprehensive documentation (README + GUIDE)
- [x] Sample data & screenshots
- [x] Professional Git workflow with `.gitignore`

---

## Future Improvements

*Note: These are potential enhancements, not currently implemented features.*

- **SQLite Persistence**: Migrate to a real embedded database for larger datasets (would require implementing `SqliteContactRepository : IContactRepository` — zero changes to `ContactService`).
- **ASP.NET Core Web API**: Expose CRUD operations via RESTful endpoints.
- **Web UI**: Build a Blazor or React frontend for browser-based access.
- **CSV Import/Export**: Bulk import contacts from CSV files or export for backup.
- **Advanced Search**: Fuzzy matching, phonetic search, or full-text search capabilities.
- **Contact Groups/Tags**: Categorize contacts into custom groups (Family, Work, Friends).
- **Audit Logging**: Track creation/modification timestamps and change history.
- **CI/CD Pipeline**: GitHub Actions for automated testing on every push.

---

## Author

**Abdalla Mahmoud Samir**  
GitHub: [@abdallasamir04](https://github.com/abdallasamir04)

---

## References

- **Official Documentation**: [Microsoft .NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- **System.Text.Json**: [JSON Serialization in .NET](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/)
- **xUnit Testing**: [xUnit.net Documentation](https://xunit.net/)
- **AVIP Task Requirements**: See `GUIDE.md` for comprehensive technical details and requirement traceability.

---

<p align="center">
  <sub>Built with C# 12 and .NET 8 as part of the ArithMatrix Virtual Internship Program 2026</sub>
  <br />
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:00BFFF,100:1E90FF&height=80&section=footer" />
</p>
