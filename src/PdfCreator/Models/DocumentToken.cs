namespace PdfCreator.Models;

/// <summary>
/// One parsed unit from the input file: either a command or a line of text.
/// </summary>
public abstract record DocumentToken;

public sealed record CommandToken(CommandKind Kind, int? IndentDelta = null) : DocumentToken;

public sealed record TextToken(string Text) : DocumentToken;
