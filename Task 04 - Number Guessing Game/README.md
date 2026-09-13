# Number Guessing Game

## Overview

A console-based Number Guessing Game built in C# on .NET 8. The
computer picks a secret number in a configurable range, the player
guesses repeatedly, and the game reports "too high" / "too low" after
each guess until the number is found, then shows a final summary.

## AVIP Task

This project implements **AVIP 2026 — Task 04: Number Guessing Game**,
part of the Software Development track of the ArithMatrix Virtual
Internship Program. It lives alongside the previously completed
Task 01 (File Organizer) and Task 03 (Contact Management System) in
the same repository, as its own isolated project folder.

## Features

- Random secret number generated within a configurable, inclusive
  range (`Minimum` to `Maximum`).
- Difficulty levels — Easy (1-50), Medium (1-100), Hard (1-500).
  **[OPTIONAL ENHANCEMENT]** — difficulty levels are not mandatory
  per the official AVIP task, but were implemented because they add
  real value without meaningfully increasing complexity.
- "Too low" / "too high" feedback after every valid guess.
- Attempt counting.
- Final summary showing the secret number and total attempts.
- Input validation: empty input, non-numeric text, and numbers too
  large to fit in an `int` are all handled without crashing.
- Out-of-range guesses are rejected with a clear message instead of
  being silently accepted.
- Simple main menu with a Start Game / Instructions / Exit flow.
  **[OPTIONAL ENHANCEMENT]** — an instructions screen, added for
  usability, not required by AVIP.

## Requirements

| Requirement | Official/Optional | Implementation | Test | Manual Evidence | Status |
|---|---|---|---|---|---|
| Random number generation | Official | `RandomNumberProvider`, `NumberGuessingEngine` | `RandomProviderConstructor_UsesValueFromProvider` | Not independently verified | Complete |
| Configurable range | Official | `GameSettings` | `ForDifficulty_ReturnsExpectedRange`, boundary tests | Not independently verified | Complete |
| User guessing | Official | `ConsoleMenu.PlayGame`, `NumberGuessingEngine.EvaluateGuess` | `GameLogicTests` | Not independently verified | Complete |
| Too high feedback | Official | `NumberGuessingEngine.EvaluateGuess` → `GuessOutcome.TooHigh` | `GuessAboveSecretNumber_ReturnsTooHigh` | Not independently verified | Complete |
| Too low feedback | Official | `NumberGuessingEngine.EvaluateGuess` → `GuessOutcome.TooLow` | `GuessBelowSecretNumber_ReturnsTooLow` | Not independently verified | Complete |
| Attempt counting | Official | `NumberGuessingEngine._attempts` | `ValidGuess_IncrementsAttemptCount`, `MultipleValidGuesses_AccumulateAttemptCount` | Not independently verified | Complete |
| Correct number in final summary | Official | `GameResult.SecretNumber` | `GameCompletion_ProducesCorrectGameResult` | Not independently verified | Complete |
| Attempts in final summary | Official | `GameResult.Attempts` | `GameCompletion_ProducesCorrectGameResult` | Not independently verified | Complete |
| Input validation | Official | `ConsoleInputHelper.ReadInt` | Manual test plan (below) | Not independently verified | Complete |
| Graceful invalid-input handling | Official | `ConsoleInputHelper.ReadInt` (loops on `TryParse` failure) | Manual test plan (below) | Not independently verified | Complete |
| Optional difficulty levels | Optional | `Difficulty` enum, `GameSettings.ForDifficulty` | `ForDifficulty_ReturnsExpectedRange` | Not independently verified | Complete |
| README | Deliverable | This file | — | — | Complete |
| Example playthrough | Deliverable | `examples/sample_playthrough.txt`, README section below | — | — | Complete |
| Gameplay evidence | Deliverable | Transcript/demo video/GIF | — | Not yet captured | Not Implemented |
| GitHub repository | Deliverable | `SD_AVIP_2026_byte` repository | — | Not yet pushed | Not Implemented |

## Technologies

- C# 12
- .NET 8 (LTS) — Console Application
- xUnit — automated testing
- No external packages beyond the test framework; no databases,
  no Entity Framework, no web frameworks.

## Project Structure

```
Task_04_Number_Guessing_Game/
│
├── src/
│   └── NumberGuessingGame/
│       ├── Models/
│       │   ├── GameSettings.cs
│       │   ├── GameResult.cs
│       │   └── GuessResult.cs
│       ├── Services/
│       │   ├── IRandomNumberProvider.cs
│       │   ├── RandomNumberProvider.cs
│       │   └── NumberGuessingEngine.cs
│       ├── UI/
│       │   ├── ConsoleMenu.cs
│       │   └── ConsoleInputHelper.cs
│       ├── Program.cs
│       └── NumberGuessingGame.csproj
│
├── tests/
│   └── NumberGuessingGame.Tests/
│       ├── GameLogicTests.cs
│       ├── ValidationTests.cs
│       ├── GameResultTests.cs
│       ├── FixedRandomNumberProvider.cs
│       └── NumberGuessingGame.Tests.csproj
│
├── examples/
│   └── sample_playthrough.txt
│
├── screenshots/            (empty until gameplay evidence is captured)
├── README.md
├── .gitignore
└── NumberGuessingGame.sln
```

## How It Works

1. The player picks a difficulty from the main menu, which fixes the
   guessing range (`GameSettings`).
2. `NumberGuessingEngine` asks an `IRandomNumberProvider` for a secret
   number inside that range and stores it privately.
3. Each guess the player types is parsed safely by
   `ConsoleInputHelper.ReadInt`, then handed to
   `NumberGuessingEngine.EvaluateGuess`, which compares it to the
   secret number and returns one of: too low, too high, correct, or
   out of range.
4. `ConsoleMenu` turns that outcome into the right message on screen.
5. Once the guess is correct, `NumberGuessingEngine.BuildResult()`
   produces a `GameResult` with the secret number and attempt count,
   which is printed as the final summary.

## Game Flow

```
Application Start
    ↓
Configure Game (choose difficulty)
    ↓
Generate Secret Number
    ↓
Display Guess Prompt
    ↓
Read User Input
    ↓
Validate Input
    ↓
Is Input Valid?
   /       \
 NO         YES
 |           |
Show Error   Count Attempt
 |           |
Retry        Compare Guess
                ↓
        ┌───────┼────────┐
        ↓       ↓        ↓
      Too Low  Correct  Too High
        ↓       ↓        ↓
      Retry   Summary   Retry
                ↓
              Exit
```

## Configuration

The guessing range is configured through `GameSettings`, which
enforces `Minimum < Maximum` and rejects invalid ranges
(`ArgumentException`) at construction time.

## Difficulty Levels

**[OPTIONAL ENHANCEMENT]** Three difficulty levels are implemented,
each mapping to a fixed range:

| Difficulty | Range |
|---|---|
| Easy | 1–50 |
| Medium | 1–100 |
| Hard | 1–500 |

These specific numbers are an engineering choice, not part of the
official AVIP requirement, made to give a clear, easy-to-explain
difficulty curve.

## Input Validation

- Empty or whitespace-only input is rejected with a message and the
  prompt repeats.
- Non-numeric text (e.g. `abc`) is rejected using `int.TryParse`,
  which never throws, instead of `int.Parse`.
- Numbers outside the `int` range (e.g. `999999999999999999999`)
  fail `TryParse` and are handled the same as any other invalid text
  — no crash, no exception-based control flow for ordinary bad input.
- Guesses that parse correctly but fall outside the configured
  Minimum–Maximum range are rejected by the game engine with a clear
  message, and still count as an attempt (see below).

## Attempt Counting Policy

- A guess only counts as an attempt once it has been **successfully
  parsed as an integer**. Non-numeric input (`abc`, empty input) is
  never counted.
- A parsed guess that is **outside the configured range** still counts
  as an attempt, because the player made a deliberate, valid numeric
  guess — the range check is a game rule, not an input-format problem.

## Error Handling

- Ordinary user mistakes (bad text, out-of-range numbers, invalid
  menu choices) are handled through validation and loops, not
  exceptions — exceptions are reserved for programmer errors
  (e.g. calling `EvaluateGuess` after the game is already won, or
  constructing `GameSettings` with an invalid range), which should
  never happen through normal UI usage.

## Testing

- Framework: **xUnit**.
- Tests exercise `NumberGuessingEngine`, `GameSettings`, and
  `GameResult` directly, with no console involved — game logic is
  fully decoupled from I/O.
- Randomness is made deterministic in tests through the
  `IRandomNumberProvider` seam: most tests use the
  `NumberGuessingEngine(GameSettings, int knownSecretNumber)`
  constructor to fix the secret number outright; one test uses a
  `FixedRandomNumberProvider` test double to verify the
  random-provider code path itself.
- See `## AVIP Requirements Traceability` and the project chat report
  for the actual test count and pass/fail result — this README does
  not restate numbers that could go stale; run `dotnet test` to see
  the live result.

## How to Build

```bash
cd Task_04_Number_Guessing_Game
dotnet build
```

## How to Run

```bash
cd Task_04_Number_Guessing_Game/src/NumberGuessingGame
dotnet run
```

## How to Test

```bash
cd Task_04_Number_Guessing_Game
dotnet test
```

## Example Playthrough

*(Example only — illustrates the expected flow; not a captured
transcript of an actual run.)*

```
========================================
       NUMBER GUESSING GAME
========================================
1. Start Game
2. Instructions
3. Exit

Choose an option: 1

Select difficulty:
1. Easy   (1-50)
2. Medium (1-100)
3. Hard   (1-500)

Choice: 2

I have selected a number between 1 and 100.

Enter your guess: 25
Too low!
Enter your guess: 75
Too high!
Enter your guess: 50
Too low!
Enter your guess: 60
Correct!

========================================
GAME OVER
========================================
Secret Number: 60
Attempts: 4
```

See also `examples/sample_playthrough.txt`.

## AVIP Requirements Traceability

See the `## Requirements` table above.

## Future Improvements

*(All items below are **[NOT CURRENTLY IMPLEMENTED]**.)*

- Persistent statistics across sessions.
- Leaderboard / high-score tracking.
- Multiple rounds per session with a running score.
- Timed guessing mode.
- Web API / web UI version.
- Multiplayer support.

## Author / Internship Context

Built as part of the ArithMatrix Virtual Internship Program (AVIP)
2026, Software Development track, Task 04.
