using PdfCreator.Models;
using PdfCreator.Parsing;

namespace PdfCreator.Tests;

public class DocumentParserTests
{
    private readonly DocumentParser _parser = new();

    [Fact]
    public void Parse_RecognisesKnownCommands()
    {
        var tokens = _parser.Parse(
        [
            ".large",
            "Title",
            ".normal",
            ".paragraph",
            ".italic",
            "word",
            ".regular",
            ".bold",
            "loud",
            ".fill",
            ".nofill",
            ".indent +2",
            ".indent -1"
        ]);

        Assert.Equal(
        [
            typeof(CommandToken),
            typeof(TextToken),
            typeof(CommandToken),
            typeof(CommandToken),
            typeof(CommandToken),
            typeof(TextToken),
            typeof(CommandToken),
            typeof(CommandToken),
            typeof(TextToken),
            typeof(CommandToken),
            typeof(CommandToken),
            typeof(CommandToken),
            typeof(CommandToken)
        ], tokens.Select(t => t.GetType()));

        Assert.Equal(CommandKind.Large, ((CommandToken)tokens[0]).Kind);
        Assert.Equal("Title", ((TextToken)tokens[1]).Text);
        Assert.Equal(2, ((CommandToken)tokens[11]).IndentDelta);
        Assert.Equal(-1, ((CommandToken)tokens[12]).IndentDelta);
    }

    [Fact]
    public void Parse_SkipsBlankLines()
    {
        var tokens = _parser.Parse(["", "  ", "hello", ""]);
        var text = Assert.Single(tokens);
        Assert.Equal("hello", Assert.IsType<TextToken>(text).Text);
    }

    [Fact]
    public void Parse_TreatsUnknownDottedLineAsText()
    {
        var tokens = _parser.Parse([".notacommand"]);
        var text = Assert.Single(tokens);
        Assert.Equal(".notacommand", Assert.IsType<TextToken>(text).Text);
    }
}
