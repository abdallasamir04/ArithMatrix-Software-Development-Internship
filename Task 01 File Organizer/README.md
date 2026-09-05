# File Organizer

A cross-platform .NET console application that automatically sorts files
in a source directory into category subfolders (Images, Documents,
Archives, Audio, Video, Others) inside a target directory — safely,
deterministically, and with a dry-run mode for previewing changes.

Built for **Task 01** of the ArithMatrix Virtual Internship Program (AVIP) 2026 — Software Development track.

## Overview

Manually sorting a cluttered downloads folder is tedious and error-prone.
File Organizer scans a directory, classifies each file by its extension,
and moves it into a matching category folder — without ever overwriting
an existing file. It supports a dry-run mode so you can preview every
planned move before anything actually happens.

## Features

- Classifies files into **Images, Documents, Archives, Audio, Video**, and a catch-all **Others** category.
- **Never overwrites** an existing file; conflicts are resolved deterministically (`report.pdf` → `report_1.pdf` → `report_2.pdf`, ...).
- Correctly handles filenames with multiple dots (`project.final.report.pdf` → `project.final.report_1.pdf`).
- **Dry-run mode** (`--dry-run`): scans, classifies, and reports planned moves without touching the filesystem.
- Configurable via **CLI arguments** or a simple **key=value config file**.
- Clear console logging plus an optional mirrored log file.
- Safety guards: refuses to run when the target directory is the same as, or nested inside, the source directory.
- Meaningful process exit codes for scripting (0 = success, 1 = bad arguments, 2 = runtime error).
- Fully unit- and integration-tested with xUnit, using temporary directories only.

## Technology Stack

- C# 14 / .NET 10 (LTS)
- xUnit (automated tests)
- `System.IO` (Directory, File, Path, DirectoryInfo, FileInfo)
- No third-party runtime dependencies

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later
- Windows, Linux, or macOS

## Installation

```powershell
git clone https://github.com/<your-username>/SD_AVIP_2026_byte.git
cd SD_AVIP_2026_byte\Task_01_File_Organizer
dotnet restore
dotnet build
```

## Project Structure

```
Task_01_File_Organizer/
├── src/
│   └── FileOrganizer/
│       ├── FileOrganizer.csproj
│       ├── Program.cs
│       ├── Models/
│       │   ├── FileCategory.cs
│       │   ├── OrganizerOptions.cs
│       │   └── FileOperationResult.cs
│       ├── Services/
│       │   ├── FileOrganizerService.cs
│       │   ├── FileClassifier.cs
│       │   └── ConflictResolver.cs
│       ├── Configuration/
│       │   └── CommandLineOptions.cs
│       └── Logging/
│           └── OrganizerLogger.cs
├── tests/
│   └── FileOrganizer.Tests/
│       ├── FileOrganizer.Tests.csproj
│       ├── FileClassifierTests.cs
│       ├── ConflictResolverTests.cs
│       └── FileOrganizerServiceTests.cs
├── examples/
│   └── sample_run.log
├── screenshots/
├── README.md
├── .gitignore
└── FileOrganizer.sln
```

## Usage

Normal mode:

```powershell
dotnet run --project src\FileOrganizer -- --source "C:\Users\Me\Downloads" --target "C:\Users\Me\Organized"
```

Dry-run mode (nothing is moved, only reported):

```powershell
dotnet run --project src\FileOrganizer -- --source "C:\Users\Me\Downloads" --target "C:\Users\Me\Organized" --dry-run
```

Using a config file instead of CLI flags:

```powershell
dotnet run --project src\FileOrganizer -- --config "organizer.config"
```

`organizer.config` example:

```
source=C:\Users\Me\Downloads
target=C:\Users\Me\Organized
dry-run=true
```

Build and test:

```powershell
dotnet build
dotnet test
```

## Supported File Types

| Category  | Extensions |
|-----------|------------|
| Images    | .jpg .jpeg .png .gif .bmp .webp .svg .tiff .tif .heic |
| Documents | .pdf .doc .docx .txt .rtf .xls .xlsx .csv .ppt .pptx .odt .md |
| Archives  | .zip .rar .7z .tar .gz .bz2 .xz |
| Audio     | .mp3 .wav .flac .aac .ogg .m4a .wma |
| Video     | .mp4 .mkv .avi .mov .wmv .webm .flv .m4v |
| Others    | any unrecognized extension, or no extension at all |

## Conflict Resolution

The program **never** overwrites an existing file. If the destination
filename already exists, a numeric suffix is inserted **before the last
extension only**:

```
report.pdf              (already exists)
report_1.pdf             (already exists)
report_2.pdf             <- new file lands here
```

Filenames with multiple dots are handled correctly:

```
project.final.report.pdf  -> project.final.report_1.pdf
```

(not `project_1.final.report.pdf`).

## Dry Run

`--dry-run` performs every step **except** the actual file move: the
source directory is scanned, files are classified, destination paths are
computed, and conflicts are resolved against what is genuinely on disk —
but `Directory.CreateDirectory` and `File.Move` are never called, and the
target directory is not even created. Output is prefixed with `[DRY-RUN]`
instead of `[MOVED]`.

## Logging

Every run prints a header (mode, source, target), one line per file
processed, and a final summary (files scanned / organized / errors) to the
console. Passing `--log <path>` additionally writes the same text to a log
file on disk.

## Error Handling

| Situation | Behavior |
|---|---|
| Source directory does not exist | Application exits with an error message and exit code 2; nothing is scanned. |
| Target directory does not exist | Created automatically (in live mode only). |
| Source and target are the same directory | Refused with a clear error before any file is touched. |
| Target is nested inside source | Refused, to avoid the tool reprocessing the files it just moved. |
| A file cannot be read/moved (locked, permissions) | That single file is reported as `[ERROR]`; the run continues with the remaining files. |
| Unknown or missing extension | File is placed in `Others`; never rejected. |

## Testing

```powershell
dotnet test
```

Tests use `Path.GetTempPath()` to create an isolated temporary directory
per test class and delete it afterward — no test ever reads or writes
outside of its own temp folder.

## Example

Before:

```
Downloads/
├── photo.jpg
├── report.pdf
├── song.mp3
├── movie.mp4
└── archive.zip
```

After:

```
Organized/
├── Images/
│   └── photo.jpg
├── Documents/
│   └── report.pdf
├── Audio/
│   └── song.mp3
├── Video/
│   └── movie.mp4
└── Archives/
    └── archive.zip
```

## Sample Output

See [`examples/sample_run.log`](examples/sample_run.log) for a full
dry-run + live-run console transcript.

## Screenshots

Place these in `screenshots/`:

1. `before.png` — the source folder before organizing (mixed file types).
2. `dry-run.png` — terminal output of a `--dry-run` execution.
3. `after.png` — the resulting category folders in the target directory.
4. `repo-structure.png` — the project structure as seen on GitHub. *(optional)*

## Future Improvements

*(Not implemented — ideas only)*

- Undo/rollback of the last run.
- Config-driven, user-customizable extension-to-category mapping (currently requires a code change).
- Parallel file processing for very large directories.
- Structured (JSON) log output in addition to plain text.

## Internship Requirements Checklist

| Official Requirement | Status |
|---|---|
| Sort files into subfolders by type (Images/Documents/Archives/Audio/Video) | ✅ `FileClassifier` + `FileOrganizerService` |
| Handle common extensions, place into correctly named folders | ✅ Extension map in `FileClassifier` |
| Never overwrite existing files | ✅ `File.Move(..., overwrite: false)` + pre-check in `ConflictResolver` |
| Deterministic conflict resolution | ✅ `ConflictResolver` (`_1`, `_2`, ...) |
| Configurable source/target directory (CLI or config file) | ✅ `CommandLineOptions` (`--source/--target` or `--config`) |
| Dry-run / logging option showing planned moves | ✅ `--dry-run` + `OrganizerLogger` |
| Deliverable: GitHub repository | ⬜ To be pushed by the developer |
| Deliverable: README with usage examples | ✅ This file |
| Deliverable: sample run log | ✅ `examples/sample_run.log` |
| Deliverable: before/after screenshots | ⬜ To be captured by the developer (see Screenshots section) |
