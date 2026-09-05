using FileOrganizer.Services;
using Xunit;

namespace FileOrganizer.Tests;

public class ConflictResolverTests
{
    [Fact]
    public void Resolve_NoExistingFile_ReturnsOriginalPath()
    {
        var resolver = new ConflictResolver(_ => false);

        var (resolvedPath, conflictResolved) = resolver.Resolve(@"C:\Target\Documents\report.pdf");

        Assert.Equal(@"C:\Target\Documents\report.pdf", resolvedPath);
        Assert.False(conflictResolved);
    }

    [Fact]
    public void Resolve_SingleConflict_AppendsUnderscoreOne()
    {
        var existing = new HashSet<string> { @"C:\Target\Documents\report.pdf" };
        var resolver = new ConflictResolver(existing.Contains);

        var (resolvedPath, conflictResolved) = resolver.Resolve(@"C:\Target\Documents\report.pdf");

        Assert.Equal(@"C:\Target\Documents\report_1.pdf", resolvedPath);
        Assert.True(conflictResolved);
    }

    [Fact]
    public void Resolve_MultipleConflicts_IncrementsUntilFree()
    {
        var existing = new HashSet<string>
        {
            @"C:\Target\Documents\report.pdf",
            @"C:\Target\Documents\report_1.pdf",
            @"C:\Target\Documents\report_2.pdf"
        };
        var resolver = new ConflictResolver(existing.Contains);

        var (resolvedPath, conflictResolved) = resolver.Resolve(@"C:\Target\Documents\report.pdf");

        Assert.Equal(@"C:\Target\Documents\report_3.pdf", resolvedPath);
        Assert.True(conflictResolved);
    }

    [Fact]
    public void Resolve_FileNameWithMultipleDots_InsertsSuffixBeforeLastExtensionOnly()
    {
        var existing = new HashSet<string> { @"C:\Target\Documents\project.final.report.pdf" };
        var resolver = new ConflictResolver(existing.Contains);

        var (resolvedPath, _) = resolver.Resolve(@"C:\Target\Documents\project.final.report.pdf");

        Assert.Equal(@"C:\Target\Documents\project.final.report_1.pdf", resolvedPath);
    }
}
