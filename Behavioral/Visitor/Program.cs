using System;
using System.Collections.Generic;

// --- 1. Element Hierarchy (Visitable Objects) ---

/// <summary>
/// The Abstract Element class defining the Accept operation.
/// This allows a visitor to 'visit' the element.
/// </summary>
abstract class Equipment
{
    public string Name { get; }
    protected Equipment(string name) => Name = name;
    
    /// <summary>
    /// The core method that delegates the call to the appropriate Visit method 
    /// on the concrete visitor (Double Dispatch).
    /// </summary>
    public abstract void Accept(EquipmentVisitor visitor);
    
    /// <summary>
    /// A base property of the element hierarchy (e.g., base cost).
    /// </summary>
    public abstract decimal NetPrice();
}

/// <summary>
/// Concrete Element representing a simple component.
/// </summary>
class FloppyDisk : Equipment
{
    public FloppyDisk(string name, decimal price) : base(name) { Price = price; }
    public decimal Price { get; }
    public override decimal NetPrice() => Price;
    
    /// <summary>
    /// Implements Accept, directing the visitor to use the specific VisitFloppyDisk method.
    /// </summary>
    public override void Accept(EquipmentVisitor visitor) => visitor.VisitFloppyDisk(this);
}

/// <summary>
/// Concrete Element representing another simple component.
/// </summary>
class Card : Equipment
{
    public Card(string name, decimal price) : base(name) { Price = price; }
    public decimal Price { get; }
    public override decimal NetPrice() => Price;
    
    /// <summary>
    /// Implements Accept, directing the visitor to use the specific VisitCard method.
    /// </summary>
    public override void Accept(EquipmentVisitor visitor) => visitor.VisitCard(this);
}

/// <summary>
/// Concrete Element that also acts as a Composite (contains other equipment).
/// </summary>
class Chassis : Equipment
{
    private readonly List<Equipment> _parts = new();
    
    public Chassis(string name) : base(name) { }
    
    public void Add(Equipment part) => _parts.Add(part);
    
    /// <summary>
    /// Calculates the cumulative price of its parts and applies a 10% discount.
    /// </summary>
    public override decimal NetPrice()
    {
        decimal total = 0;
        foreach (var p in _parts) total += p.NetPrice();
        return total * 0.9m;
    }
    
    /// <summary>
    /// Implements Accept. Crucially, it traverses the structure (its parts) 
    /// before accepting the visit for itself.
    /// </summary>
    public override void Accept(EquipmentVisitor visitor)
    {
        // 1. Recursively accept the visitor for all child parts
        foreach (var p in _parts)
            p.Accept(visitor);
        
        // 2. Accept the visit for the composite element itself
        visitor.VisitChassis(this);
    }
}

// --- 2. Visitor Hierarchy (The Operations) ---

/// <summary>
/// The Abstract Visitor class declaring a Visit method for every concrete element.
/// </summary>
abstract class EquipmentVisitor
{
    // Default implementations are empty, allowing concrete visitors to ignore elements if necessary.
    public virtual void VisitFloppyDisk(FloppyDisk floppy) { }
    public virtual void VisitCard(Card card) { }
    public virtual void VisitChassis(Chassis chassis) { }
}

/// <summary>
/// Concrete Visitor implementing the pricing operation.
/// It maintains state (TotalPrice) specific to this operation.
/// </summary>
class PricingVisitor : EquipmentVisitor
{
    public decimal TotalPrice { get; private set; }

    // Adds the individual component's price
    public override void VisitFloppyDisk(FloppyDisk floppy) => TotalPrice += floppy.NetPrice();
    
    // Adds the individual component's price
    public override void VisitCard(Card card) => TotalPrice += card.NetPrice();
    
    // Adds the discounted total price of the composite element
    public override void VisitChassis(Chassis chassis) => TotalPrice += chassis.NetPrice();
}

/// <summary>
/// Concrete Visitor implementing the inventory counting operation.
/// It maintains state (TotalItems) specific to this operation.
/// </summary>
class InventoryVisitor : EquipmentVisitor
{
    public int TotalItems { get; private set; }
    
    // Increments count for every discrete FloppyDisk found
    public override void VisitFloppyDisk(FloppyDisk floppy) => TotalItems++;
    
    // Increments count for every discrete Card found
    public override void VisitCard(Card card) => TotalItems++;
    
    // Increments count for the Chassis itself
    public override void VisitChassis(Chassis chassis) => TotalItems++;
}

// --- 3. Client ---

/// <summary>
/// Client code that builds the object structure and initiates the Visitor operations.
/// </summary>
class Program
{
    static void Main()
    {
        Console.WriteLine("--- Visitor Pattern Demonstration: Equipment Inventory ---");

        // 1. Build the object structure (The Equipment Composite)
        var chassis = new Chassis("PC Tower Chassis");
        chassis.Add(new FloppyDisk("3.5in Floppy Drive", 10m));
        chassis.Add(new Card("Gigabit Ethernet Card", 25m));
        // Note: The chassis NetPrice is (10 + 25) * 0.9 = 31.5m

        Console.WriteLine($"\nEquipment Structure created: '{chassis.Name}' with 2 parts.");
        Console.WriteLine($"Base price of Floppy: $10.0");
        Console.WriteLine($"Base price of Card: $25.0");
        Console.WriteLine($"Chassis discounted price: $31.5 (35 * 0.9)");

        // 2. Create Visitors
        var pricingVisitor = new PricingVisitor();
        var inventoryVisitor = new InventoryVisitor();

        // 3. Apply Visitors to the structure (start the traversal)
        Console.WriteLine("\nApplying PricingVisitor and InventoryVisitor...");
        chassis.Accept(pricingVisitor);
        chassis.Accept(inventoryVisitor);

        // 4. Output Results
        Console.WriteLine("\n--- Results ---");
        
        // Pricing calculation: 10 (Floppy) + 25 (Card) + 31.5 (Chassis NetPrice) = 66.5
        Console.WriteLine($"Total Price (Accumulated Costs): ${pricingVisitor.TotalPrice}");
        
        // Inventory count: 1 (Floppy) + 1 (Card) + 1 (Chassis) = 3
        Console.WriteLine($"Total Items (Inventory Count): {inventoryVisitor.TotalItems}");
        
        Console.WriteLine("\nNote: New operations (e.g., DiagnosticsVisitor) can be added easily without changing Equipment classes.");
    }
}
