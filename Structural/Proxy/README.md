# **Proxy Pattern** in **C#**

## Overview

This project demonstrates the **Proxy Pattern** using a practical example of **loading and rendering images in a document editor**.

The **Proxy Pattern** is a **structural** pattern that **provides a surrogate or placeholder for another object to control access to it**.

In this example, we have:

* **`Client`**: The part of the system that uses graphical objects in a document.
* **`Graphic`**: The common interface that all graphical objects implement.
* **`Image`**: The real object that loads and displays images.
* **`ImageProxy`**: The proxy object that controls access to the real image and loads it on demand.
* **`TextDocument`**: A container for `Graphic` objects that acts as the orchestrator.

---

## Structure

### Diagram

![UML Diagram illustrating the Proxy pattern](diagram_placeholder.png)

### 1. Core Interface / Abstract Class

* **`Graphic`**: Defines the common operations `Draw(Point at)` and `GetExtent()`.

### 2. Concrete Implementations

* **`Image`**: Loads and displays the image file; performs the actual rendering.
* **`ImageProxy`**: Maintains a reference to the image file and only instantiates `Image` when necessary; caches image extent.

### 3. Client

* **`TextDocument`**: Uses `Graphic` objects through the `Graphic` interface without knowing whether it’s a real image or a proxy.

### 4. Optional Orchestrator

* **`TextDocument`**: Manages a collection of `Graphic` objects and orchestrates rendering and layout.

---

## Example Usage

```csharp
using System;

namespace ProxyPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Create proxy objects
            Graphic image1 = new ImageProxy("picture1.jpg");
            Graphic image2 = new ImageProxy("picture2.jpg");

            // 2. Create a document to hold graphics
            TextDocument document = new TextDocument();
            document.Insert(image1);
            document.Insert(image2);

            // 3. Use the client to render images
            document.Render();
        }
    }
}
```

### Expected Output:

```
Drawing image picture1.jpg at 0, 0
Drawing image picture2.jpg at 0, 0
```

> Note: Actual image loading happens only when `Draw` is called, not when the proxy is created.

---

## Benefits

* **Lazy Loading**: Expensive objects are created only when needed.
* **Encapsulation of Access**: Client code does not need to manage object instantiation or access rules.
* **Reduced Resource Usage**: Prevents loading large objects until necessary.
* **Transparent Substitution**: Client interacts with `Graphic` objects uniformly, whether they are real or proxies.

---

## Common Use Cases

* Loading large images, videos, or other resources on demand.
* Providing controlled access to sensitive objects (protection proxy).
* Representing objects in remote systems (remote proxy).
* Implementing copy-on-write optimizations for large objects.

---

## Implementation Notes

* Use **abstract classes or interfaces** to define the common operations for real and proxy objects.
* Proxies can **cache metadata** to respond to queries without fully loading the real object.
* Introducing proxies adds **a level of indirection**, so weigh performance and complexity benefits carefully.
