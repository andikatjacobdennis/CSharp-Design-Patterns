using System;
using System.Collections.Generic;

// Receiver
class Document
{
    public void Open() => Console.WriteLine("Document opened.");
    public void Paste() => Console.WriteLine("Content pasted.");
    public void Save() => Console.WriteLine("Document saved.");
}

// Command Interface
interface ICommand
{
    void Execute();
}

// Concrete Commands
class OpenCommand : ICommand
{
    private Document _document;
    public OpenCommand(Document doc) { _document = doc; }
    public void Execute() { _document.Open(); }
}

class PasteCommand : ICommand
{
    private Document _document;
    public PasteCommand(Document doc) { _document = doc; }
    public void Execute() { _document.Paste(); }
}

class SaveCommand : ICommand
{
    private Document _document;
    public SaveCommand(Document doc) { _document = doc; }
    public void Execute() { _document.Save(); }
}

// Macro Command
class MacroCommand : ICommand
{
    private List<ICommand> _commands = new List<ICommand>();
    
    public void Add(ICommand command) => _commands.Add(command);
    public void Remove(ICommand command) => _commands.Remove(command);

    public void Execute()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }
}

// Invoker
class MenuItem
{
    private ICommand _command;
    public MenuItem(ICommand command) { _command = command; }
    public void Click() { _command.Execute(); }
}

// Client
class Program
{
    static void Main()
    {
        Document doc = new Document();

        // Create individual commands
        ICommand openCmd = new OpenCommand(doc);
        ICommand pasteCmd = new PasteCommand(doc);
        ICommand saveCmd = new SaveCommand(doc);

        // Create menu items for individual commands
        MenuItem openMenu = new MenuItem(openCmd);
        MenuItem pasteMenu = new MenuItem(pasteCmd);
        MenuItem saveMenu = new MenuItem(saveCmd);

        // Execute individual commands
        openMenu.Click();
        pasteMenu.Click();
        saveMenu.Click();

        Console.WriteLine("\n--- Macro Command Example ---");

        // Create a macro command
        MacroCommand macro = new MacroCommand();
        macro.Add(openCmd);
        macro.Add(pasteCmd);
        macro.Add(saveCmd);

        // Menu item for macro
        MenuItem macroMenu = new MenuItem(macro);
        macroMenu.Click();
    }
}
