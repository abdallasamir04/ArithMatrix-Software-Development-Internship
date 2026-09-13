using FileOrganizer.Models;
using FileOrganizer.Services;
using Xunit;

namespace FileOrganizer.Tests;

public class FileClassifierTests
{
    private readonly FileClassifier _classifier = new();

    [Theory]
    [InlineData("photo.jpg", FileCategory.Images)]
    [InlineData("scan.png", FileCategory.Images)]
    [InlineData("icon.svg", FileCategory.Images)]
    public void Classify_ImageExtensions_ReturnsImages(string fileName, FileCategory expected)
    {
        Assert.Equal(expected, _classifier.Classify(fileName));
    }

    [Theory]
    [InlineData("report.pdf")]
    [InlineData("notes.docx")]
    [InlineData("data.csv")]
    public void Classify_DocumentExtensions_ReturnsDocuments(string fileName)
    {
        Assert.Equal(FileCategory.Documents, _classifier.Classify(fileName));
    }

    [Theory]
    [InlineData("backup.zip")]
    [InlineData("archive.7z")]
    [InlineData("bundle.tar")]
    public void Classify_ArchiveExtensions_ReturnsArchives(string fileName)
    {
        Assert.Equal(FileCategory.Archives, _classifier.Classify(fileName));
    }

    [Theory]
    [InlineData("song.mp3")]
    [InlineData("track.flac")]
    public void Classify_AudioExtensions_ReturnsAudio(string fileName)
    {
        Assert.Equal(FileCategory.Audio, _classifier.Classify(fileName));
    }

    [Theory]
    [InlineData("movie.mp4")]
    [InlineData("clip.mkv")]
    public void Classify_VideoExtensions_ReturnsVideo(string fileName)
    {
        Assert.Equal(FileCategory.Video, _classifier.Classify(fileName));
    }

    [Fact]
    public void Classify_UnknownExtension_ReturnsOthers()
    {
        Assert.Equal(FileCategory.Others, _classifier.Classify("data.xyz"));
    }

    [Fact]
    public void Classify_NoExtension_ReturnsOthers()
    {
        Assert.Equal(FileCategory.Others, _classifier.Classify("README"));
    }

    [Theory]
    [InlineData("PHOTO.JPG")]
    [InlineData("Photo.Jpg")]
    public void Classify_IsCaseInsensitive(string fileName)
    {
        Assert.Equal(FileCategory.Images, _classifier.Classify(fileName));
    }

    [Fact]
    public void Classify_FileNameWithMultipleDots_UsesLastExtensionOnly()
    {
        Assert.Equal(FileCategory.Documents, _classifier.Classify("project.final.report.pdf"));
    }
}
