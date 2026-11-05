using PdfCreator.Models;

namespace PdfCreator.Rendering;

public interface IPdfDocumentWriter
{
    void Write(IReadOnlyList<DocumentToken> tokens, string outputPath);
}
