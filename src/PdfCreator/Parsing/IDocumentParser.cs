using PdfCreator.Models;

namespace PdfCreator.Parsing;

public interface IDocumentParser
{
    IReadOnlyList<DocumentToken> Parse(IEnumerable<string> lines);
}
