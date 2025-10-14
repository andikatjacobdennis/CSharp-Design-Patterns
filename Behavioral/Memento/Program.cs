using System;
using System.Collections.Generic;

// 1. Memento (EditorState) - Stores the Originator's state
/// <summary>
/// The Memento stores a snapshot of the Originator's internal state.
/// It has a narrow interface for the Caretaker and a wide interface for the Originator.
/// </summary>
public class EditorState
{
    private readonly string _content;

    // The constructor is internal or private in a real application to restrict access 
    // only to the Originator class (Editor), ensuring the Caretaker cannot modify the state.
    public EditorState(string content)
    {
        _content = content;
    }

    // This method represents the 'wide interface' and should ideally only be accessible
    // by the Originator (Editor) via friendship or internal access modifiers in C#.
    public string GetContent()
    {
        return _content;
    }
}

// 2. Originator (Editor) - The object whose state is being saved
/// <summary>
/// The Originator creates the Memento and uses it to restore its internal state.
/// </summary>
public class Editor
{
    // The internal state to be encapsulated
    private string _content = "";

    /// <summary>
    /// Modifies the internal state.
    /// </summary>
    public void Type(string text)
    {
        _content += text;
        Console.WriteLine($"Editor current content: '{_content}'");
    }

    /// <summary>
    /// Creates a Memento (snapshot of the current state).
    /// </summary>
    /// <returns>A new EditorState object containing the current content.</returns>
    public EditorState CreateState()
    {
        // The Originator initializes the Memento with its current state.
        return new EditorState(_content); 
    }

    /// <summary>
    /// Restores the Originator's state from a given Memento.
    /// </summary>
    /// <param name="state">The memento to restore from.</param>
    public void Restore(EditorState state)
    {
        // Only the Originator can access the state inside the Memento.
        _content = state.GetContent(); 
    }

    public string GetContent()
    {
        return _content;
    }
}

// 3. Caretaker (HistoryManager) - Manages the Mementos
/// <summary>
/// The Caretaker is responsible for the Memento's safekeeping (the history stack).
/// It never operates on or examines the Memento's contents, treating it as opaque.
/// </summary>
public class HistoryManager
{
    // A Stack is ideal for simple undo functionality (LIFO - Last-In, First-Out)
    private readonly Stack<EditorState> _history = new Stack<EditorState>();

    /// <summary>
    /// Saves a new Memento onto the history stack.
    /// </summary>
    /// <param name="state">The Memento provided by the Originator.</param>
    public void Save(EditorState state)
    {
        _history.Push(state);
        Console.WriteLine("--- State Checkpoint Saved ---");
    }

    /// <summary>
    /// Retrieves the previous Memento from the history stack to enable undo.
    /// </summary>
    /// <returns>The Memento representing the state *before* the last change.</returns>
    public EditorState Undo()
    {
        if (_history.Count > 1) 
        {
            // Remove the current (latest) state
            _history.Pop(); 
            Console.WriteLine("--- Undo Performed ---");
        } else {
            Console.WriteLine("--- Cannot Undo further: At initial state ---");
        }
        
        // Return the state that is now at the top of the stack (the previous good state)
        return _history.Peek(); 
    }
}

/// <summary>
/// Client class to demonstrate the Memento pattern.
/// </summary>
public class Program
{
    public static void Main()
    {
        // The Originator (Editor) and the Caretaker (HistoryManager)
        Editor editor = new Editor();
        HistoryManager history = new HistoryManager();

        Console.WriteLine("--- Starting Demo ---");
        
        // --- Action 1: Type "Hello " and Save State ---
        editor.Type("Hello ");
        // Originator creates the state, Caretaker saves it
        history.Save(editor.CreateState());

        // --- Action 2: Type "World" and Save State ---
        editor.Type("World");
        history.Save(editor.CreateState());
        
        // --- Action 3: Type "!" (State not saved yet) ---
        editor.Type("!");

        // Current state before undo
        Console.WriteLine($"\nFinal content before undo: '{editor.GetContent()}'");

        // --- Undo 1: Revert to "Hello World" ---
        EditorState previousState1 = history.Undo();
        // The Originator restores its state using the Memento provided by the Caretaker
        editor.Restore(previousState1);
        Console.WriteLine($"Current content after first undo: '{editor.GetContent()}'\n");

        // --- Undo 2: Revert to "Hello " ---
        EditorState previousState2 = history.Undo();
        editor.Restore(previousState2);
        Console.WriteLine($"Current content after second undo: '{editor.GetContent()}'\n");

        // --- Undo 3: Try to undo past the first saved state ---
        history.Undo();
        // Since the undo count check prevents popping the last item, Peek() returns the same state
        editor.Restore(history.Undo()); 
        Console.WriteLine($"Current content after third undo attempt: '{editor.GetContent()}'");
    }
}
