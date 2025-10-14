# **Visitor Pattern** in **C#**

## Overview

This project demonstrates the **Visitor Pattern** through a practical example of **PC Component Diagnostics** — where we perform different types of diagnostics (like power consumption and health checks) on computer parts **without modifying their classes**.

The **Visitor Pattern** is a **behavioral design pattern** that allows you to **add new operations to an object structure without changing its classes**.

In simpler terms:

> You can add new “visitors” (diagnostic operations) without touching the existing “components” (like `GraphicsCard` or `Motherboard`).

---

## Structure

### Diagram



---

### 1. Core Abstractions

* **`PCComponent`** — Abstract base class defining the `Accept(IDiagnosticVisitor visitor)` method. Every hardware component inherits from it.
* **`IDiagnosticVisitor`** — Declares a visit method for each component type (`VisitFloppyDrive`, `VisitGraphicsCard`, etc.).

### 2. Concrete Implementations

* **Components** — `FloppyDrive`, `GraphicsCard`, and `Motherboard` define component-specific properties (power, temperature, health).
* **Composite Component** — `SystemUnit` holds multiple components and delegates the `Accept()` call to all of them.
* **Visitors**:

  * `PowerConsumptionVisitor`: Calculates and reports total power usage.
  * `HealthCheckVisitor`: Performs temperature and health diagnostics.

### 3. Client

* **`Program`** — Builds a composite `SystemUnit` structure and applies the visitors to it, printing diagnostic reports.

---

## Example Usage

```csharp
using System;
using System.Collections.Generic;

// =============================
//  ELEMENT HIERARCHY
// =============================
abstract class PCComponent
{
    public string Name { get; }
    protected PCComponent(string name) => Name = name;
    public abstract void Accept(IDiagnosticVisitor visitor);
}

class FloppyDrive : PCComponent
{
    public int PowerUsageWatts { get; }
    public int TemperatureCelsius { get; }
    public string HealthStatus { get; }

    public FloppyDrive(string name, int watts, int temp, string health)
        : base(name)
    {
        PowerUsageWatts = watts;
        TemperatureCelsius = temp;
        HealthStatus = health;
    }

    public override void Accept(IDiagnosticVisitor visitor) => visitor.VisitFloppyDrive(this);
}

class GraphicsCard : PCComponent
{
    public int PowerUsageWatts { get; }
    public int TemperatureCelsius { get; }
    public string HealthStatus { get; }

    public GraphicsCard(string name, int watts, int temp, string health)
        : base(name)
    {
        PowerUsageWatts = watts;
        TemperatureCelsius = temp;
        HealthStatus = health;
    }

    public override void Accept(IDiagnosticVisitor visitor) => visitor.VisitGraphicsCard(this);
}

class Motherboard : PCComponent
{
    public int PowerUsageWatts { get; }
    public int TemperatureCelsius { get; }
    public string HealthStatus { get; }

    public Motherboard(string name, int watts, int temp, string health)
        : base(name)
    {
        PowerUsageWatts = watts;
        TemperatureCelsius = temp;
        HealthStatus = health;
    }

    public override void Accept(IDiagnosticVisitor visitor) => visitor.VisitMotherboard(this);
}

class SystemUnit : PCComponent
{
    private readonly List<PCComponent> _components = new();

    public SystemUnit(string name) : base(name) { }

    public void Add(PCComponent component) => _components.Add(component);

    public override void Accept(IDiagnosticVisitor visitor)
    {
        foreach (var c in _components)
            c.Accept(visitor);

        visitor.VisitSystemUnit(this);
    }
}

// =============================
//  VISITOR INTERFACE
// =============================
interface IDiagnosticVisitor
{
    void VisitFloppyDrive(FloppyDrive floppy);
    void VisitGraphicsCard(GraphicsCard gpu);
    void VisitMotherboard(Motherboard mb);
    void VisitSystemUnit(SystemUnit unit);
}

// =============================
//  CONCRETE VISITORS
// =============================
class PowerConsumptionVisitor : IDiagnosticVisitor
{
    public int TotalPower { get; private set; }

    public void VisitFloppyDrive(FloppyDrive floppy)
    {
        Console.WriteLine($"[POWER] {floppy.Name}: {floppy.PowerUsageWatts}W");
        TotalPower += floppy.PowerUsageWatts;
    }

    public void VisitGraphicsCard(GraphicsCard gpu)
    {
        Console.WriteLine($"[POWER] {gpu.Name}: {gpu.PowerUsageWatts}W");
        TotalPower += gpu.PowerUsageWatts;
    }

    public void VisitMotherboard(Motherboard mb)
    {
        Console.WriteLine($"[POWER] {mb.Name}: {mb.PowerUsageWatts}W");
        TotalPower += mb.PowerUsageWatts;
    }

    public void VisitSystemUnit(SystemUnit unit)
    {
        Console.WriteLine($"[POWER] --- Total Power Draw: {TotalPower}W ---");
    }
}

class HealthCheckVisitor : IDiagnosticVisitor
{
    public void VisitFloppyDrive(FloppyDrive floppy)
    {
        Console.WriteLine($"[HEALTH] {floppy.Name}: {floppy.HealthStatus} | Temp: {floppy.TemperatureCelsius}°C");
    }

    public void VisitGraphicsCard(GraphicsCard gpu)
    {
        Console.WriteLine($"[HEALTH] {gpu.Name}: {gpu.HealthStatus} | Temp: {gpu.TemperatureCelsius}°C");
    }

    public void VisitMotherboard(Motherboard mb)
    {
        Console.WriteLine($"[HEALTH] {mb.Name}: {mb.HealthStatus} | Temp: {mb.TemperatureCelsius}°C");
    }

    public void VisitSystemUnit(SystemUnit unit)
    {
        Console.WriteLine($"[HEALTH] --- System Diagnostics Complete ---");
    }
}

// =============================
//  CLIENT
// =============================
class Program
{
    static void Main()
    {
        Console.WriteLine("=== Visitor Pattern Demo: PC Diagnostics ===\n");

        var system = new SystemUnit("Gaming PC");
        system.Add(new FloppyDrive("Kingston USB Drive", 5, 30, "Healthy"));
        system.Add(new GraphicsCard("NVIDIA RTX 4080", 320, 72, "Stable"));
        system.Add(new Motherboard("ASUS Prime Z790", 70, 40, "Healthy"));

        var powerVisitor = new PowerConsumptionVisitor();
        var healthVisitor = new HealthCheckVisitor();

        Console.WriteLine("Running Power Consumption Diagnostic...\n");
        system.Accept(powerVisitor);

        Console.WriteLine("\n--------------------------------\n");

        Console.WriteLine("Running Health Check Diagnostic...\n");
        system.Accept(healthVisitor);

        Console.WriteLine("\n--------------------------------");
        Console.WriteLine($"TOTAL SYSTEM POWER USAGE: {powerVisitor.TotalPower}W");
        Console.WriteLine("\nDiagnostics Complete.");
    }
}
```

---

### Output

```text
=== Visitor Pattern Demo: PC Diagnostics ===

Running Power Consumption Diagnostic...

[POWER] Kingston USB Drive: 5W
[POWER] NVIDIA RTX 4080: 320W
[POWER] ASUS Prime Z790: 70W
[POWER] --- Total Power Draw: 395W ---

--------------------------------

Running Health Check Diagnostic...

[HEALTH] Kingston USB Drive: Healthy | Temp: 30°C
[HEALTH] NVIDIA RTX 4080: Stable | Temp: 72°C
[HEALTH] ASUS Prime Z790: Healthy | Temp: 40°C
[HEALTH] --- System Diagnostics Complete ---

--------------------------------
TOTAL SYSTEM POWER USAGE: 395W

Diagnostics Complete.
```

---

## Benefits

✅ **Add New Operations Easily**
You can add new diagnostics (like `ThermalEfficiencyVisitor` or `PerformanceBenchmarkVisitor`) without changing any existing component classes.

✅ **Encapsulation of Behavior**
Each visitor holds its own logic in one place — not scattered across multiple component types.

✅ **Follows the Open/Closed Principle**
The system is *open* for new operations but *closed* for modification of component classes.

---

## Common Use Cases

**Hardware / Device Monitoring Systems** — Run different diagnostic or analytics passes over a fixed set of device types.
**Compilers / AST Processing** — Apply visitors for syntax checking, code generation, or optimization.
**Game Engines / Graphics Systems** — Traverse scene graphs for rendering, physics, or lighting calculations.
**Business Applications** — Apply financial, audit, or reporting visitors over a data model.

---

Would you like me to add a **matching UML diagram (image)** and **one-sentence summary tagline** (for GitHub or video thumbnail)?
