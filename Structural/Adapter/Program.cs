using System;

// Simple Point struct for coordinates
public struct Point
{
    public float X { get; }
    public float Y { get; }

    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}, {Y})";
}

// Target interface (Shape)
public interface IShape
{
    void BoundingBox(out Point bottomLeft, out Point topRight);
    Manipulator CreateManipulator();
}

// Adaptee (TextView)
public class TextView
{
    // Dummy implementation for demonstration
    public void GetOrigin(out float x, out float y)
    {
        x = 10f; // Example origin X
        y = 20f; // Example origin Y
    }

    public void GetExtent(out float width, out float height)
    {
        width = 200f;  // Example width
        height = 50f;  // Example height
    }

    public bool IsEmpty()
    {
        return false; // Example: not empty
    }
}

// Abstract Manipulator class
public abstract class Manipulator
{
    // Base class for manipulators; could have methods for animation/dragging
}

// Concrete Manipulator for TextShape
public class TextManipulator : Manipulator
{
    private readonly TextShape _shape;

    public TextManipulator(TextShape shape)
    {
        _shape = shape;
    }

    // Could add dragging logic here if needed
}

// Adapter (TextShape) using object adapter pattern (composition)
public class TextShape : IShape
{
    private readonly TextView _text;

    public TextShape(TextView text)
    {
        _text = text;
    }

    public void BoundingBox(out Point bottomLeft, out Point topRight)
    {
        float left, bottom, width, height;
        _text.GetOrigin(out left, out bottom);
        _text.GetExtent(out width, out height);

        bottomLeft = new Point(left, bottom);
        topRight = new Point(left + width, bottom + height);
    }

    // Additional method forwarded from TextView (not part of IShape, but exposed by adapter)
    public bool IsEmpty()
    {
        return _text.IsEmpty();
    }

    public Manipulator CreateManipulator()
    {
        return new TextManipulator(this);
    }
}

// Client (e.g., DrawingEditor simulation)
public class DrawingEditor
{
    public void AddShape(IShape shape)
    {
        Point bottomLeft, topRight;
        shape.BoundingBox(out bottomLeft, out topRight);
        Console.WriteLine($"Added shape with bounding box: BottomLeft {bottomLeft}, TopRight {topRight}");

        var manipulator = shape.CreateManipulator();
        Console.WriteLine($"Created manipulator of type: {manipulator.GetType().Name}");
    }
}

// Example usage
class Program
{
    static void Main()
    {
        // Create the adaptee
        TextView textView = new TextView();

        // Adapt it to IShape using the adapter
        IShape textShape = new TextShape(textView);

        // Use in client (DrawingEditor)
        DrawingEditor editor = new DrawingEditor();
        editor.AddShape(textShape);

        // Demonstrate additional method (IsEmpty)
        if (textShape is TextShape ts)
        {
            Console.WriteLine($"Is the text shape empty? {ts.IsEmpty()}");
        }
    }
}
