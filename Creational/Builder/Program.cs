using System;
using System.Collections.Generic;

// --- Builder Interface ---
abstract class TextConverter
{
    public abstract void ConvertText(string text);
    public abstract void ConvertBold(string text);
    public abstract void ConvertItalic(string text);
    public abstract object GetResult();
}

// --- Concrete Builders ---
class ASCIIConverter : TextConverter
{
    private string result = "";

    public override void ConvertText(string text) => result += text;

    public override void ConvertBold(string text) => result += text; // ignore formatting

    public override void ConvertItalic(string text) => result += text; // ignore formatting

    public override object GetResult() => result;
}

class TeXConverter : TextConverter
{
    private string result = "";

    public override void ConvertText(string text) => result += text;

    public override void ConvertBold(string text) => result += $"\\textbf{{{text}}}";

    public override void ConvertItalic(string text) => result += $"\\textit{{{text}}}";

    public override object GetResult() => result;
}

class TextWidgetConverter : TextConverter
{
    private List<Dictionary<string, string>> widgets = new List<Dictionary<string, string>>();

    public override void ConvertText(string text)
    {
        widgets.Add(new Dictionary<string, string> { { "type", "label" }, { "text", text } });
    }

    public override void ConvertBold(string text)
    {
        widgets.Add(new Dictionary<string, string> { { "type", "label" }, { "text", text }, { "style", "bold" } });
    }

    public override void ConvertItalic(string text)
    {
        widgets.Add(new Dictionary<string, string> { { "type", "label" }, { "text", text }, { "style", "italic" } });
    }

    public override object GetResult() => widgets;
}

// --- Director ---
class RTFReader
{
    private TextConverter converter;

    public RTFReader(TextConverter converter)
    {
        this.converter = converter;
    }

    public object Parse(List<(string tokenType, string tokenValue)> rtfTokens)
    {
        foreach (var token in rtfTokens)
        {
            switch (token.tokenType)
            {
                case "text":
                    converter.ConvertText(token.tokenValue);
                    break;
                case "bold":
                    converter.ConvertBold(token.tokenValue);
                    break;
                case "italic":
                    converter.ConvertItalic(token.tokenValue);
                    break;
            }
        }

        return converter.GetResult();
    }
}

// --- Client Code ---
class Program
{
    static void Main(string[] args)
    {
        var rtfDocument = new List<(string, string)>
        {
            ("text", "Hello "),
            ("bold", "World"),
            ("italic", "!")
        };

        // Convert to plain ASCII
        var asciiReader = new RTFReader(new ASCIIConverter());
        var asciiText = asciiReader.Parse(rtfDocument);
        Console.WriteLine("ASCII: " + asciiText);

        // Convert to TeX
        var texReader = new RTFReader(new TeXConverter());
        var texText = texReader.Parse(rtfDocument);
        Console.WriteLine("TeX: " + texText);

        // Convert to TextWidget
        var widgetReader = new RTFReader(new TextWidgetConverter());
        var widgets = widgetReader.Parse(rtfDocument);
        Console.WriteLine("TextWidget:");
        foreach (var widget in (List<Dictionary<string, string>>)widgets)
        {
            Console.WriteLine(string.Join(", ", widget));
        }
    }
}
