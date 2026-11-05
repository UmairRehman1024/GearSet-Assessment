using System.Globalization;
using PdfCreator.Models;

namespace PdfCreator.Parsing;

/// <summary>
/// Turns raw input lines into <see cref="DocumentToken"/>s.
/// Unknown dotted lines are treated as text so odd content is not silently dropped.
/// </summary>
public sealed class DocumentParser : IDocumentParser
{
    public IReadOnlyList<DocumentToken> Parse(IEnumerable<string> lines)
    {
        var tokens = new List<DocumentToken>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.TrimEnd();

            if (TryParseCommand(line, out var command))
            {
                tokens.Add(command);
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                tokens.Add(new TextToken(line.Trim()));
            }
        }

        return tokens;
    }

    private static bool TryParseCommand(string line, out CommandToken command)
    {
        command = null!;

        if (string.IsNullOrWhiteSpace(line) || line[0] != '.')
        {
            return false;
        }

        var parts = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        var name = parts[0].ToLowerInvariant();

        switch (name)
        {
            case ".paragraph":
                command = new CommandToken(CommandKind.Paragraph);
                return true;
            case ".fill":
                command = new CommandToken(CommandKind.Fill);
                return true;
            case ".nofill":
                command = new CommandToken(CommandKind.NoFill);
                return true;
            case ".regular":
                command = new CommandToken(CommandKind.Regular);
                return true;
            case ".italic":
                command = new CommandToken(CommandKind.Italic);
                return true;
            case ".bold":
                command = new CommandToken(CommandKind.Bold);
                return true;
            case ".large":
                command = new CommandToken(CommandKind.Large);
                return true;
            case ".normal":
                command = new CommandToken(CommandKind.Normal);
                return true;
            case ".indent" when parts.Length == 2
                && int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var delta):
                command = new CommandToken(CommandKind.Indent, delta);
                return true;
            default:
                return false;
        }
    }
}
