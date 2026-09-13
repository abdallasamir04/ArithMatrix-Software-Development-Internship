<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:0F2027,100:00C6FF&height=220&section=header&text=ArithMatrix%20AVIP%202026&fontSize=46&fontColor=FFFFFF&animation=fadeIn&fontAlignY=35&desc=Software%20Development%20Track%20%E2%80%94%203-Project%20Portfolio&descAlignY=55&descSize=18" />
</p>

<p align="center">
  <img src="https://readme-typing-svg.demolab.com?font=Fira+Code&pause=1200&color=00C6FF&center=true&vCenter=true&width=700&lines=Hi%2C+I'm+Abdalla+Samir;Software+Engineer+%7C+Backend+%26+.NET+Developer;ArithMatrix+Virtual+Internship+Program+2026;Task+01+%7C+Task+03+%7C+Task+04" />
</p>

<div align="center">

[![Status](https://img.shields.io/badge/All%20Tasks-Complete-success?style=flat-square)]()
[![Language](https://img.shields.io/badge/Language-C%23%2012-239120?style=flat-square&logo=csharp&logoColor=white)]()
[![Framework](https://img.shields.io/badge/.NET-8%20%2F%209-512BD4?style=flat-square&logo=dotnet&logoColor=white)]()
[![Tests](https://img.shields.io/badge/Combined%20Tests-103%2F103%20Passed-brightgreen?style=flat-square&logo=xunit)]()
[![Internship](https://img.shields.io/badge/AVIP-2026-blueviolet?style=flat-square)]()

**ArithMatrix Virtual Internship Program 2026 — Software Development Track**

This repository hub brings together **three independent console applications**, each built as a standalone deliverable for the ArithMatrix Virtual Internship Program (AVIP) 2026 — Software Development Track. Every project follows the same engineering philosophy: **layered/clean architecture, defensive programming, zero external dependencies beyond the .NET base class library, and full automated test coverage with xUnit.**


</div>

<br/>

##  About Me : 

<div align="center">
  <b>Abdalla Mahmoud Samir</b>   Software Engineer (B.Sc., Faculty of Computer Science &amp; Artificial Intelligence, Assiut National University)<br/>
  Passionate about <b>backend engineering, clean architecture, and enterprise-grade .NET systems.</b>
</div>

<table align="center" style="border: none;">
  <tr>
    <td align="center" style="border: none;">
      <a href="https://www.linkedin.com/in/abdalla-samir-9264242b6">
        <img src="https://img.icons8.com/fluency/48/linkedin-circled.png" width="40" alt="LinkedIn" />
      </a>
    </td>
    <td align="center" style="border: none;">
      <a href="mailto:samirovic707@gmail.com">
        <img src="https://img.icons8.com/fluency/48/gmail-new.png" width="40" alt="Email" />
      </a>
    </td>
    <td align="center" style="border: none;">
      <a href="https://github.com/abdallasamir04">
        <img src="https://img.icons8.com/fluency/48/github.png" width="40" alt="GitHub" />
      </a>
    </td>
  </tr>
</table>

<br/>


---

## 📑 Table of Contents

| # | Project | Focus | Tech | Tests |
| :---: | :--- | :--- | :--- | :---: |
| 01 | [📁 File Organizer](#-task-01--file-organizer) | Automated file classification & conflict-safe sorting | C# · .NET 9 | 34/34 ✅ |
| 03 | [📇 Contact Management System](#-task-03--contact-management-system) | CRUD console app with JSON persistence | C# · .NET 8 | 49/49 ✅ |
| 04 | [🎯 Number Guessing Game](#-task-04--number-guessing-game) | Interactive CLI game with layered architecture | C# · .NET 8 | 20/20 ✅ |


---

# 📁 Task 01 — File Organizer

<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:0F2027,100:00C6FF&height=180&section=header&text=File%20Organizer&fontSize=40&fontColor=FFFFFF&animation=fadeIn" />
</p>

<div align="center">

[![Status](https://img.shields.io/badge/Status-Complete-success?style=flat-square)]()
[![Language](https://img.shields.io/badge/Language-C%23-239120?style=flat-square&logo=csharp&logoColor=white)]()
[![Framework](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)]()
[![Tests](https://img.shields.io/badge/Tests-34/34%20Passed-brightgreen?style=flat-square&logo=xunit)]()
[![Internship](https://img.shields.io/badge/AVIP-2026-blueviolet?style=flat-square)]()

</div>

### About The Project

The **File Organizer** is a robust, command-line interface (CLI) application built with C# and .NET 9. It automates the tedious process of sorting mixed files from a source directory into logically categorized subfolders within a target directory.

Designed with clean architecture and separation of concerns, this tool evaluates file extensions, safely handles edge cases (like missing extensions or naming conflicts), and provides transparent logging—all without overwriting existing user data.

### The Problem

- **Cluttered Workspaces**: Download and working directories quickly become disorganized mixes of images, documents, archives, and media.
- **Manual Sorting is Error-Prone**: Manually moving hundreds of files is repetitive and risks accidental deletion or overwriting.
- **Naming Collisions**: Moving files with identical names can result in data loss if not handled deterministically.
- **Edge Cases**: Files without extensions or with unknown formats are often ignored or mishandled by basic scripts.

**The Solution**: A deterministic, automated CLI tool that classifies, resolves conflicts safely, and organizes files with a single command, complete with a `--dry-run` mode for risk-free previewing.

### Key Features

- **Automatic Classification**: Sorts files into `Images`, `Documents`, `Archives`, `Audio`, `Video`, and `Others` based on extension.
- **Configurable Paths**: Accepts dynamic `--source` and `--target` directories via CLI arguments or a configuration file.
- **Zero Data Loss**: **Never** overwrites existing files. Employs a deterministic conflict resolution strategy (`file_1.ext`, `file_2.ext`).
- **Dry-Run Mode**: Previews all planned operations without making any changes to the filesystem.
- **Edge Case Handling**: Safely processes files with multiple dots (e.g., `archive.tar.gz`), unknown extensions, and files with no extension at all.
- **Comprehensive Logging**: Provides clear, structured console output and optional file logging for audit trails.
- **Automated Testing**: Backed by a suite of 34 xUnit tests covering classification, conflicts, dry-run behavior, and error handling.

### Supported File Categories

| Category | Supported Extensions |
| :--- | :--- |
| **Images** | `.jpg`, `.jpeg`, `.png`, `.gif`, `.bmp`, `.webp`, `.svg`, `.tiff`, `.heic` |
| **Documents** | `.pdf`, `.doc`, `.docx`, `.txt`, `.rtf`, `.xls`, `.xlsx`, `.csv`, `.ppt`, `.pptx`, `.odt`, `.md` |
| **Archives** | `.zip`, `.rar`, `.7z`, `.tar`, `.gz`, `.bz2`, `.xz` |
| **Audio** | `.mp3`, `.wav`, `.flac`, `.aac`, `.ogg`, `.m4a`, `.wma` |
| **Video** | `.mp4`, `.mkv`, `.avi`, `.mov`, `.wmv`, `.webm`, `.flv`, `.m4v` |
| **Others** | Unknown extensions or files with **no extension** (e.g., `README`) |

### How It Works

1. **Parse Arguments**: Reads CLI flags (`--source`, `--target`, `--dry-run`, etc.) or loads a config file.
2. **Validate Paths**: Ensures the source directory exists and that the target is not the same as, or nested inside, the source.
3. **Scan Files**: Enumerates files in the source directory (ignoring subdirectories unless `--recursive` is specified).
4. **Extract & Classify**: Identifies the file extension and maps it to a category using a case-insensitive dictionary lookup.
5. **Prepare Destination**: Creates the target category folder if it does not already exist (skipped in Dry-Run).
6. **Resolve Conflicts**: Checks if the destination filename exists. If so, appends `_1`, `_2`, etc., before the final extension.
7. **Execute Move**: Safely moves the file using `File.Move` with `overwrite: false` as a final safeguard.
8. **Log & Summarize**: Records each operation and prints a final summary of scanned, organized, and errored files.

### CLI Usage

**Standard Organization**
```bash
dotnet run --project src\FileOrganizer -- --source "C:\Users\Me\Downloads" --target "C:\Users\Me\Organized"
```

**Dry-Run Mode (Preview Only)**
```bash
dotnet run --project src\FileOrganizer -- --source "C:\Users\Me\Downloads" --target "C:\Users\Me\Organized" --dry-run
```

**Using a Configuration File**
```bash
dotnet run --project src\FileOrganizer -- --config "config.txt"
```
*(Config file format: `key=value` per line, e.g., `source=C:\Downloads`)*

**Additional Options**
- `--recursive`: Include files in subdirectories of the source.
- `--log <path>`: Save the operation log to a specified text file.

### Before / After Example

**Before (`TestSource/`)**
```text
TestSource/
├── photo.jpg
├── document.pdf
├── song.mp3
├── video.mp4
├── archive.zip
├── unknown.xyz
└── README
```

**After (`TestTarget/`)**
```text
TestTarget/
├── Images/
│   └── photo.jpg
├── Documents/
│   └── document.pdf
├── Audio/
│   └── song.mp3
├── Video/
│   └── video.mp4
├── Archives/
│   └── archive.zip
└── Others/
    ├── README
    └── unknown.xyz
```

### Conflict Resolution

The application guarantees **no data loss** through deterministic renaming. If a file with the same name exists in the destination, the incoming file is renamed by appending an incrementing counter *before* the final extension.

- `report.pdf` → `report_1.pdf`
- `report_1.pdf` (if it also exists) → `report_2.pdf`
- `project.final.report.pdf` → `project.final.report_1.pdf` *(Correctly preserves the base name, not `project_1.final.report.pdf`)*

### Dry-Run Mode

The `--dry-run` flag allows you to validate the organization logic without modifying the filesystem. No directories are created, and no files are moved.

**Sample Output:**
```text
==================================================
 FILE ORGANIZER
==================================================
 Mode: DRY RUN
 Source: C:\TestSource
 Target: C:\TestTarget

 [DRY-RUN] photo.jpg -> Images\photo.jpg
 [DRY-RUN] document.pdf -> Documents\document.pdf
 [DRY-RUN] unknown.xyz -> Others\unknown.xyz

 Summary:
 Files scanned: 3
 Files organized: 3
 Errors: 0
==================================================
```

### Project Structure

```text
Task_01_File_Organizer/
├── src/
│   └── FileOrganizer/          # Main application source code
│       ├── Models/             # Data records (Options, Results, Categories)
│       ├── Services/           # Core logic (Classifier, ConflictResolver, Organizer)
│       ├── Configuration/      # CLI argument and config file parsing
│       ├── Logging/            # Console and file logging utilities
│       └── Program.cs          # Application entry point
├── tests/
│   └── FileOrganizer.Tests/    # xUnit automated test suite
├── examples/
│   └── sample_run.log          # Sample execution log
├── screenshots/                # Visual proof of execution
├── FileOrganizer.sln           # Visual Studio Solution file
├── README.md                   # Full task documentation
├── GUIDE.md                    # Comprehensive technical guide
└── .gitignore                  # Git ignore rules for .NET
```

### Testing & Quality Assurance

The project includes a comprehensive automated test suite built with **xUnit**, utilizing temporary, isolated directories to ensure no impact on the host filesystem.

- **Total Tests**: 34
- **Passed**: 34
- **Failed**: 0
- **Skipped**: 0

**Test Coverage Includes:**
- Extension classification (case-insensitive, multiple dots, no extension).
- Deterministic conflict resolution sequences.
- Dry-run mode verification (ensuring zero filesystem mutations).
- Invalid path handling and source/target overlap prevention.

Run tests locally:
```bash
dotnet test
```

### Sample Run Output

```text
==================================================
 FILE ORGANIZER
==================================================
 Mode: NORMAL
 Source: E:\SD_AVIP_2026_byte\Task_01_File_Organizer\TestSource
 Target: E:\SD_AVIP_2026_byte\Task_01_File_Organizer\TestTarget

 [MOVED]   photo.jpg -> Images\photo.jpg
 [MOVED]   document.pdf -> Documents\document.pdf
 [MOVED]   song.mp3 -> Audio\song.mp3
 [MOVED]   video.mp4 -> Video\video.mp4
 [MOVED]   archive.zip -> Archives\archive.zip
 [MOVED]   README -> Others\README
 [MOVED]   unknown.xyz -> Others\unknown.xyz

 Summary:
 Files scanned: 7
 Files organized: 7
 Errors: 0
==================================================
```

### Screenshots

| Before Organization | After Organization |
| :---: | :---: |
| ![Before](Task_01_File_Organizer/screenshots/before.png) | ![After](Task_01_File_Organizer/screenshots/after.png) |
| **Successful Execution** | **Dry-Run Preview** |
| ![Run](Task_01_File_Organizer/screenshots/run.png) | ![Dry Run](Task_01_File_Organizer/screenshots/dry-run.png) |

### Installation & Setup

**Prerequisites**
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git

```bash
# 1. Clone the Repository
git clone https://github.com/abdallasamir04/SD_AVIP_2026_byte.git

# 2. Navigate to the Task Directory
cd "SD_AVIP_2026_byte/Task_01_File_Organizer"

# 3. Restore Dependencies & Build
dotnet restore
dotnet build

# 4. Run Tests
dotnet test

# 5. Execute the Application
dotnet run --project src\FileOrganizer -- --source "<Your_Source_Path>" --target "<Your_Target_Path>"
```

### Engineering Notes

- **Separation of Concerns**: Classification logic (`FileClassifier`) and conflict resolution (`ConflictResolver`) are decoupled from filesystem execution (`FileOrganizerService`), enabling pure, fast unit testing.
- **Defensive Programming**: Explicit guards prevent catastrophic errors (e.g., rejecting `Source == Target` to prevent infinite recursive processing).
- **Deterministic Algorithms**: Conflict resolution relies on predictable, sequential naming rather than random GUIDs or timestamps, ensuring reproducible results.
- **Modern C# Features**: Utilizes `record` types for immutable data transfer, pattern matching (`is ... or ...`), and file-scoped namespaces for clean, readable code.

### Internship Context

**ArithMatrix Virtual Internship Program (AVIP) 2026** · *Software Development Track* · **Task 01 — File Organizer**

Fulfills all official internship requirements, including CLI configuration, dry-run capabilities, deterministic conflict resolution, automated testing, and comprehensive documentation.

### Project Status

- [x] File classification by extension
- [x] Category-based folder creation
- [x] Configurable source/target directories
- [x] Deterministic conflict resolution (no overwrites)
- [x] Dry-run mode implementation
- [x] Unknown and extensionless file handling
- [x] Automated testing (34/34 passed)
- [x] Comprehensive documentation & README
- [x] Sample run log & screenshots

### Future Improvements

*Note: These are potential enhancements, not currently implemented features.*
- Integration of GitHub Actions for CI/CD and automated test runs on push.
- Custom support, user-defined classification rules via JSON configuration.
- Richer CLI UX with progress bars for directories containing thousands of files.
- Structured logging (e.g., JSON output) for easier integration with log aggregation tools.

<p align="right"><a href="#-table-of-contents">⬆ Back to Table of Contents</a></p>

---

# 📇 Task 03 — Contact Management System

<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:0F2027,100:00C6FF&height=180&section=header&text=Contact%20Management%20System&fontSize=36&fontColor=FFFFFF&animation=fadeIn" />
</p>

<div align="center">

[![Status](https://img.shields.io/badge/Status-Complete-success?style=flat-square)]()
[![Language](https://img.shields.io/badge/Language-C%23-239120?style=flat-square&logo=csharp&logoColor=white)]()
[![Framework](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)]()
[![Tests](https://img.shields.io/badge/Tests-49/49%20Passed-brightgreen?style=flat-square&logo=xunit)]()
[![Internship](https://img.shields.io/badge/AVIP-2026-blueviolet?style=flat-square)]()

</div>

### About The Project

The **Contact Management System** is a robust, command-line interface (CLI) application built with C# and .NET 8. It provides a complete CRUD (Create, Read, Update, Delete) solution for managing personal contacts with persistent JSON storage, comprehensive validation, and intelligent duplicate detection.

Designed with clean architecture and separation of concerns, this application features a layered design pattern with distinct Models, Validation, Persistence, Services, and UI layers—ensuring maintainability, testability, and professional software engineering practices.

### The Problem

- **Manual Contact Management**: Keeping track of contacts in scattered notes, emails, or paper lists is inefficient and error-prone.
- **Data Loss Risk**: Without persistent storage, contacts are lost when applications close or crash.
- **Duplicate Entries**: Accidentally adding the same contact multiple times creates confusion and data inconsistency.
- **No Validation**: Accepting invalid emails, phone numbers, or empty names corrupts data quality.
- **Lack of Search**: Finding specific contacts in a large list becomes tedious without filtering capabilities.

**The Solution**: A deterministic, user-friendly CLI application that manages contacts with full CRUD operations, automatic JSON persistence, input validation, duplicate detection (by phone or email), and multi-field search capabilities—all backed by 49 automated tests.

### Key Features

- **Complete CRUD Operations**: Add, view, list, search, edit, and delete contacts with clear confirmation messages.
- **Persistent JSON Storage**: All contacts are automatically saved to `data/contacts.json` and survive application restarts.
- **Comprehensive Validation**: Validates names (non-empty, ≤100 chars), phone numbers (7-15 digits, flexible formatting), and emails (practical pattern matching).
- **Intelligent Duplicate Detection**: Prevents adding contacts with duplicate phone numbers or emails (case-insensitive, formatting-insensitive).
- **Multi-Field Search**: Search by name, phone, email, or all fields simultaneously with case-insensitive partial matching.
- **Safe Edit Operations**: Editing a contact excludes itself from duplicate checks and allows keeping existing values via empty input.
- **Confirmation-Based Deletion**: Requires explicit `y/n` confirmation before deleting contacts to prevent accidental data loss.
- **Malformed JSON Protection**: Treats corrupted JSON files as fatal errors and refuses to overwrite them, protecting user data.
- **Automated Testing**: Backed by a comprehensive suite of 49 xUnit tests covering validation, business logic, persistence, and edge cases.

### Contact Fields & Validation

| Field | Validation Rules | Valid Examples | Invalid Examples |
| :--- | :--- | :--- | :--- |
| **Full Name** | Required, non-whitespace, ≤100 chars, any script/characters | `John Doe`, `أحمد محمد`, `Mary-Jane O'Brien` | `""`, `"   "` |
| **Phone** | Required, 7-15 digits, allows spaces/hyphens/leading `+` | `01001234567`, `+201001234567`, `0100 123 4567` | `"abc"`, `"123"` |
| **Email** | Required, ≤254 chars, practical `x@y.z` pattern | `john@example.com`, `user.name@domain.org` | `"notanemail"`, `"test@.com"` |

### How It Works

```
User Input → ConsoleMenu → ContactService → ContactValidator
                                    ↓
                            IContactRepository
                                    ↓
                         JsonContactRepository
                                    ↓
                            contacts.json (JSON file)
```

**Runtime Process**

1. **Startup**: `Program.cs` resolves the data file path (`data/contacts.json` or `--data <path>` override).
2. **Load**: `JsonContactRepository` loads existing contacts from JSON (or returns empty list if file missing/empty).
3. **Menu Loop**: `ConsoleMenu` displays the main menu and waits for user input.
4. **Operation Dispatch**: Based on user choice, dispatches to Add/View/List/Search/Edit/Delete operations.
5. **Validation**: `ContactValidator` checks input validity and normalizes values for comparison.
6. **Business Logic**: `ContactService` applies duplicate rules, generates IDs, and mutates the in-memory list.
7. **Persistence**: After every successful mutation, `SaveAll()` writes the complete list back to JSON using a temp-file-then-replace strategy.
8. **Confirmation**: Clear success/failure messages are displayed, then the menu loops back.
9. **Exit**: User selects "0. Exit" → application terminates gracefully.

### CLI Usage

**Standard Run**
```bash
dotnet run --project src\ContactManagement\ContactManagement.csproj
```

**Custom Data File Path**
```bash
dotnet run --project src\ContactManagement\ContactManagement.csproj -- --data "custom\path\contacts.json"
```

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

### Duplicate Handling

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

### JSON Storage Format

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

### Project Structure

```
Task_03_Contact_Management/
├── src/
│   └── ContactManagement/
│       ├── Models/
│       │   └── Contact.cs                    # Contact data model
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
│   └── ContactManagement.Tests/
│       ├── ContactValidatorTests.cs          # Validation rule tests
│       ├── ContactServiceTests.cs            # Business logic tests
│       ├── JsonContactRepositoryTests.cs     # Persistence tests
│       ├── TestDoubles/
│       │   └── InMemoryContactRepository.cs  # In-memory test double
│       └── ContactManagement.Tests.csproj
├── data/
│   └── contacts.json                         # Runtime data (not committed)
├── examples/
│   └── sample_contacts.json                  # Fictional sample data
├── screenshots/                              # Visual proof of execution
├── ContactManagement.sln                     # Visual Studio Solution
├── README.md                                 # Full task documentation
├── GUIDE.md                                  # Comprehensive technical guide
└── .gitignore                                # Git ignore rules
```

### Testing & Quality Assurance

The project includes a comprehensive automated test suite built with **xUnit**, utilizing temporary directories and in-memory test doubles to ensure isolation and reproducibility.

- **Total Tests**: 49
- **Passed**: 49
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

### Screenshots

| Main Menu | First Run (Empty) |
| :---: | :---: |
| ![Main Menu](Task_03_Contact_Management/screenshots/first%20run%20without%20any%20thing.png) | ![Empty State](Task_03_Contact_Management/screenshots/first%20run%20without%20any%20thing.png) |

| List Contacts |
| :---: |
| ![List Contacts](Task_03_Contact_Management/screenshots/show%20contacts.png) |

| Running Tests |
| :---: |
| ![Tests](Task_03_Contact_Management/screenshots/running%20tests.png) |

### Installation & Setup

**Prerequisites**
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS version)
- Git

```bash
# 1. Clone the Repository
git clone https://github.com/abdallasamir04/SD_AVIP_2026_byte.git

# 2. Navigate to Task 03 Directory
cd "SD_AVIP_2026_byte/Task_03_Contact_Management"

# 3. Restore Dependencies & Build
dotnet restore
dotnet build

# 4. Run Tests
dotnet test

# 5. Execute the Application
dotnet run --project src\ContactManagement\ContactManagement.csproj

# 6. (Optional) Use Custom Data File
dotnet run --project src\ContactManagement\ContactManagement.csproj -- --data "mydata\contacts.json"
```

### Technology Stack

| Technology | Version | Purpose |
| :--- | :--- | :--- |
| **C#** | 12 | Modern language features (nullable reference types, pattern matching, records) |
| **.NET** | 8.0 (LTS) | Long-term support runtime with security updates |
| **System.Text.Json** | Built-in | High-performance JSON serialization/deserialization |
| **xUnit** | 2.4.2 | Modern unit testing framework |
| **Microsoft.NET.Test.Sdk** | 17.6.0 | Test platform infrastructure |

**No External Dependencies**: The main application uses zero NuGet packages—only the .NET 8 shared framework libraries.

### Architecture & Design Patterns

**Layered Architecture**

1. **Models Layer** (`Contact.cs`): Pure data structure with no behavior.
2. **Validation Layer** (`ContactValidator.cs`): Stateless validation and normalization logic.
3. **Persistence Layer** (`IContactRepository`, `JsonContactRepository.cs`): Abstracts storage details behind an interface.
4. **Services Layer** (`ContactService.cs`): Contains all business logic, duplicate rules, and CRUD operations.
5. **UI Layer** (`ConsoleMenu.cs`, `ConsoleInputHelper.cs`): Thin presentation layer with zero business logic.

**Key Design Decisions**

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

### Engineering Notes

- **Separation of Concerns**: Each layer has a single, well-defined responsibility. UI never touches validation or persistence directly.
- **Dependency Inversion**: `ContactService` depends on the `IContactRepository` abstraction, not the concrete `JsonContactRepository` implementation.
- **Defensive Programming**: Explicit guards prevent data loss (malformed JSON protection, delete confirmations, no-overwrite guarantees).
- **Normalization vs. Validation**: Validation asks "is this acceptable?" while normalization asks "how should equivalent values compare?" Storage preserves user formatting; comparison uses normalized forms.
- **Modern C# Features**: Utilizes nullable reference types, pattern matching (`switch` expressions), LINQ (`Where`, `FirstOrDefault`, `Max`), and file-scoped namespaces.
- **Testability**: Business logic is fully testable without UI or filesystem dependencies via `InMemoryContactRepository`.

### Edge Cases Handled

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

### Internship Context

**ArithMatrix Virtual Internship Program (AVIP) 2026** · *Software Development Track* · **Task 03 — Contact Management System**

Fulfills all official AVIP requirements:
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

### Project Status

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

### Future Improvements

*Note: These are potential enhancements, not currently implemented features.*

- **SQLite Persistence**: Migrate to a real embedded database for larger datasets (would require implementing `SqliteContactRepository : IContactRepository` — zero changes to `ContactService`).
- **ASP.NET Core Web API**: Expose CRUD operations via RESTful endpoints.
- **Web UI**: Build a Blazor or React frontend for browser-based access.
- **CSV Import/Export**: Bulk import contacts from CSV files or export for backup.
- **Advanced Search**: Fuzzy matching, phonetic search, or full-text search capabilities.
- **Contact Groups/Tags**: Categorize contacts into custom groups (Family, Work, Friends).
- **Audit Logging**: Track creation/modification timestamps and change history.
- **CI/CD Pipeline**: GitHub Actions for automated testing on every push.

### References

- **Official Documentation**: [Microsoft .NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- **System.Text.Json**: [JSON Serialization in .NET](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/)
- **xUnit Testing**: [xUnit.net Documentation](https://xunit.net/)
- **AVIP Task Requirements**: See `GUIDE.md` for comprehensive technical details and requirement traceability.

<p align="right"><a href="#-table-of-contents">⬆ Back to Table of Contents</a></p>

---

# 🎯 Task 04 — Number Guessing Game

<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:0F2027,100:00C6FF&height=180&section=header&text=Number%20Guessing%20Game&fontSize=40&fontColor=FFFFFF&animation=fadeIn" />
</p>

<div align="center">

[![Status](https://img.shields.io/badge/Status-Complete-success?style=flat-square)]()
[![Language](https://img.shields.io/badge/Language-C%23-239120?style=flat-square&logo=csharp&logoColor=white)]()
[![Framework](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)]()
[![Tests](https://img.shields.io/badge/Tests-20/20%20Passed-brightgreen?style=flat-square&logo=xunit)]()
[![Internship](https://img.shields.io/badge/AVIP-2026-blueviolet?style=flat-square)]()

</div>

### About The Project

The **Number Guessing Game** is an interactive, command-line interface (CLI) application built with C# and .NET 8. It challenges the user to guess a randomly generated secret number within a configurable range, providing real-time feedback ("Too High" / "Too Low") and tracking the number of attempts until the correct number is found.

Designed with clean architecture and separation of concerns, this application moves beyond basic console scripts. It features a layered design pattern with distinct Models, Services, and UI layers, alongside comprehensive input validation and a fully decoupled, testable game engine backed by 20 automated xUnit tests.

### The Problem

- **Fragile Console Apps**: Basic console games often crash when users input letters instead of numbers, or enter values that exceed integer limits.
- **Tight Coupling**: Mixing game logic, random number generation, and console I/O in a single `Program.cs` file makes the code impossible to unit test.
- **Poor User Experience**: Lack of clear instructions, unhelpful error messages, or no way to adjust difficulty makes the game frustrating.
- **Unpredictable Testing**: Testing logic that relies on `System.Random` is inherently flaky and non-deterministic.

**The Solution**: A robust, user-friendly CLI application featuring safe input parsing (`int.TryParse`), configurable difficulty levels, a decoupled game engine, and a dependency-injected `IRandomNumberProvider` interface that guarantees 100% deterministic automated testing.

### Key Features

- **Configurable Difficulty Levels**: Easy (1-50), Medium (1-100), and Hard (1-500) ranges. *[OPTIONAL ENHANCEMENT]*
- **Robust Input Validation**: Safely handles empty input, non-numeric text, and massively oversized numbers without throwing exceptions or crashing.
- **Real-Time Feedback**: Clear "Too High", "Too Low", or "Out of Range" messages after every valid guess.
- **Attempt Tracking**: Accurately counts only valid numeric guesses toward the final score.
- **Decoupled Architecture**: Game logic (`NumberGuessingEngine`) is completely separated from UI (`ConsoleMenu`) and randomness (`IRandomNumberProvider`).
- **Deterministic Testing**: Utilizes a `FixedRandomNumberProvider` test double to guarantee predictable, repeatable unit tests.
- **Interactive Main Menu**: Clean, looping menu with Start, Instructions, and Exit options.

### How It Works

```
User Input → ConsoleMenu → NumberGuessingEngine → IRandomNumberProvider
                                    ↓
                            GameResult / GuessResult
                                    ↓
                            Console Output (UI)
```

**Runtime Process**

1. **Startup**: `Program.cs` initializes the `ConsoleMenu` and `NumberGuessingEngine`.
2. **Menu Loop**: `ConsoleMenu` displays the main menu and waits for user input.
3. **Configuration**: User selects a difficulty, which instantiates a `GameSettings` object with specific Min/Max bounds.
4. **Game Initialization**: `NumberGuessingEngine` requests a secret number from the `IRandomNumberProvider` within the configured bounds.
5. **Guessing Loop**: `ConsoleInputHelper` safely reads and parses user input.
6. **Evaluation**: `NumberGuessingEngine.EvaluateGuess` compares the input to the secret number, increments the attempt counter (if valid), and returns a `GuessResult`.
7. **Feedback**: `ConsoleMenu` displays the outcome. If correct, it generates a final `GameResult` summary and returns to the main menu.
8. **Exit**: User selects "3. Exit" → application terminates gracefully.

### CLI Usage

**Standard Run**
```bash
dotnet run --project src\NumberGuessingGame\NumberGuessingGame.csproj
```

**Main Menu:**
```text
========================================
       NUMBER GUESSING GAME
========================================
1. Start Game
2. Instructions
3. Exit

Choose an option:
```

**Gameplay Example:**
```text
Select difficulty:
1. Easy   (1-50)
2. Medium (1-100)
3. Hard   (1-500)

Choice: 2

I have selected a number between 1 and 100.

Enter your guess: 50
Too low!
Enter your guess: 75
Too high!
Enter your guess: abc
Invalid input. Please enter a valid number.
Enter your guess: 62
Correct!

========================================
GAME OVER
========================================
Secret Number: 62
Attempts: 3
```

### Input Validation & Edge Cases

The application employs defensive programming to ensure a crash-free experience:

| Edge Case | Behavior |
| :--- | :--- |
| **Empty / Whitespace Input** | Rejected with "Invalid input" message; prompt repeats. Attempt count is **not** incremented. |
| **Non-Numeric Text (e.g., "abc")** | Safely rejected via `int.TryParse`; no `FormatException` is thrown. |
| **Massive Numbers (e.g., 99999999999999)** | Safely rejected via `int.TryParse` overflow protection; no crash. |
| **Out-of-Range Guess (e.g., 150 in Easy mode)** | Rejected with "Guess must be between X and Y" message. Attempt count **is** incremented (it was a valid number, just a bad guess). |
| **Invalid Menu Option** | "Invalid option." message displayed; menu loops back. |

### Project Structure

```text
Task_04_Number_Guessing_Game/
├── src/
│   └── NumberGuessingGame/
│       ├── Models/
│       │   ├── GameSettings.cs          # Configures Min/Max bounds and validates them
│       │   ├── GameResult.cs            # Final summary data (SecretNumber, Attempts)
│       │   └── GuessResult.cs           # Outcome of a single guess (TooHigh, TooLow, Correct, OutOfRange)
│       ├── Services/
│       │   ├── IRandomNumberProvider.cs # Abstraction for randomness (enables testing)
│       │   ├── RandomNumberProvider.cs  # Production implementation using System.Random
│       │   └── NumberGuessingEngine.cs  # Core business logic and state management
│       ├── UI/
│       │   ├── ConsoleMenu.cs           # Interactive menu loop and display logic
│       │   └── ConsoleInputHelper.cs    # Safe input reading and parsing utilities
│       ├── Program.cs                   # Application entry point & composition root
│       └── NumberGuessingGame.csproj
├── tests/
│   └── NumberGuessingGame.Tests/
│       ├── GameLogicTests.cs            # Tests for NumberGuessingEngine behavior
│       ├── ValidationTests.cs           # Tests for GameSettings and input boundaries
│       ├── GameResultTests.cs           # Tests for final summary generation
│       ├── FixedRandomNumberProvider.cs # Test double for deterministic testing
│       └── NumberGuessingGame.Tests.csproj
├── examples/
│   └── sample_playthrough.txt           # Text-based example of a game session
├── screenshots/                         # Visual proof of execution
├── NumberGuessingGame.sln               # Visual Studio Solution
├── README.md                            # Full task documentation
└── .gitignore                           # Git ignore rules (excludes bin/, obj/, etc.)
```

### Testing & Quality Assurance

The project includes a comprehensive automated test suite built with **xUnit**. By depending on the `IRandomNumberProvider` interface, the `NumberGuessingEngine` can be tested with a `FixedRandomNumberProvider`, ensuring 100% deterministic and repeatable tests without relying on actual randomness.

- **Total Tests**: 20
- **Passed**: 20
- **Failed**: 0
- **Skipped**: 0

**Test Coverage Includes:**

| Category | Coverage |
| :--- | :--- |
| **Game Logic** | Correct guess detection, Too High/Low evaluation, attempt counting accumulation. |
| **Validation** | `GameSettings` rejects invalid ranges (Min >= Max). |
| **Edge Cases** | Out-of-range guesses are flagged correctly; game state resets properly. |
| **Test Doubles** | `FixedRandomNumberProvider` successfully injects known values for predictable assertions. |

**Run Tests Locally:**
```bash
dotnet test
```

### Installation & Setup

**Prerequisites**
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS version)
- Git

```bash
# 1. Clone the Repository
git clone https://github.com/abdallasamir04/SD_AVIP_2026_byte.git

# 2. Navigate to Task 04 Directory
cd "SD_AVIP_2026_byte/Task_04_Number_Guessing_Game"

# 3. Restore Dependencies & Build
dotnet restore
dotnet build

# 4. Run Tests
dotnet test

# 5. Execute the Application
dotnet run --project src\NumberGuessingGame\NumberGuessingGame.csproj
```

### Technology Stack

| Technology | Version | Purpose |
| :--- | :--- | :--- |
| **C#** | 12 | Modern language features (nullable reference types, pattern matching) |
| **.NET** | 8.0 (LTS) | Long-term support runtime with security updates |
| **xUnit** | Latest | Modern, attribute-based unit testing framework |

**No External Dependencies**: The main application uses zero NuGet packages—only the built-in .NET 8 shared framework libraries.

### Architecture & Design Decisions

**Why `IRandomNumberProvider` Interface?**
This is the most critical design decision in the project. If the game engine directly called `new Random().Next()`, unit tests would be flaky and unpredictable. By abstracting this behind an interface, we can inject a `FixedRandomNumberProvider` during testing, guaranteeing the secret number is always known, making assertions like `Assert.Equal(5, result.Attempts)` 100% reliable.

**Why `GameSettings` Validates at Construction?**
By throwing an `ArgumentException` if `Minimum >= Maximum` during object creation, we guarantee that the `NumberGuessingEngine` can *never* be instantiated with an invalid state. This is "Fail Fast" defensive programming.

**Why Separate `GuessResult` and `GameResult`?**
- `GuessResult` represents the outcome of a *single action* (Too High, Too Low, Correct).
- `GameResult` represents the *final state* of the completed game (Secret Number, Total Attempts).
Separating these prevents state leakage and keeps method signatures clean and single-purpose.

### Internship Context

**ArithMatrix Virtual Internship Program (AVIP) 2026** · *Software Development Track* · **Task 04 — Number Guessing Game**

It fulfills all official AVIP requirements and demonstrates a strong understanding of:
- ✅ Separation of Concerns (UI vs. Business Logic)
- ✅ Dependency Inversion (Interface-based randomness)
- ✅ Robust Error Handling (No crashes on bad input)
- ✅ Automated Unit Testing (Deterministic test doubles)
- ✅ Professional Git workflow and documentation

<p align="right"><a href="#-table-of-contents">⬆ Back to Table of Contents</a></p>

---

## 🧰 Combined Tech Stack

<p align="center">
  <img src="https://skillicons.dev/icons?i=cs,dotnet,git,github,vscode,visualstudio&perline=8" alt="Languages, frameworks and tools" />
</p>

| Technology | Used In | Purpose |
| :--- | :--- | :--- |
| **C# 12** | All 3 tasks | Nullable reference types, pattern matching, records, file-scoped namespaces |
| **.NET 9.0** | Task 01 | Latest runtime for File Organizer |
| **.NET 8.0 (LTS)** | Task 03, Task 04 | Long-term support runtime |
| **System.Text.Json** | Task 03 | Contact persistence serialization |
| **xUnit** | All 3 tasks | Automated unit testing (103 tests combined) |
| **Dependency Inversion (custom interfaces)** | Task 03 (`IContactRepository`), Task 04 (`IRandomNumberProvider`) | Testability without mocking frameworks |

---

## 📦 Repository Structure (All Tasks)

```text
SD_AVIP_2026_byte/
├── Task_01_File_Organizer/
│   ├── src/FileOrganizer/
│   ├── tests/FileOrganizer.Tests/
│   └── README.md
├── Task_03_Contact_Management/
│   ├── src/ContactManagement/
│   ├── tests/ContactManagement.Tests/
│   └── README.md
├── Task_04_Number_Guessing_Game/
│   ├── src/NumberGuessingGame/
│   ├── tests/NumberGuessingGame.Tests/
│   └── README.md
└── README.md   # ← this hub file
```

---

## ⚙️ General Installation & Setup

**Prerequisites**
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) and/or [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git

```bash
# Clone once — all three tasks live in the same repository
git clone https://github.com/abdallasamir04/SD_AVIP_2026_byte.git
cd SD_AVIP_2026_byte

# Then jump into whichever task you need:
cd Task_01_File_Organizer   && dotnet restore && dotnet build && dotnet test
cd ../Task_03_Contact_Management && dotnet restore && dotnet build && dotnet test
cd ../Task_04_Number_Guessing_Game && dotnet restore && dotnet build && dotnet test
```

Each task folder is fully self-contained (its own `.sln`, `src/`, `tests/`, and `README.md`) and can be built, tested, and run independently of the others.

---



<p align="center">
  <sub>Built with C# 12 and .NET 8/9 as part of the ArithMatrix Virtual Internship Program 2026 — Software Development Track</sub>
  <br />
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:00BFFF,100:1E90FF&height=100&section=footer" />
</p>
