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
