<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:0F2027,100:00C6FF&height=200&section=header&text=Number%20Guessing%20Game&fontSize=48&fontColor=FFFFFF&animation=fadeIn&fontAlignY=" />
</p>

<p align="center">
  <img src="https://readme-typing-svg.demolab.com?font=Fira+Code&pause=1200&color=00C6FF&center=true&vCenter=true&width=600&lines=Task+04+%7C+ArithMatrix+AVIP+2026;Interactive+Console+Game+with+Layered+Architecture" />
</p>

<div align="center">

[![Status](https://img.shields.io/badge/Status-Complete-success?style=flat-square)]()
[![Language](https://img.shields.io/badge/Language-C%23-239120?style=flat-square&logo=csharp&logoColor=white)]()
[![Framework](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)]()
[![Tests](https://img.shields.io/badge/Tests-20/20%20Passed-brightgreen?style=flat-square&logo=xunit)]()
[![Internship](https://img.shields.io/badge/AVIP-2026-blueviolet?style=flat-square)]()

**ArithMatrix Virtual Internship Program 2026 — Software Development Track**

</div>

<br />

## About The Project

The **Number Guessing Game** is an interactive, command-line interface (CLI) application built with C# and .NET 8. It challenges the user to guess a randomly generated secret number within a configurable range, providing real-time feedback ("Too High" / "Too Low") and tracking the number of attempts until the correct number is found.

Designed with clean architecture and separation of concerns, this application moves beyond basic console scripts. It features a layered design pattern with distinct Models, Services, and UI layers, alongside comprehensive input validation and a fully decoupled, testable game engine backed by 20 automated xUnit tests.

---

## The Problem

- **Fragile Console Apps**: Basic console games often crash when users input letters instead of numbers, or enter values that exceed integer limits.
- **Tight Coupling**: Mixing game logic, random number generation, and console I/O in a single `Program.cs` file makes the code impossible to unit test.
- **Poor User Experience**: Lack of clear instructions, unhelpful error messages, or no way to adjust difficulty makes the game frustrating.
- **Unpredictable Testing**: Testing logic that relies on `System.Random` is inherently flaky and non-deterministic.

**The Solution**: A robust, user-friendly CLI application featuring safe input parsing (`int.TryParse`), configurable difficulty levels, a decoupled game engine, and a dependency-injected `IRandomNumberProvider` interface that guarantees 100% deterministic automated testing.

---

## Key Features

- **Configurable Difficulty Levels**: Easy (1-50), Medium (1-100), and Hard (1-500) ranges. *[OPTIONAL ENHANCEMENT]*
- **Robust Input Validation**: Safely handles empty input, non-numeric text, and massively oversized numbers without throwing exceptions or crashing.
- **Real-Time Feedback**: Clear "Too High", "Too Low", or "Out of Range" messages after every valid guess.
- **Attempt Tracking**: Accurately counts only valid numeric guesses toward the final score.
- **Decoupled Architecture**: Game logic (`NumberGuessingEngine`) is completely separated from UI (`ConsoleMenu`) and randomness (`IRandomNumberProvider`).
- **Deterministic Testing**: Utilizes a `FixedRandomNumberProvider` test double to guarantee predictable, repeatable unit tests.
- **Interactive Main Menu**: Clean, looping menu with Start, Instructions, and Exit options.

---

## How It Works

### Architecture Flow

```
User Input → ConsoleMenu → NumberGuessingEngine → IRandomNumberProvider
                                    ↓
                            GameResult / GuessResult
                                    ↓
                            Console Output (UI)
```

### Runtime Process

1. **Startup**: `Program.cs` initializes the `ConsoleMenu` and `NumberGuessingEngine`.
2. **Menu Loop**: `ConsoleMenu` displays the main menu and waits for user input.
3. **Configuration**: User selects a difficulty, which instantiates a `GameSettings` object with specific Min/Max bounds.
4. **Game Initialization**: `NumberGuessingEngine` requests a secret number from the `IRandomNumberProvider` within the configured bounds.
5. **Guessing Loop**: 
   - `ConsoleInputHelper` safely reads and parses user input.
6. **Evaluation**: `NumberGuessingEngine.EvaluateGuess` compares the input to the secret number, increments the attempt counter (if valid), and returns a `GuessResult`.
7. **Feedback**: `ConsoleMenu` displays the outcome. If correct, it generates a final `GameResult` summary and returns to the main menu.
8. **Exit**: User selects "3. Exit" → application terminates gracefully.

---

## CLI Usage

### Standard Run
```bash
dotnet run --project src\NumberGuessingGame\NumberGuessingGame.csproj
```

### Interactive Menu Operations

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

---

## Input Validation & Edge Cases

The application employs defensive programming to ensure a crash-free experience:

| Edge Case | Behavior |
| :--- | :--- |
| **Empty / Whitespace Input** | Rejected with "Invalid input" message; prompt repeats. Attempt count is **not** incremented. |
| **Non-Numeric Text (e.g., "abc")** | Safely rejected via `int.TryParse`; no `FormatException` is thrown. |
| **Massive Numbers (e.g., 99999999999999)** | Safely rejected via `int.TryParse` overflow protection; no crash. |
| **Out-of-Range Guess (e.g., 150 in Easy mode)** | Rejected with "Guess must be between X and Y" message. Attempt count **is** incremented (it was a valid number, just a bad guess). |
| **Invalid Menu Option** | "Invalid option." message displayed; menu loops back. |

---

## Project Structure

```text
Task 04 - Number Guessing Game/
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
├── screenshots/                         # Visual proof of execution (to be added)
├── NumberGuessingGame.sln               # Visual Studio Solution
├── README.md                            # This documentation
└── .gitignore                           # Git ignore rules (excludes bin/, obj/, etc.)
```

---

## Testing & Quality Assurance

The project includes a comprehensive automated test suite built with **xUnit**. By depending on the `IRandomNumberProvider` interface, the `NumberGuessingEngine` can be tested with a `FixedRandomNumberProvider`, ensuring 100% deterministic and repeatable tests without relying on actual randomness.

- **Total Tests**: 20
- **Passed**: 20 ✅
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

---


## Installation & Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS version)
- Git

### 1. Clone the Repository
```bash
git clone https://github.com/abdallasamir04/ArithMatrix-Software-Development-Internship.git
```

### 2. Navigate to Task 04 Directory
```bash
cd "ArithMatrix-Software-Development-Internship/Task 04 - Number Guessing Game"
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
dotnet run --project src\NumberGuessingGame\NumberGuessingGame.csproj
```

---

## Technology Stack

| Technology | Version | Purpose |
| :--- | :--- | :--- |
| **C#** | 12 | Modern language features (nullable reference types, pattern matching) |
| **.NET** | 8.0 (LTS) | Long-term support runtime with security updates |
| **xUnit** | Latest | Modern, attribute-based unit testing framework |

**No External Dependencies**: The main application uses zero NuGet packages—only the built-in .NET 8 shared framework libraries.

---

## Architecture & Design Decisions

### Why `IRandomNumberProvider` Interface?
This is the most critical design decision in the project. If the game engine directly called `new Random().Next()`, unit tests would be flaky and unpredictable. By abstracting this behind an interface, we can inject a `FixedRandomNumberProvider` during testing, guaranteeing the secret number is always known, making assertions like `Assert.Equal(5, result.Attempts)` 100% reliable.

### Why `GameSettings` Validates at Construction?
By throwing an `ArgumentException` if `Minimum >= Maximum` during object creation, we guarantee that the `NumberGuessingEngine` can *never* be instantiated with an invalid state. This is "Fail Fast" defensive programming.

### Why Separate `GuessResult` and `GameResult`?
- `GuessResult` represents the outcome of a *single action* (Too High, Too Low, Correct).
- `GameResult` represents the *final state* of the completed game (Secret Number, Total Attempts). 
Separating these prevents state leakage and keeps method signatures clean and single-purpose.

---

## Internship Context

This project was developed as part of the:  
**ArithMatrix Virtual Internship Program (AVIP) 2026**  
*Software Development Track*  
**Task 04 — Number Guessing Game**

It fulfills all official AVIP requirements and demonstrates a strong understanding of:
- ✅ Separation of Concerns (UI vs. Business Logic)
- ✅ Dependency Inversion (Interface-based randomness)
- ✅ Robust Error Handling (No crashes on bad input)
- ✅ Automated Unit Testing (Deterministic test doubles)
- ✅ Professional Git workflow and documentation

---



## Author

**Abdalla Mahmoud Samir**  
GitHub: [@abdallasamir04](https://github.comabdallasamir04)

---

<p align="center">
  <sub>Built with C# 12 and .NET 8 as part of the ArithMatrix Virtual Internship Program 2026</sub>
  <br />
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:00BFFF,100:1E90FF&height=80&section=footer" />
</p>
