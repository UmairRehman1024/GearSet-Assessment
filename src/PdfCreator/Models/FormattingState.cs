namespace PdfCreator.Models;

/// <summary>
/// Mutable formatting state applied while walking document tokens.
/// </summary>
public sealed class FormattingState
{
    public bool Fill { get; set; }
    public bool Italic { get; set; }
    public bool Bold { get; set; }
    public bool Large { get; set; }
    public int IndentLevel { get; set; }
}
