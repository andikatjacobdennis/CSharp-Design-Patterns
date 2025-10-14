using System;

// --- 1. Strategy Interface (ICompositor) ---
/// <summary>
/// The Strategy interface declares an operation common to all supported algorithms.
/// Context uses this interface to call the algorithm defined by a ConcreteStrategy.
/// </summary>
public interface ICompositor
{
    /// <summary>
    /// Executes the line-breaking algorithm.
    /// </summary>
    /// <param name="text">The text content to be composed (line-broken).</param>
    void Compose(string text);
}

// --- 2. Concrete Strategies (Line-breaking Algorithms) ---

/// <summary>
/// Concrete Strategy 1: Implements a simple line-breaking strategy,
/// typically breaking one line at a time without global optimization.
/// </summary>
public class SimpleCompositor : ICompositor
{
    public void Compose(string text)
    {
        // In a real implementation, this would contain logic to determine breaks
        // based on simple, greedy rules.
        Console.WriteLine($"[SimpleCompositor]: Applying a single-line greedy strategy to the text.");
        // Mock result: Showing the first 30 characters as the first line
        Console.WriteLine($"    -> Line 1: \"{text.Substring(0, Math.Min(30, text.Length))}...\"");
    }
}

/// <summary>
/// Concrete Strategy 2: Implements a complex, global line-breaking strategy,
/// such as the TeX algorithm, optimizing breaks over an entire paragraph.
/// </summary>
public class TeXCompositor : ICompositor
{
    public void Compose(string text)
    {
        // In a real implementation, this would use dynamic programming
        // to find the globally optimal set of line breaks.
        Console.WriteLine($"[TeXCompositor]: Applying global optimization to minimize whitespace and hyphenation.");
        Console.WriteLine($"    -> Output paragraph has ideal 'color' and even spacing.");
    }
}

/// <summary>
/// Concrete Strategy 3: Implements a fixed-interval line-breaking strategy,
/// useful for breaking non-text items (like icons) into fixed rows.
/// </summary>
public class ArrayCompositor : ICompositor
{
    private readonly int _interval;

    public ArrayCompositor(int interval)
    {
        _interval = interval;
    }

    public void Compose(string text)
    {
        // This strategy largely ignores the content size and breaks based on a fixed count.
        Console.WriteLine($"[ArrayCompositor]: Breaking components into lines of a fixed size ({_interval} components/row).");
        Console.WriteLine($"    -> Suitable for laying out grids or icon arrays.");
    }
}


// --- 3. Context (Composition) ---
/// <summary>
/// The Context maintains a reference to one of the Strategy objects and
/// communicates with this object only through the Strategy interface.
/// </summary>
public class Composition
{
    // The reference to the Strategy (Compositor)
    private ICompositor _compositor;
    private readonly string _documentText;

    /// <summary>
    /// Initializes the Context, configuring it with an initial ConcreteStrategy.
    /// </summary>
    /// <param name="initialCompositor">The chosen line-breaking algorithm.</param>
    /// <param name="text">The content the composition holds.</param>
    public Composition(ICompositor initialCompositor, string text)
    {
        this._compositor = initialCompositor;
        this._documentText = text;
        Console.WriteLine($"\n*** Composition created with initial Strategy: {initialCompositor.GetType().Name} ***");
    }

    /// <summary>
    /// Allows the client to change the strategy at run-time.
    /// </summary>
    public void SetCompositor(ICompositor newCompositor)
    {
        this._compositor = newCompositor;
        Console.WriteLine($"\n*** Composition strategy changed to: {newCompositor.GetType().Name} ***");
    }

    /// <summary>
    /// The Context operation that delegates the work to the Strategy object.
    /// This method is independent of which concrete strategy is currently configured.
    /// </summary>
    public void Repair()
    {
        Console.WriteLine("\n--- Composition.Repair() called ---");
        // The Context forwards the request to its Strategy
        this._compositor.Compose(this._documentText);
        Console.WriteLine("--- Composition repair finished ---");
    }
}


// --- Client (Program) ---
public class Program
{
    public static void Main(string[] args)
    {
        // Define the content to be line-broken
        string longText = "Many algorithms exist for breaking a stream of text into lines. " +
                          "The Strategy pattern allows us to define and interchange these algorithms dynamically " +
                          "without modifying the client (Composition).";

        // 1. Initialize Context with Simple Strategy
        ICompositor simpleCompositor = new SimpleCompositor();
        Composition textDocument = new Composition(simpleCompositor, longText);

        // Client uses the Context, unaware of the specific algorithm used
        textDocument.Repair();


        // 2. Switch Strategy at Run-time to TeX Compositor
        ICompositor texCompositor = new TeXCompositor();
        textDocument.SetCompositor(texCompositor);

        // Client calls Repair again, and a different algorithm is executed
        textDocument.Repair();


        // 3. Switch Strategy to Array Compositor (with a specific interval parameter)
        ICompositor arrayCompositor = new ArrayCompositor(15);
        textDocument.SetCompositor(arrayCompositor);

        // The final algorithm is executed
        textDocument.Repair();
    }
}
