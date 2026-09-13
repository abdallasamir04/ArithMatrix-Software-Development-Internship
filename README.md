<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:0F2027,100:00C6FF&height=200&section=header&text=ArithMatrix%20AVIP%202026&fontSize=48&fontColor=FFFFFF&animation=fadeIn&desc=Software%20Development%20Track%20%E2%80%94%20Engineering%20Case%20Studies&descAlignY=55&descAlign=60" />
</p>

<div align="center">

# 🏗️ ArithMatrix Virtual Internship Program 2026
### Software Development Track — Engineering Case Studies

[![Status](https://img.shields.io/badge/Status-Complete-success?style=for-the-badge)]()
[![Framework](https://img.shields.io/badge/.NET-8%20%26%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)]()
[![Language](https://img.shields.io/badge/C%23%2012-239120?style=for-the-badge&logo=csharp&logoColor=white)]()
[![Tests](https://img.shields.io/badge/Total_Tests-103/103-brightgreen?style=for-the-badge&logo=xunit)]()
[![Internship](https://img.shields.io/badge/AVIP-2026-blueviolet?style=for-the-badge)]()

</div>

---

## 📋 Repository Overview

This repository contains the complete engineering deliverables for the **ArithMatrix Virtual Internship Program (AVIP) 2026**. Each task was treated as a standalone professional software project, built from scratch with industry-grade practices: **Clean Architecture, Test-Driven Development (TDD), Defensive Programming, and Comprehensive Documentation**.
ش
Rather than a collection of toy scripts, this repository demonstrates three distinct engineering case studies, each solving a real-world problem with deliberate architectural trade-offs.

---

## 🧭 Common Engineering Philosophy

Across all three projects, the following non-negotiable engineering principles were applied:

| Principle | Implementation |
| :--- | :--- |
| **Separation of Concerns** | Strict layering: `Models`, `Services`, `Persistence`, `Validation`, `UI`. UI never touches storage directly. |
| **Dependency Inversion** | Business logic depends on **abstractions** (`IContactRepository`, `IRandomNumberProvider`), never concrete implementations. |
| **Defensive Programming** | Explicit guards, fail-fast validation, temp-file-then-replace writes, and crash-safe operations. |
| **Deterministic Testing** | Test doubles and in-memory fakes replace filesystem and randomness for 100% reproducible tests. |
| **Modern C# (12)** | Nullable reference types, pattern matching, file-scoped namespaces, `record` types, primary constructors. |
| **No Unnecessary Dependencies** | Zero external NuGet packages in production code — only the .NET shared framework. |

---

## 📂 Project Case Studies

---

### 📁 Case Study 01 — File Organizer
#### *Automated Filesystem Classification with Zero Data Loss*

<p>
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet" />
  <img src="https://img.shields.io/badge/xUnit-34/34_Passed-brightgreen?style=flat-square" />
  <img src="https://img.shields.io/badge/Type-CLI_Tool-orange?style=flat-square" />
</p>

#### 🎯 The Engineering Challenge
Build a CLI utility that can safely organize thousands of mixed files (images, documents, archives, media) into categorized subdirectories — **without ever overwriting user data**, even when naming conflicts occur.

#### 🏛 Architecture Highlights

```
┌─────────────────────────────────────────────────────┐
│                   Program.cs (Composition Root)     │
├─────────────────────────────────────────────────────┤
│  Configuration Layer │ CLI Args + Config File Parse │
├─────────────────────────────────────────────────────┤
│  FileClassifier      │ Extension → Category (O(1))  │
│  ConflictResolver    │ Deterministic rename logic   │
│  FileOrganizerService│ Orchestrates the pipeline    │
├─────────────────────────────────────────────────────┤
│  Logging Layer       │ Console + File audit trail   │
└─────────────────────────────────────────────────────┘
```

#### 💡 Key Technical Decisions

**1. Dictionary-based Classification (O(1) Lookup)**
Instead of long `if/else` or `switch` chains, extensions are mapped via a case-insensitive `Dictionary<string, FileCategory>`. Adding a new extension requires exactly one line of code.

**2. Deterministic Conflict Resolution**
The project **never overwrites**. When `report.pdf` conflicts with an existing file, the resolver generates `report_1.pdf`, `report_2.pdf`, etc. Critically, for multi-dot filenames like `project.final.report.pdf`, it correctly produces `project.final.report_1.pdf` (preserving the base name, not corrupting it).

**3. Dry-Run Mode**
A full preview mode that simulates every operation without touching the filesystem — essential for risk-free validation before destructive moves.

**4. No DI Container**
Dependency Injection containers (like `Microsoft.Extensions.DependencyInjection`) were intentionally **omitted**. The project is small enough that direct instantiation in `Program.cs` is clearer and more maintainable. This avoids over-engineering.

#### 🧪 Testing Strategy
- **34 xUnit tests** covering extension classification, multiple-dot filenames, extensionless files (`README`), unknown formats, conflict sequences, and dry-run behavior.
- Tests use **temporary isolated directories** — zero impact on the host filesystem.

---

### 📇 Case Study 02 — Contact Management System
#### *CRUD Console Application with JSON Persistence & Duplicate Detection*

<p>
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet" />
  <img src="https://img.shields.io/badge/xUnit-49/49_Passed-brightgreen?style=flat-square" />
  <img src="https://img.shields.io/badge/Storage-JSON-yellow?style=flat-square" />
</p>

#### 🎯 The Engineering Challenge
Design a complete CRUD system with persistent storage, full UTF-8/Arabic support, intelligent duplicate detection (by phone and email, format-insensitive), and protection against data corruption — all in a console application.

#### 🏛 Layered Architecture

```
┌─────────────────────────────────────────────────────────┐
│  UI Layer        │ ConsoleMenu, ConsoleInputHelper  │
├──────────────────────────────────────────────────────┤
│  Services Layer    │ ContactService (Business Logic)│
├──────────────────────────────────────────────────────┤
│  Validation Layer  │ ContactValidator + Normalization │
├──────────────────────────────────────────────────────┤
│  Persistence Layer │ IContactRepository (interface) │
│                    │   └─ JsonContactRepository   │
│                    │   └─ InMemoryContactRepository │ (test double)
└──────────────────────────────────────────────────────┘
```

#### 💡 Key Technical Decisions

**1. `ServiceResult<T>` over Exceptions for Expected Failures**
Invalid input and duplicate entries are **routine business outcomes**, not exceptional conditions. Using exceptions for validation is considered poor .NET practice. `PersistenceException` is reserved only for genuine I/O failures.

**2. Normalization vs. Validation Distinction**
- **Validation** asks: *"Is this acceptable?"* (non-empty name, valid email pattern).
- **Normalization** asks: *"How should equivalent values compare?"* (stripping phone formatting, lowercasing emails).
- **Storage preserves user formatting**; only **comparison uses normalized forms**.

**3. Format-Insensitive Duplicate Detection**
`0100 123 4567`, `0100-123-4567`, and `01001234567` are all recognized as the **same** phone number. Case-insensitive email matching (`John@Example.com` == `john@example.com`).

**4. Edit Self-Exclusion**
When editing Contact A, its own ID is excluded from duplicate checks — prevents a contact from "colliding" with itself when only one field is updated.

**5. Malformed JSON Protection**
If `contacts.json` becomes corrupted, the app **exits fatally without overwriting** the corrupted file. This protects user data from catastrophic loss.

**6. `int` IDs over GUIDs**
Console users must **type IDs manually**. `3` is far more usable than `3fa85f64-5717-4562-b3fc-2c963f66afa6`. A single-user local app has zero distributed-ID collision risk.

**7. JSON over SQLite**
Zero setup complexity, human-readable, Git-diffable, and perfectly sufficient for tens to low thousands of contacts. Teaches serialization directly.

#### 🧪 Testing Strategy
- **49 xUnit tests** — the largest suite in the repository.
- **InMemoryContactRepository** test double enables full CRUD testing without filesystem I/O.
- Tests cover: validation rules, CRUD success/failure paths, duplicate detection across formatting variations, edit self-exclusion, malformed JSON handling, and missing files.

---

### 🎮 Case Study 03 — Number Guessing Game
#### *Interactive Console Game with Decoupled Engine & Deterministic Testing*

<p>
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet" />
  <img src="https://img.shields.io/badge/xUnit-20/20_Passed-brightgreen?style=flat-square" />
  <img src="https://img.shields.io/badge/Pattern-Layered_Architecture-purple?style=flat-square" />
</p>

#### 🎯 The Engineering Challenge
Transform a classic beginner "guess the number" game into a **testable, layered, and robust** application — proving that even simple games can be engineered professionally.

#### 🏛 Architecture Flow

```
User Input → ConsoleMenu → NumberGuessingEngine → IRandomNumberProvider
                                  ↓
                          GameResult / GuessResult
                                  ↓
                          Console Output (UI)
```

#### 💡 Key Technical Decisions

**1. `IRandomNumberProvider` — The Critical Design Decision**
If the engine directly called `new Random().Next()`, unit tests would be **flaky and non-deterministic**. By abstracting randomness behind an interface, we inject a `FixedRandomNumberProvider` during testing — guaranteeing the secret number is always known. This makes assertions like `Assert.Equal(5, result.Attempts)` **100% reliable**.

**2. `GameSettings` Fail-Fast Validation**
Throws `ArgumentException` at construction if `Minimum >= Maximum`. This guarantees the `NumberGuessingEngine` can **never** be instantiated with an invalid state. Pure defensive programming.

**3. Separation of `GuessResult` and `GameResult`**
- `GuessResult`: outcome of a **single action** (TooHigh, TooLow, Correct, OutOfRange).
- `GameResult`: **final state** of the completed game (SecretNumber, TotalAttempts).
Separating these prevents state leakage and keeps method signatures single-purpose.

**4. Robust Input Validation via `int.TryParse`**
Safely handles empty input, non-numeric text (`abc`), and massively oversized numbers (`99999999999999`) **without throwing exceptions**. The app never crashes on bad input.

**5. Attempt Counting Logic**
Only valid numeric guesses increment the attempt counter. Empty input and text are rejected without penalty. Out-of-range numeric guesses (valid numbers, bad guesses) **do** count — they were legitimate attempts.

#### 🧪 Testing Strategy
- **20 xUnit tests** covering correct guess detection, high/low evaluation, attempt accumulation, invalid range rejection, game state reset, and test double injection.

---

## 📊 Comparative Analysis

| Dimension | File Organizer | Contact Management | Number Guessing Game |
| :--- | :--- | :--- | :--- |
| **Framework** | .NET 9 | .NET 8 | .NET 8 |
| **Tests** | 34 / 34 ✅ | 49 / 49 ✅ | 20 / 20 ✅ |
| **Primary Pattern** | Pipeline / Strategy | Layered + Repository | Layered + DI |
| **Storage** | Filesystem | JSON file | In-memory |
| **Key Interface** | — | `IContactRepository` | `IRandomNumberProvider` |
| **Test Double** | Temp directories | `InMemoryContactRepository` | `FixedRandomNumberProvider` |
| **Main Challenge** | Conflict resolution | Duplicate detection | Deterministic randomness |
| **User Interaction** | CLI arguments | Interactive menu | Interactive game loop |

---

## 🚀 Quick Start

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (for Task 01)
- Git

### Clone & Build All
```bash
git clone https://github.com/abdallasamir04/ArithMatrix-Software-Development-Internship.git
cd ArithMatrix-Software-Development-Internship
```

### Run Individual Tasks
```bash
# Task 01 - File Organizer
cd "Task 01 File Organizer"
dotnet run --project src\FileOrganizer -- --source "./TestSource" --target "./TestTarget"

# Task 03 - Contact Management
cd "../Task 03 Contact Management"
dotnet run --project src\ContactManagement\ContactManagement.csproj

# Task 04 - Number Guessing Game
cd "../Task 04 - Number Guessing Game"
dotnet run --project src\NumberGuessingGame\NumberGuessingGame.csproj
```

### Run All Tests (103 total)
```bash
dotnet test
```

---

## 📚 Further Reading

Each task directory contains its own comprehensive documentation:

| Document | Purpose |
| :--- | :--- |
| `README.md` (per task) | Full feature docs, CLI usage, screenshots |
| `GUIDE.md` (per task) | Deep architectural explanations, API deep-dives, interview prep |
| `examples/` | Sample runs, fictional data, playthroughs |
| `screenshots/` | Visual proof of execution |

---

## 👨‍💻 Author

**Abdalla Mahmoud Samir**  
Software Engineer — Building scalable systems with clean architecture.

<p>
  <a href="https://www.linkedin.com/in/abdalla-samir-9264242b6">
    <img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white" />
  </a>
  <a href="https://github.com/abdallasamir04">
    <img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white" />
  </a>
  <a href="mailto:samirovic707@gmail.com">
    <img src="https://img.shields.io/badge/Email-D14836?style=for-the-badge&logo=gmail&logoColor=white" />
  </a>
</p>

---

<p align="center">
  <sub>Built with C# 12, .NET 8/9, and a commitment to engineering excellence.</sub>
  <br />
  <sub>ArithMatrix Virtual Internship Program 2026 — Software Development Track</sub>
  <br />
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:00BFFF,100:1E90FF&height=80&section=footer" />
</p>
```
