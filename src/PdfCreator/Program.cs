using PdfCreator.Models;
using PdfCreator.Parsing;
using PdfCreator.Rendering;

// Usage:
//   dotnet run --project src/PdfCreator -- [inputPath] [outputPath]
// Defaults: samples/input.txt → output.pdf

var inputPath = args.ElementAtOrDefault(0) ?? Path.Combine(AppContext.BaseDirectory, "samples", "input.txt");
var outputPath = args.ElementAtOrDefault(1) ?? "output.pdf";

if (!File.Exists(inputPath))
{
    Console.Error.WriteLine($"Input file not found: {inputPath}");
    Console.Error.WriteLine("Pass an explicit path, e.g. dotnet run -- samples/input.txt output.pdf");
    return 1;
}

var lines = await File.ReadAllLinesAsync(inputPath);
var parser = new DocumentParser();
var sampleTokens = parser.Parse(lines);

// Assessment: repeat content so pagination covers at least 3 pages.
const int repetitions = 8;
var documentTokens = new List<DocumentToken>(sampleTokens.Count * repetitions);
for (var i = 0; i < repetitions; i++)
{
    documentTokens.AddRange(sampleTokens);
    if (i < repetitions - 1)
    {
        documentTokens.Add(new CommandToken(CommandKind.Paragraph));
    }
}

new PdfDocumentWriter().Write(documentTokens, outputPath);

Console.WriteLine($"Wrote '{outputPath}' from '{inputPath}' ({repetitions}× sample, {documentTokens.Count} tokens).");
return 0;
