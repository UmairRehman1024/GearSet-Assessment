namespace PdfCreator.Models;

/// <summary>
/// Formatting / layout directives recognised in the input file.
/// Lines that are not commands are treated as text content.
/// </summary>
public enum CommandKind
{
    Paragraph,
    Fill,
    NoFill,
    Regular,
    Italic,
    Bold,
    Indent,
    Large,
    Normal
}
