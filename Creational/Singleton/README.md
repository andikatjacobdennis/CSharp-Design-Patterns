# Singleton Design Pattern in C\#

## Overview

This project demonstrates the **Singleton Design Pattern** using a simple C# example.

The Singleton pattern ensures that a class has only **one instance** and provides a **global point of access** to it. It is useful when exactly one object is needed to coordinate actions across the system.

In this example:

* **Singleton** → The class that ensures a single instance.
* **Instance** → Provides global access to the Singleton object.
* **Client** → Uses the Singleton instance to perform operations.

---

## Structure

### Diagram

```
+----------------+
|    Singleton   |
+----------------+
| - instance     |
| - Singleton()  |
+----------------+
| + Instance     |
| + DoSomething()|
+----------------+
        ^
        |
      Client
```

### 1. Singleton Class

* `Singleton` → Ensures only one instance is created, and provides a global access point (`Instance`).

### 2. Client

* `Program` → Demonstrates usage of the singleton instance and ensures all references point to the same object.

---

## Example Usage

```csharp
using System;

class Program
{
    static void Main()
    {
        // Access the singleton instance
        Singleton s1 = Singleton.Instance;
        s1.DoSomething();

        Singleton s2 = Singleton.Instance;
        s2.DoSomething();

        // Check if both references point to the same instance
        Console.WriteLine(ReferenceEquals(s1, s2));  // Output: True
    }
}
```

### Output:

```
Singleton instance created.
Doing something...
Doing something...
True
```

---

## Benefits of Singleton

* **Single Instance** → Guarantees only one object exists.
* **Global Access** → Provides a central point of access to the object.
* **Lazy Initialization** → Instance is created only when needed, saving resources.
* **Thread-Safe** → Ensures consistent behavior in multi-threaded applications (with `Lazy<T>`).

---

## Use Cases

* Database connections.
* Logging mechanisms.
* Configuration management.
* Caching shared resources.
