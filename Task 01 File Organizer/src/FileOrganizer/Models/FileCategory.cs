namespace FileOrganizer.Models;

/// <summary>
/// Represents the destination category a file is classified into.
/// Each category maps 1:1 to a subfolder name under the target directory.
/// </summary>
public enum FileCategory
{
    Images,
    Documents,
    Archives,
    Audio,
    Video,
    Others
}
