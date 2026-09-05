using FileOrganizer.Logging;
using FileOrganizer.Models;
using FileOrganizer.Services;
using Xunit;

namespace FileOrganizer.Tests;

/// <summary>
/// Integration-style tests that exercise the real filesystem, but only ever
/// inside a freshly created temp directory (never the developer's own
/// filesystem/paths), so tests are safe to run on any machine and clean up
/// after themselves via IDisposable.
/// </summary>
public class FileOrganizerServiceTests : IDisposable
{
    private readonly string _root;
    private readonly string _source;
    private readonly string _target;
    private readonly FileOrganizerService _service;

    public FileOrganizerServiceTests()
    {
        _root = Path.Combine(Path.GetTempPath(), "FileOrganizerTests_" + Guid.NewGuid());
        _source = Path.Combine(_root, "Source");
        _target = Path.Combine(_root, "Target");
        Directory.CreateDirectory(_source);

        _service = new FileOrganizerService(new FileClassifier(), new ConflictResolver());
    }

    public void Dispose()
    {
        if (Directory.Exists(_root))
        {
            Directory.Delete(_root, recursive: true);
        }
    }

    private static OrganizerLogger SilentLogger() => new(logFilePath: null);

    private void CreateSourceFile(string name, string content = "x") =>
        File.WriteAllText(Path.Combine(_source, name), content);

    [Fact]
    public void Organize_MissingSourceDirectory_ThrowsDirectoryNotFoundException()
    {
        var missingSource = Path.Combine(_root, "DoesNotExist");
        var options = new OrganizerOptions { SourceDirectory = missingSource, TargetDirectory = _target };

        Assert.Throws<DirectoryNotFoundException>(() => _service.Organize(options, SilentLogger()));
    }

    [Fact]
    public void Organize_EmptySourceDirectory_ReturnsNoResults()
    {
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _target };

        var results = _service.Organize(options, SilentLogger());

        Assert.Empty(results);
    }

    [Fact]
    public void Organize_CreatesDestinationFoldersAndMovesFiles()
    {
        CreateSourceFile("photo.jpg");
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _target };

        var results = _service.Organize(options, SilentLogger());

        var expectedPath = Path.Combine(_target, "Images", "photo.jpg");
        Assert.True(File.Exists(expectedPath));
        Assert.False(File.Exists(Path.Combine(_source, "photo.jpg")));
        Assert.Single(results);
        Assert.Equal(OperationStatus.Moved, results[0].Status);
    }

    [Fact]
    public void Organize_MultipleFilesOfDifferentCategories_AllPlacedCorrectly()
    {
        CreateSourceFile("photo.jpg");
        CreateSourceFile("report.pdf");
        CreateSourceFile("song.mp3");
        CreateSourceFile("movie.mp4");
        CreateSourceFile("archive.zip");
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _target };

        var results = _service.Organize(options, SilentLogger());

        Assert.Equal(5, results.Count);
        Assert.True(File.Exists(Path.Combine(_target, "Images", "photo.jpg")));
        Assert.True(File.Exists(Path.Combine(_target, "Documents", "report.pdf")));
        Assert.True(File.Exists(Path.Combine(_target, "Audio", "song.mp3")));
        Assert.True(File.Exists(Path.Combine(_target, "Video", "movie.mp4")));
        Assert.True(File.Exists(Path.Combine(_target, "Archives", "archive.zip")));
    }

    [Fact]
    public void Organize_DryRun_DoesNotMoveOrCreateAnyFiles()
    {
        CreateSourceFile("photo.jpg");
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _target, DryRun = true };

        var results = _service.Organize(options, SilentLogger());

        Assert.True(File.Exists(Path.Combine(_source, "photo.jpg")), "Source file must remain untouched.");
        Assert.False(Directory.Exists(_target), "Target directory must not be created in dry-run mode.");
        Assert.Single(results);
        Assert.Equal(OperationStatus.Planned, results[0].Status);
    }

    [Fact]
    public void Organize_ExistingFileAtDestination_IsNeverOverwritten_ConflictRenamed()
    {
        CreateSourceFile("report.pdf", content: "new content");
        var destinationFolder = Path.Combine(_target, "Documents");
        Directory.CreateDirectory(destinationFolder);
        var preExistingPath = Path.Combine(destinationFolder, "report.pdf");
        File.WriteAllText(preExistingPath, "original content - must survive");

        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _target };
        var results = _service.Organize(options, SilentLogger());

        Assert.Equal("original content - must survive", File.ReadAllText(preExistingPath));
        var renamedPath = Path.Combine(destinationFolder, "report_1.pdf");
        Assert.True(File.Exists(renamedPath));
        Assert.Equal("new content", File.ReadAllText(renamedPath));
        Assert.True(results[0].ConflictResolved);
    }

    [Fact]
    public void Organize_ThreeFilesWithSameName_AllPreservedWithDeterministicNames()
    {
        // Simulate three separately-named source files that all classify to
        // the same destination file name by pre-seeding the target folder,
        // then moving one more file with that exact name from source.
        var destinationFolder = Path.Combine(_target, "Documents");
        Directory.CreateDirectory(destinationFolder);
        File.WriteAllText(Path.Combine(destinationFolder, "report.pdf"), "v0");
        File.WriteAllText(Path.Combine(destinationFolder, "report_1.pdf"), "v1");

        CreateSourceFile("report.pdf", content: "v2");
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _target };

        _service.Organize(options, SilentLogger());

        Assert.True(File.Exists(Path.Combine(destinationFolder, "report_2.pdf")));
        Assert.Equal("v2", File.ReadAllText(Path.Combine(destinationFolder, "report_2.pdf")));
    }

    [Fact]
    public void Organize_FileNameWithMultipleDots_ConflictInsertsSuffixBeforeLastExtension()
    {
        var destinationFolder = Path.Combine(_target, "Documents");
        Directory.CreateDirectory(destinationFolder);
        File.WriteAllText(Path.Combine(destinationFolder, "project.final.report.pdf"), "old");

        CreateSourceFile("project.final.report.pdf", content: "new");
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _target };

        _service.Organize(options, SilentLogger());

        Assert.True(File.Exists(Path.Combine(destinationFolder, "project.final.report_1.pdf")));
    }

    [Fact]
    public void Organize_SourceEqualsTarget_ThrowsInvalidOperationException()
    {
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _source };

        Assert.Throws<InvalidOperationException>(() => _service.Organize(options, SilentLogger()));
    }

    [Fact]
    public void Organize_TargetInsideSource_ThrowsInvalidOperationException()
    {
        var nestedTarget = Path.Combine(_source, "Organized");
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = nestedTarget };

        Assert.Throws<InvalidOperationException>(() => _service.Organize(options, SilentLogger()));
    }

    [Fact]
    public void Organize_UnknownExtension_GoesToOthersFolder()
    {
        CreateSourceFile("data.xyz");
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _target };

        _service.Organize(options, SilentLogger());

        Assert.True(File.Exists(Path.Combine(_target, "Others", "data.xyz")));
    }

    [Fact]
    public void Organize_FileWithoutExtension_GoesToOthersFolder()
    {
        CreateSourceFile("README");
        var options = new OrganizerOptions { SourceDirectory = _source, TargetDirectory = _target };

        _service.Organize(options, SilentLogger());

        Assert.True(File.Exists(Path.Combine(_target, "Others", "README")));
    }
}
