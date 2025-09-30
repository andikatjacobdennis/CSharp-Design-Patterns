# Composite Design Pattern in C#

## Overview

This project demonstrates the **Composite Design Pattern** using a practical example of **graphical objects composed into part-whole hierarchies**.

The **Composite Pattern** is a **structural** pattern that **allows clients to treat individual objects and compositions of objects uniformly**.

In this example, we have:

*   **`DemoClient`**: The part of the system that initiates the work and uses the pattern's structure.
*   **`Graphic`**: The abstract class that defines the common interface for all components in the pattern.
*   **`Line` and `Text`**: Leaf classes that provide specific implementations of the `Graphic` interface.
*   **`CompositeGraphic`**: Composite class that can contain children and implement operations recursively.

---

## Structure

### Diagram

![UML Diagram illustrating the Composite pattern](diagram_placeholder.png)

### 1. Core Interface / Abstract Class

*   **`Graphic`**: Defines operations like `Draw()`, and optional child management methods `Add()` and `Remove()`. Provides default implementations that throw exceptions for leaf objects.

### 2. Concrete Implementations

*   **`Line`**: Draws a line and does not support children.
*   **`Text`**: Draws a text object and does not support children.
*   **`CompositeGraphic`**: Stores child `Graphic` objects, implements `Draw()` recursively, and supports `Add()` and `Remove()` operations.

### 3. Client

*   **`DemoClient`**: Creates a tree of `Graphic` objects and calls `Draw()` on the root. It treats leaves and composites uniformly.

### 4. Optional Orchestrator

*   **`CompositeGraphic`** also acts as an orchestrator by maintaining child components and forwarding operations.

---

## Example Usage

```csharp
// 1. Create leaf and composite objects
var picture = new CompositeGraphic("Root Picture");
var header = new CompositeGraphic("Header");
header.Add(new Text("Welcome to Composite Demo"));

var body = new CompositeGraphic("Body");
body.Add(new Line("Line 1"));
body.Add(new Line("Line 2"));

var group = new CompositeGraphic("Grouped Shapes");
group.Add(new Line("Diagonal"));
group.Add(new Text("Grouped Label"));
body.Add(group);

picture.Add(header);
picture.Add(body);

// 2. Client draws the full scene
picture.Draw();

// 3. Demonstrate safety: adding to leaf throws exception
try {
    var leaf = new Line("Lonely Line");
    leaf.Add(new Text("Should fail"));
} catch (NotSupportedException ex) {
    Console.WriteLine("Caught expected exception: " + ex.Message);
}

// 4. Update composite via AsComposite()
picture.AsComposite()?.Add(new Text("Footer added"));
picture.Draw();

// 5. Remove a child and redraw
picture.Remove(header);
picture.Draw();
```

### Output:

```
+ Composite: Root Picture
  + Composite: Header
    - Text: "Welcome to Composite Demo"
  + Composite: Body
    - Line: Line 1
    - Line: Line 2
    + Composite: Grouped Shapes
      - Line: Diagonal
      - Text: "Grouped Label"
Caught expected exception: Add not supported on this component.
+ Composite: Root Picture
  + Composite: Body
    - Line: Line 1
    - Line: Line 2
    + Composite: Grouped Shapes
      - Line: Diagonal
      - Text: "Grouped Label"
    - Text: "Footer added"
+ Composite: Root Picture
  + Composite: Body
    - Line: Line 1
    - Line: Line 2
    + Composite: Grouped Shapes
      - Line: Diagonal
      - Text: "Grouped Label"
    - Text: "Footer added"
```

---

## Benefits

*   **Loose Coupling**: Client is decoupled from concrete classes.
*   **Flexibility and Extensibility**: New leaf or composite classes can be added without modifying client code.
*   **Single Responsibility**: Each class has a focused purpose (Leaf or Composite).
*   **Simplified Client Code**: The client interacts with all objects uniformly.

---

## Common Use Cases

*   When you need to **treat individual objects and compositions uniformly**.
*   When you want to **represent tree structures, like UI elements or parse trees**.
*   When your design requires **recursive composition of objects**.
*   When you must **support dynamic addition/removal of components**.

---

## Implementation Notes

*   **Abstract Class vs Interface**: `Graphic` is an abstract class to provide default behavior. Interfaces could also be used if only method signatures are needed.
*   **Dynamic Modification**: `CompositeGraphic` allows runtime addition or removal of children.
*   **Safety vs Transparency**: Leaf objects throw exceptions for unsupported operations, but the pattern could allow no-op defaults for complete transparency.
