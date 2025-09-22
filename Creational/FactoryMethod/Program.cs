using System;

// Abstract Product
abstract class Document
{
    public abstract void Open();
    public abstract void Save();
}

// Concrete Products
class DrawingDocument : Document
{
    public override void Open()
    {
        Console.WriteLine("Opening a drawing document.");
    }

    public override void Save()
    {
        Console.WriteLine("Saving a drawing document.");
    }
}

class TextDocument : Document
{
    public override void Open()
    {
        Console.WriteLine("Opening a text document.");
    }

    public override void Save()
    {
        Console.WriteLine("Saving a text document.");
    }
}

// Abstract Creator
abstract class Application
{
    // Factory Method
    public abstract Document CreateDocument();

    public void NewDocument()
    {
        Document doc = CreateDocument(); // Creation is deferred to subclass
        doc.Open();
        Console.WriteLine("Document created through Factory Method.");
    }
}

// Concrete Creators
class DrawingApplication : Application
{
    public override Document CreateDocument()
    {
        return new DrawingDocument();
    }
}

class TextApplication : Application
{
    public override Document CreateDocument()
    {
        return new TextDocument();
    }
}

// Client
class Program
{
    static void Main(string[] args)
    {
        Application app;

        // Using DrawingApplication
        app = new DrawingApplication();
        app.NewDocument();

        Console.WriteLine();

        // Using TextApplication
        app = new TextApplication();
        app.NewDocument();
    }
}
