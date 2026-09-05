using FileOrganizer.Models;

namespace FileOrganizer.Services;

/// <summary>
/// Maps a file extension to a <see cref="FileCategory"/>.
///
/// The mapping is stored as a single dictionary keyed by lower-case extension
/// (including the leading dot, e.g. ".jpg"). This keeps the classifier a pure,
/// stateless lookup: no branching logic, no chain of if/else, easy to extend
/// by adding one dictionary entry, and easy to unit test in isolation.
/// </summary>
public sealed class FileClassifier
{
    private static readonly Dictionary<string, FileCategory> ExtensionMap = BuildExtensionMap();

    /// <summary>
    /// Classifies a file based on its extension. Comparison is case-insensitive
    /// (".JPG" and ".jpg" both resolve to Images) because Windows filenames are
    /// case-insensitive in practice and users routinely receive files with
    /// uppercase extensions from cameras, phones, and other tools.
    /// Files with no extension, or with an extension we do not recognize,
    /// are classified as <see cref="FileCategory.Others"/> rather than being
    /// rejected — every file must land somewhere so nothing is silently dropped.
    /// </summary>
    public FileCategory Classify(string fileName)
    {
        var extension = Path.GetExtension(fileName);

        if (string.IsNullOrEmpty(extension))
        {
            return FileCategory.Others;
        }

        return ExtensionMap.TryGetValue(extension.ToLowerInvariant(), out var category)
            ? category
            : FileCategory.Others;
    }

    /// <summary>
    /// Builds the extension-to-category table.
    ///
    /// Category choices, briefly:
    ///  - Images: common raster/vector formats produced by cameras, phones,
    ///    screenshots, and design tools.
    ///  - Documents: office/text/spreadsheet/presentation formats people
    ///    typically call "documents" in everyday use, including PDF.
    ///  - Archives: compressed/packaged container formats.
    ///  - Audio: common compressed and lossless audio formats.
    ///  - Video: common container/codec formats for video.
    ///  - Others: the catch-all for anything unrecognized or extension-less,
    ///    so the tool never fails to place a file, but also never guesses.
    /// </summary>
    private static Dictionary<string, FileCategory> BuildExtensionMap()
    {
        var map = new Dictionary<string, FileCategory>(StringComparer.Ordinal);

        string[] images = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".tiff", ".tif", ".heic"];
        string[] documents = [".pdf", ".doc", ".docx", ".txt", ".rtf", ".xls", ".xlsx", ".csv", ".ppt", ".pptx", ".odt", ".md"];
        string[] archives = [".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz"];
        string[] audio = [".mp3", ".wav", ".flac", ".aac", ".ogg", ".m4a", ".wma"];
        string[] video = [".mp4", ".mkv", ".avi", ".mov", ".wmv", ".webm", ".flv", ".m4v"];

        Register(map, images, FileCategory.Images);
        Register(map, documents, FileCategory.Documents);
        Register(map, archives, FileCategory.Archives);
        Register(map, audio, FileCategory.Audio);
        Register(map, video, FileCategory.Video);

        return map;
    }

    private static void Register(Dictionary<string, FileCategory> map, string[] extensions, FileCategory category)
    {
        foreach (var extension in extensions)
        {
            map[extension] = category;
        }
    }
}
