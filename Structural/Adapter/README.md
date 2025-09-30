# **Adapter Pattern** in **C#**

## Overview

This project demonstrates the **Adapter Pattern** using a practical example of **adapting a `TextView` class to behave like a `Shape` in a drawing application**.

The **Adapter Pattern** is a **structural** pattern that **allows incompatible interfaces to work together by converting one interface into another expected by the client**.

In this example, we have:

* **`Client`**: The part of the system that uses `Shape` objects, e.g., a drawing editor.
* **`Shape` (Core Interface / Abstract Class)**: Defines the operations that all shapes must implement, such as `BoundingBox()` and `CreateManipulator()`.
* **`TextView` (Adaptee / Concrete Implementation A)**: Existing class that has an incompatible interface.
* **`TextShape` (Adapter / Concrete Implementation B)**: Converts `TextView`’s interface into the `Shape` interface.
* **`TextManipulator` (Optional Orchestrator)**: Handles interactions with shapes, such as dragging or resizing.

---

## Structure

### Diagram

![UML Diagram illustrating the Adapter Pattern](adapter_structure.png)

### 1. Core Interface / Abstract Class

* **`Shape`**: Defines the common operations that all shapes must provide:

```csharp
public abstract class Shape
{
    public abstract void BoundingBox(out int bottomLeftX, out int bottomLeftY, out int topRightX, out int topRightY);
    public abstract bool IsEmpty();
    public abstract Manipulator CreateManipulator();
}
```

### 2. Concrete Implementations

* **`TextView`**: Provides existing functionality that doesn’t match `Shape`.

```csharp
public class TextView
{
    public void GetOrigin(out int x, out int y) { x = 10; y = 20; }
    public void GetExtent(out int width, out int height) { width = 100; height = 50; }
    public bool IsEmpty() { return false; }
}
```

* **`TextShape`**: Adapts `TextView` to the `Shape` interface.

```csharp
public class TextShape : Shape
{
    private TextView _textView;

    public TextShape(TextView textView) { _textView = textView; }

    public override void BoundingBox(out int bottomLeftX, out int bottomLeftY, out int topRightX, out int topRightY)
    {
        int originX, originY, width, height;
        _textView.GetOrigin(out originX, out originY);
        _textView.GetExtent(out width, out height);

        bottomLeftX = originX;
        bottomLeftY = originY;
        topRightX = originX + width;
        topRightY = originY + height;
    }

    public override bool IsEmpty() => _textView.IsEmpty();

    public override Manipulator CreateManipulator() => new TextManipulator(this);
}
```

### 3. Client

* **`Program`**: Works with `Shape` objects without knowing about `TextView`.

### 4. Optional Orchestrator

* **`TextManipulator`**: Handles interactions for `TextShape`.

```csharp
public abstract class Manipulator
{
    public abstract void Manipulate();
}

public class TextManipulator : Manipulator
{
    private Shape _shape;

    public TextManipulator(Shape shape) { _shape = shape; }

    public override void Manipulate()
    {
        Console.WriteLine("Manipulating shape...");
    }
}
```

---

## Example Usage

```csharp
class Program
{
    static void Main(string[] args)
    {
        TextView textView = new TextView();
        Shape textShape = new TextShape(textView);

        textShape.BoundingBox(out int x1, out int y1, out int x2, out int y2);
        Console.WriteLine($"BoundingBox: ({x1},{y1}) to ({x2},{y2})");

        Console.WriteLine($"IsEmpty: {textShape.IsEmpty()}");

        Manipulator manipulator = textShape.CreateManipulator();
        manipulator.Manipulate();
    }
}
```

### Output:

```
BoundingBox: (10,20) to (110,70)
IsEmpty: False
Manipulating shape...
```

---

## Benefits

* **Loose Coupling**: The client works with `Shape` and doesn’t depend on `TextView`.
* **Flexibility and Extensibility**: You can introduce new adapters for other classes without modifying the client.
* **Single Responsibility**: Adapter focuses solely on converting interfaces.
* **Simplified Client Code**: The client can use a consistent interface (`Shape`) regardless of the underlying class.

---

## Common Use Cases

* When you need to **integrate an existing class with a new interface**.
* When you want to **reuse a class that was not designed for your domain**.
* When you must **provide a unified interface to a set of diverse classes**.
* When your design requires **adding functionality without modifying existing code**.

---

## Implementation Notes

* **Interface vs. Abstract Class**: Use abstract classes if you have shared behavior; otherwise, interfaces maximize flexibility.
* **Dynamic Switching**: Object adapters allow swapping the adaptee instance at runtime.
* **Overhead**: Introducing adapters adds a layer of abstraction; use them when benefits outweigh costs.
