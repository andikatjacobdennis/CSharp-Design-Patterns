# **Bridge Pattern** in **C#**

## Overview

This project demonstrates the **Bridge Pattern** using a practical example of **a windowing system that can render windows and icons across different platforms (X Window System, Presentation Manager)**.

The **Bridge Pattern** is a **structural** pattern that **decouples an abstraction from its implementation so that the two can vary independently**.

In this example, we have:

*   **`Client`**: The main program (`Program`) that creates windows and icons.
*   **`Window` (Abstraction)**: Defines the interface for window operations, such as `Open()`, `Close()`, and `DrawContents()`.
*   **`ApplicationWindow` / `IconWindow` (Refined Abstraction)**: Specific types of windows that implement the `DrawContents()` behavior.
*   **`IWindowImp` (Implementor)**: The core interface for platform-specific implementations.
*   **`XWindowImp` / `PMWindowImp` (Concrete Implementors)**: Platform-specific implementations of window drawing operations.
*   **`WindowImpFactory` (Optional Orchestrator)**: Factory class to create platform-specific implementors based on runtime selection.

---

## Structure

### Diagram

![UML Diagram illustrating the pattern](diagram_placeholder.png)

### 1. Core Interface / Abstract Class

*   **`Window`**: Defines high-level operations like `Open()`, `Close()`, `DrawContents()`, and forwards drawing operations to `IWindowImp`.

### 2. Concrete Implementations

*   **`ApplicationWindow`**: Draws application-specific contents.
*   **`IconWindow`**: Draws icon-specific contents.

### 3. Implementor Interface

*   **`IWindowImp`**: Declares primitive operations for opening, closing, and drawing.
*   **`XWindowImp` / `PMWindowImp`**: Concrete implementations using platform-specific APIs (simulated with console output).

### 4. Client

*   **`Program`**: Interacts with `Window` abstractions and operates them without knowing the specific implementor.

### 5. Optional Orchestrator

*   **`WindowImpFactory`**: Creates the appropriate `IWindowImp` implementation depending on the platform.

---

## Example Usage

```csharp
// 1. Create concrete implementations
IWindowImp xImp = WindowImpFactory.Create(WindowImpFactory.Platform.X);
IWindowImp pmImp = WindowImpFactory.Create(WindowImpFactory.Platform.PM);

// 2. Create abstractions that use implementors
Window appWindow = new ApplicationWindow(xImp, "My Application");
Window iconWindow = new IconWindow(pmImp, "MyIcon.png");

// 3. Operate through the abstraction
appWindow.Open();
appWindow.DrawContents();
appWindow.Close();

iconWindow.Open();
iconWindow.DrawContents();
iconWindow.Close();

// 4. Demonstrate switching implementor at runtime
appWindow.SetImplementor(pmImp);
appWindow.Open();
appWindow.DrawContents();
appWindow.Close();
```

### Output:

```
[XWindowImp] Opening window using X Window System primitives.
[ApplicationWindow] Drawing contents of 'My Application'.
[XWindowImp] Drawing rectangle at (10,10) size 300x200 (X draw API call).
[XWindowImp] Drawing text 'My Application' at (20,30) (X text API call).
[XWindowImp] Closing window (X).

[PMWindowImp] Opening window using Presentation Manager primitives.
[IconWindow] Drawing icon 'MyIcon.png'.
[PMWindowImp] Drawing rectangle at (0,0) size 64x64 (PM draw API call).
[PMWindowImp] Drawing text 'MyIcon.png' at (8,32) (PM text API call).
[PMWindowImp] Closing window (PM).

Switching the ApplicationWindow implementor at runtime to PM implementation...
[PMWindowImp] Opening window using Presentation Manager primitives.
[ApplicationWindow] Drawing contents of 'My Application'.
[PMWindowImp] Drawing rectangle at (10,10) size 300x200 (PM draw API call).
[PMWindowImp] Drawing text 'My Application' at (20,30) (PM text API call).
[PMWindowImp] Closing window (PM).
```

---

## Benefits

*   **Loose Coupling**: The client (`Program`) is decoupled from concrete classes, depending only on abstractions.
*   **Flexibility and Extensibility**: New window types or platform implementations can be introduced without modifying client code.
*   **Single Responsibility**: Each class has a clear purpose, either abstraction or platform-specific implementation.
*   **Simplified Client Code**: Clients interact with a clean, unified interface without platform-specific details.

---

## Common Use Cases

*   When you need to **support multiple implementations of an abstraction independently**.
*   When you want to **switch implementations at runtime**.
*   When you must **decouple high-level logic from platform-specific or low-level details**.
*   When your design requires **sharing implementations among multiple abstractions**.

---

## Implementation Notes

*   **Interface vs. Abstract Class**: Use an abstract class (`Window`) to share common behavior, and an interface (`IWindowImp`) for maximum flexibility.
*   **Dynamic Switching**: `SetImplementor()` allows runtime switching of platform-specific implementations.
*   **Overhead**: Introducing the Bridge adds a layer of indirection; use it when flexibility outweighs the cost.
