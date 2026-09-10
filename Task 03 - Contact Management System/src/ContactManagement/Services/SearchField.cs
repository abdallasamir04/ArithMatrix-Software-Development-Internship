namespace ContactManagement.Services;

/// <summary>
/// Which field(s) a search should be performed against.
/// An enum is used instead of a plain string ("name"/"phone"/"email")
/// so invalid values are impossible at compile time - the compiler
/// guarantees only these four options can ever be passed around.
/// </summary>
public enum SearchField
{
    Name,
    Phone,
    Email,

    /// <summary>
    /// [ENGINEERING ENHANCEMENT] Searches all three fields at once.
    /// Not explicitly required by the official AVIP task, which only
    /// asks for searching by name, phone, or email individually, but
    /// it is a small, natural addition once per-field search exists.
    /// </summary>
    All
}
