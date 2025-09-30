using System;
using System.Collections.Generic;

// Flyweight Interface
interface IGlyph
{
    void Draw(GlyphContext context);
}

// Concrete Flyweight
class Character : IGlyph
{
    private char _charCode;  // Intrinsic state

    public Character(char charCode)
    {
        _charCode = charCode;
    }

    public void Draw(GlyphContext context)
    {
        Console.WriteLine($"Drawing '{_charCode}' at position {context.Position} with font {context.Font}");
    }
}

// Extrinsic State Holder
class GlyphContext
{
    public int Position { get; set; }
    public string Font { get; set; }

    public GlyphContext(int position, string font)
    {
        Position = position;
        Font = font;
    }
}

// Flyweight Factory
class GlyphFactory
{
    private Dictionary<char, Character> _characters = new Dictionary<char, Character>();

    public Character GetCharacter(char c)
    {
        if (!_characters.ContainsKey(c))
        {
            _characters[c] = new Character(c);
        }
        return _characters[c];
    }
}

// Client
class DocumentEditor
{
    static void Main(string[] args)
    {
        GlyphFactory factory = new GlyphFactory();

        string text = "hello flyweight";
        string font = "Times New Roman";
        int position = 0;

        foreach (char c in text)
        {
            Character character = factory.GetCharacter(c);
            GlyphContext context = new GlyphContext(position, font);
            character.Draw(context);
            position++;
        }

        Console.WriteLine("\nTotal unique character objects created: " + factoryUniqueCount(factory));
    }

    // Utility to show total unique characters
    static int factoryUniqueCount(GlyphFactory factory)
    {
        var field = typeof(GlyphFactory).GetField("_characters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var dict = (Dictionary<char, Character>)field.GetValue(factory);
        return dict.Count;
    }
}
