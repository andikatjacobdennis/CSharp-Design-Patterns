using System;

// Prototype interface
public interface IPrototype
{
    IPrototype Clone();
}

// Concrete Prototype 1
public class ConcretePrototype1 : IPrototype
{
    public string Name { get; set; }
    public int Value { get; set; }

    public ConcretePrototype1(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public IPrototype Clone()
    {
        // Create a shallow copy (for value types and strings)
        return new ConcretePrototype1(Name, Value);
    }

    public override string ToString()
    {
        return $"ConcretePrototype1: Name={Name}, Value={Value}";
    }
}

// Concrete Prototype 2
public class ConcretePrototype2 : IPrototype
{
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }

    public ConcretePrototype2(string description, DateTime createdDate)
    {
        Description = description;
        CreatedDate = createdDate;
    }

    public IPrototype Clone()
    {
        // Create a shallow copy
        return new ConcretePrototype2(Description, CreatedDate);
    }

    public override string ToString()
    {
        return $"ConcretePrototype2: Description={Description}, CreatedDate={CreatedDate}";
    }
}

// Client class
public class Client
{
    public void Operation()
    {
        // Create original prototypes
        var prototype1 = new ConcretePrototype1("Original", 100);
        var prototype2 = new ConcretePrototype2("Test Object", DateTime.Now);

        // Clone the prototypes
        var clone1 = prototype1.Clone() as ConcretePrototype1;
        var clone2 = prototype2.Clone() as ConcretePrototype2;

        // Modify clones to show they are separate objects
        if (clone1 != null)
        {
            clone1.Name = "Cloned";
            clone1.Value = 200;
        }

        if (clone2 != null)
        {
            clone2.Description = "Modified Clone";
        }

        // Display results
        Console.WriteLine("Original Prototype 1: " + prototype1);
        Console.WriteLine("Cloned Prototype 1: " + clone1);
        Console.WriteLine();
        Console.WriteLine("Original Prototype 2: " + prototype2);
        Console.WriteLine("Cloned Prototype 2: " + clone2);
    }
}
