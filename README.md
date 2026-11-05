# PdfCreator — Gearset internship assessment

C# console app that reads a command-annotated text file and writes a formatted PDF (iText 7).

## Requirements (from the brief)

- Input: text file with directives (`.paragraph`, `.fill`, `.nofill`, `.regular`, `.italic`, `.bold`, `.indent N`, `.large`, `.normal`) plus body text
- Output: PDF matching the sample formatting
- Repeat content so the PDF has **at least 3 pages**
- Prefer clear abstractions and unit tests

## Layout

```
PdfCreator.sln
samples/input.txt              # assessment sample
src/PdfCreator/
  Program.cs                   # CLI entry
  Parsing/                     # line → DocumentToken
  Models/                      # tokens + formatting state
  Rendering/                   # tokens → PDF (iText)
tests/PdfCreator.Tests/
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

If `dotnet` is not on your PATH after a user-local install:

```bash
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$HOME/.dotnet:$PATH"
```

## Run

```bash
dotnet restore
dotnet run --project src/PdfCreator -- samples/input.txt output.pdf
```

Or with defaults (bundled `samples/input.txt` → `output.pdf` in the working directory):

```bash
dotnet run --project src/PdfCreator
```

## Test

```bash
dotnet test
```

## What’s scaffolded vs still to finish

**Done enough to iterate**

- Solution + console + xUnit projects
- iText 7 packages
- Sample input
- `DocumentParser` (commands → tokens) with tests
- `PdfDocumentWriter` skeleton that emits a multi-page PDF

**Still to implement / harden** (see TODOs in `PdfDocumentWriter`)

- True bold / italic fonts on inline runs
- Correct `.fill` behaviour (last line of a paragraph should not be stretched)
- Indent unit calibration (~ width of `"WWWW"`)
- Confirm when commands imply a paragraph break
- Any notes/assumptions for the submission zip
