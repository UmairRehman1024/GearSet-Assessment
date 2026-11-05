using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PdfCreator.Models;

namespace PdfCreator.Rendering;

/// <summary>
/// Renders parsed tokens to a PDF using iText.
/// Layout rules (fill justification, indent units, mixed inline styles, page breaks)
/// still need to be completed — see TODOs below.
/// </summary>
public sealed class PdfDocumentWriter : IPdfDocumentWriter
{
    private const float NormalFontSize = 12f;
    private const float LargeFontSize = 24f;
    // Assessment hint: one indent unit ≈ width of "WWWW"
    private const float IndentUnitPoints = 40f;

    public void Write(IReadOnlyList<DocumentToken> tokens, string outputPath)
    {
        using var writer = new PdfWriter(outputPath);
        using var pdf = new PdfDocument(writer);
        using var document = new Document(pdf);

        var state = new FormattingState();
        var currentRuns = new List<(string Text, FormattingState Style)>();

        void FlushParagraph()
        {
            if (currentRuns.Count == 0)
            {
                return;
            }

            // TODO: honour .fill / .nofill (justify vs left), and keep last line of a
            // filled paragraph left-aligned rather than stretched.
            var paragraph = new Paragraph()
                .SetTextAlignment(state.Fill ? TextAlignment.JUSTIFIED : TextAlignment.LEFT)
                .SetMarginLeft(Math.Max(0, state.IndentLevel) * IndentUnitPoints)
                .SetFontSize(state.Large ? LargeFontSize : NormalFontSize);

            foreach (var (text, style) in currentRuns)
            {
                // TODO: apply bold / italic per-run correctly via PdfFontFactory
                // (inline style changes mid-paragraph should not force a new Paragraph).
                var run = new Text(text + " ");
                if (style.Large)
                {
                    run.SetFontSize(LargeFontSize);
                }

                paragraph.Add(run);
            }

            document.Add(paragraph);
            currentRuns.Clear();
        }

        foreach (var token in tokens)
        {
            switch (token)
            {
                case CommandToken { Kind: CommandKind.Paragraph }:
                    FlushParagraph();
                    break;

                case CommandToken { Kind: CommandKind.Fill }:
                    state.Fill = true;
                    break;

                case CommandToken { Kind: CommandKind.NoFill }:
                    state.Fill = false;
                    break;

                case CommandToken { Kind: CommandKind.Regular }:
                    state.Italic = false;
                    state.Bold = false;
                    break;

                case CommandToken { Kind: CommandKind.Italic }:
                    state.Italic = true;
                    state.Bold = false;
                    break;

                case CommandToken { Kind: CommandKind.Bold }:
                    state.Bold = true;
                    state.Italic = false;
                    break;

                case CommandToken { Kind: CommandKind.Large }:
                    FlushParagraph();
                    state.Large = true;
                    break;

                case CommandToken { Kind: CommandKind.Normal }:
                    FlushParagraph();
                    state.Large = false;
                    break;

                case CommandToken { Kind: CommandKind.Indent, IndentDelta: { } delta }:
                    // TODO: confirm whether .indent always implies a paragraph break.
                    FlushParagraph();
                    state.IndentLevel += delta;
                    break;

                case TextToken text:
                    // Snapshot style at the time the text arrives so mid-paragraph
                    // .italic / .bold changes apply only to following runs.
                    currentRuns.Add((text.Text, Clone(state)));
                    break;
            }
        }

        FlushParagraph();
    }

    private static FormattingState Clone(FormattingState source) =>
        new()
        {
            Fill = source.Fill,
            Italic = source.Italic,
            Bold = source.Bold,
            Large = source.Large,
            IndentLevel = source.IndentLevel
        };
}
