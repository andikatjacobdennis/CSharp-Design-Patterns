using System;
using System.Collections.Generic;

// ============================================================================
//  VISITOR PATTERN: PC COMPONENT DIAGNOSTICS SYSTEM (Real-World Example)
//  Author: Ajdcode
//  Description: Demonstrates how to run new diagnostics (operations)
//               without modifying existing PC component classes.
// ============================================================================

// ------------------------------
// 1. ELEMENT HIERARCHY
// ------------------------------

/// <summary>
/// The abstract base class for all PC components.
/// Each component must implement Accept() to allow a visitor to "visit" it.
/// </summary>
abstract class PCComponent
{
    public string Name { get; }
    protected PCComponent(string name) => Name = name;

    // The key method enabling the visitor pattern (Double Dispatch)
    public abstract void Accept(IDiagnosticVisitor visitor);
}

/// <summary>
/// A simple PC component — represents a Floppy Drive.
/// </summary>
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

/// <summary>
/// Represents a Graphics Card.
/// </summary>
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

/// <summary>
/// Represents a Motherboard (System Board).
/// </summary>
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

/// <summary>
/// Composite component — represents the overall PC system.
/// Contains multiple PC components.
/// </summary>
class SystemUnit : PCComponent
{
    private readonly List<PCComponent> _components = new();

    public SystemUnit(string name) : base(name) { }

    public void Add(PCComponent component) => _components.Add(component);

    public override void Accept(IDiagnosticVisitor visitor)
    {
        foreach (var component in _components)
            component.Accept(visitor);

        visitor.VisitSystemUnit(this);
    }
}

// ------------------------------
// 2. VISITOR INTERFACE
// ------------------------------

/// <summary>
/// Visitor interface defining one Visit() method for each component type.
/// </summary>
interface IDiagnosticVisitor
{
    void VisitFloppyDrive(FloppyDrive floppy);
    void VisitGraphicsCard(GraphicsCard gpu);
    void VisitMotherboard(Motherboard mb);
    void VisitSystemUnit(SystemUnit unit);
}

// ------------------------------
// 3. CONCRETE VISITORS
// ------------------------------

/// <summary>
/// Concrete Visitor that calculates total power usage across all components.
/// </summary>
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

/// <summary>
/// Concrete Visitor that performs health and temperature diagnostics.
/// </summary>
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

// ------------------------------
// 4. CLIENT
// ------------------------------

/// <summary>
/// The client builds the PC structure and applies diagnostics (visitors).
/// </summary>
class Program
{
    static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== Visitor Pattern Demo: PC Diagnostics System ===\n");
        Console.ResetColor();

        // Create the composite PC structure
        var systemUnit = new SystemUnit("Gaming PC System");
        systemUnit.Add(new FloppyDrive("Kingston USB Floppy", 5, 30, "Healthy"));
        systemUnit.Add(new GraphicsCard("NVIDIA RTX 4080", 320, 72, "Stable"));
        systemUnit.Add(new Motherboard("ASUS Prime Z790", 70, 40, "Healthy"));

        // Create visitors
        var powerVisitor = new PowerConsumptionVisitor();
        var healthVisitor = new HealthCheckVisitor();

        // Apply visitors (diagnostics)
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Running Power Consumption Diagnostic...\n");
        Console.ResetColor();
        systemUnit.Accept(powerVisitor);

        Console.WriteLine("\n---------------------------------------------\n");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Running Health Check Diagnostic...\n");
        Console.ResetColor();
        systemUnit.Accept(healthVisitor);

        // Final summary
        Console.WriteLine("\n---------------------------------------------");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"TOTAL SYSTEM POWER USAGE: {powerVisitor.TotalPower}W");
        Console.ResetColor();

        Console.WriteLine("\nDiagnostics Complete.");
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
