using System;
using System.Collections.Generic;

// --- 1. Core Interface / Abstract Class (Mediator) ---
/// <summary>
/// The abstract Mediator defines the interface for Colleague objects to communicate with.
/// </summary>
public abstract class DialogDirector
{
    /// <summary>
    /// Widgets call this method to notify the director that their state has changed.
    /// The director then coordinates the interaction among colleagues.
    /// </summary>
    public abstract void WidgetChanged(Widget widget);
    
    /// <summary>
    /// Abstract method to create and initialize the specific widgets in the dialog.
    /// </summary>
    protected abstract void CreateWidgets();
}

// --- 1. Core Interface / Abstract Class (Colleague) ---
/// <summary>
/// The abstract Colleague maintains a reference to its Mediator.
/// It delegates communication to the director instead of interacting directly with other colleagues.
/// </summary>
public abstract class Widget
{
    protected DialogDirector _director;

    public Widget(DialogDirector director)
    {
        // Colleagues only know about their director
        _director = director;
    }

    /// <summary>
    /// Called by concrete colleagues when a significant event occurs (e.g., user interaction).
    /// This method notifies the director.
    /// </summary>
    public void Changed()
    {
        _director.WidgetChanged(this);
    }
}

// --- 2. Concrete Implementations (Colleagues) ---

/// <summary>
/// A Concrete Colleague representing a list of selectable items.
/// </summary>
public class ListBox : Widget
{
    private string _selection = "Times New Roman";

    public ListBox(DialogDirector director) : base(director) { }

    public string GetSelection() => _selection;

    /// <summary>
    /// Simulates a user selecting a new item, which triggers notification to the director.
    /// </summary>
    public void SetSelection(string selection)
    {
        Console.WriteLine($"[ListBox]: Selection changed to '{selection}'.");
        _selection = selection;
        Changed(); // Notify the director that state has changed
    }
}

/// <summary>
/// A Concrete Colleague representing a text input field.
/// </summary>
public class EntryField : Widget
{
    private string _text = "";

    public EntryField(DialogDirector director) : base(director) { }

    public void SetText(string text)
    {
        Console.WriteLine($"[EntryField]: Text set to '{text}'.");
        _text = text;
        
        // Simulating the director setting the text based on ListBox selection.
        // If this were a user typing, we'd call Changed() here too to update other controls.
    }

    public string GetText() => _text;
}

/// <summary>
/// A Concrete Colleague representing a clickable button.
/// </summary>
public class Button : Widget
{
    public string Text { get; }
    public bool IsEnabled { get; private set; }

    public Button(DialogDirector director, string text) : base(director)
    {
        Text = text;
        IsEnabled = false;
    }

    /// <summary>
    /// Simulates a user pressing the button, which triggers notification if enabled.
    /// </summary>
    public void SimulatePress()
    {
        if (IsEnabled)
        {
            Console.WriteLine($"[Button]: '{Text}' pressed. Notifying director.");
            Changed(); // Notify the director that the button was pressed
        }
        else
        {
            Console.WriteLine($"[Button]: '{Text}' is disabled and cannot be pressed.");
        }
    }

    public void SetEnabled(bool enabled)
    {
        IsEnabled = enabled;
        Console.WriteLine($"[Button]: '{Text}' is now {(enabled ? "ENABLED" : "DISABLED")}.");
    }
}

// --- 2. Concrete Implementations (Mediator) ---
/// <summary>
/// The Concrete Mediator (Director) that coordinates the specific Font Dialog widgets.
/// It contains the collective behavior logic.
/// </summary>
public class FontDialogDirector : DialogDirector
{
    // The director must know and maintain its specific colleagues
    private ListBox _fontList;
    private EntryField _fontName;
    private Button _okButton;
    private Button _cancelButton;

    public FontDialogDirector()
    {
        // Initialization creates the colleagues and registers itself as their director
        CreateWidgets();
    }

    protected override void CreateWidgets()
    {
        _fontList = new ListBox(this);
        _fontName = new EntryField(this);
        _okButton = new Button(this, "OK");
        _cancelButton = new Button(this, "Cancel");

        // Initial state logic: if the EntryField is empty, disable OK button
        _okButton.SetEnabled(!string.IsNullOrEmpty(_fontName.GetText()));

        Console.WriteLine("\n--- Font Dialog Initialized ---");
    }

    /// <summary>
    /// The central logic for coordinating colleague behavior.
    /// </summary>
    public override void WidgetChanged(Widget theChangedWidget)
    {
        if (theChangedWidget == _fontList)
        {
            Console.WriteLine("\n[Director Action]: ListBox changed. Coordinating...");
            
            // 1. Get the current selection from the ListBox
            string selection = _fontList.GetSelection();
            
            // 2. Update the EntryField with the new selection (interaction logic)
            _fontName.SetText(selection);

            // 3. Update the OK button state based on the EntryField content
            _okButton.SetEnabled(!string.IsNullOrEmpty(_fontName.GetText()));
        }
        else if (theChangedWidget == _fontName)
        {
            Console.WriteLine("\n[Director Action]: EntryField text changed (e.g., by user typing). Coordinating...");
             // Only update the OK button state if EntryField content changes
            _okButton.SetEnabled(!string.IsNullOrEmpty(_fontName.GetText()));
        }
        else if (theChangedWidget == _okButton)
        {
            Console.WriteLine($"\n[Director Action]: OK button pressed. Applying font '{_fontName.GetText()}' and closing dialog.");
        }
        else if (theChangedWidget == _cancelButton)
        {
            Console.WriteLine("\n[Director Action]: Cancel button pressed. Dismissing dialog.");
        }
    }

    // Public getters to allow the client (Main method) to simulate actions on the colleagues
    public ListBox FontList => _fontList;
    public EntryField FontName => _fontName;
    public Button OkButton => _okButton;
}


// --- 3. Client ---
public class Program
{
    public static void Main(string[] args)
    {
        // The client creates the Mediator
        var director = new FontDialogDirector();
        
        // --- Scenario 1: User selects a font from the ListBox ---
        Console.WriteLine("\n==================================================");
        Console.WriteLine("SIMULATION START: User selects 'Arial' from ListBox.");
        Console.WriteLine("==================================================");
        
        // ListBox acts: it calls Changed(), notifying the director
        director.FontList.SetSelection("Arial");
        
        // --- Scenario 2: User presses the now-enabled OK button ---
        Console.WriteLine("\n==================================================");
        Console.WriteLine("SIMULATION: User presses the OK button.");
        Console.WriteLine("==================================================");
        
        // OK Button acts: it calls Changed(), notifying the director
        director.OkButton.SimulatePress();

        // --- Scenario 3: User deletes text from the entry field manually ---
        Console.WriteLine("\n==================================================");
        Console.WriteLine("SIMULATION: User manually clears the EntryField.");
        Console.WriteLine("==================================================");
        
        // EntryField is modified, acting as if the user typed.
        // If EntryField were more complex, it would call Changed() directly.
        // Here, we simulate the state change and notification logic.
        director.FontName.SetText(""); // Simulate user typing empty string
        director.FontName.Changed(); // Manually trigger notification for demonstration
    }
}
