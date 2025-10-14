using System;
using System.Collections.Generic;

// The Iterator Design Pattern separates the concern of traversing a collection
// from the collection (Aggregate) itself.

// --- 1. The Item (Element in the Aggregate) ---
/// <summary>
/// Represents the data object stored in the collection.
/// </summary>
public class Employee
{
    public string Name { get; set; }
    public string Title { get; set; }

    public Employee(string name, string title)
    {
        Name = name;
        Title = title;
    }

    public void Print()
    {
        Console.WriteLine($"\t- {Name} ({Title})");
    }
}

// --- 2. The Abstract Iterator ---
/// <summary>
/// Defines the interface for accessing and traversing elements.
/// </summary>
/// <typeparam name="T">The type of elements to traverse.</typeparam>
public interface IIterator<T>
{
    /// <summary>Initializes the current element to the first element.</summary>
    void First();
    
    /// <summary>Returns the current element in the list.</summary>
    T CurrentItem();
    
    /// <summary>Advances the current element to the next element.</summary>
    void Next();
    
    /// <summary>Tests whether we've advanced beyond the last element.</summary>
    bool IsDone();
}

// --- 3. The Abstract Aggregate ---
/// <summary>
/// Defines the interface for creating an Iterator object (Factory Method).
/// </summary>
public interface IEmployeeCollection
{
    /// <summary>Creates and returns a concrete iterator instance.</summary>
    IIterator<Employee> CreateIterator();
    
    // Interface for basic collection access needed by the concrete iterator
    int Count { get; }
    Employee Get(int index);
}

// --- 4. The Concrete Aggregate ---
/// <summary>
/// Implements the Aggregate interface and holds the actual collection.
/// </summary>
public class EmployeeList : IEmployeeCollection
{
    // The underlying representation (internal structure)
    private readonly List<Employee> _employees = new List<Employee>();

    public EmployeeList()
    {
        // Initialize with some data
        _employees.Add(new Employee("Alice Johnson", "CEO"));
        _employees.Add(new Employee("Bob Smith", "CTO"));
        _employees.Add(new Employee("Charlie Brown", "Developer"));
    }

    public void Add(Employee employee)
    {
        _employees.Add(employee);
    }

    /// <summary>
    /// Factory Method: Returns a new iterator instance associated with this collection.
    /// </summary>
    public IIterator<Employee> CreateIterator()
    {
        // Clients use this method to get an iterator without knowing the concrete class.
        return new EmployeeIterator(this);
    }

    // Access methods exposed to the iterator (not necessarily to the client directly)
    public int Count => _employees.Count;
    public Employee Get(int index) => _employees[index];
}

// --- 5. The Concrete Iterator (External/Active Iterator) ---
/// <summary>
/// Implements the traversal algorithm for the EmployeeList.
/// </summary>
public class EmployeeIterator : IIterator<Employee>
{
    private readonly EmployeeList _list;
    // Keeps track of the current position in the traversal
    private int _current = 0;

    public EmployeeIterator(EmployeeList list)
    {
        // The iterator is coupled to the concrete aggregate (EmployeeList)
        // to efficiently access its elements.
        _list = list;
    }

    /// <summary>
    /// Positions the iterator to the start of the list.
    /// </summary>
    public void First()
    {
        _current = 0;
    }

    /// <summary>
    /// Returns the element at the current position.
    /// </summary>
    public Employee CurrentItem()
    {
        if (IsDone())
        {
            // Throw an exception if accessing outside bounds
            throw new InvalidOperationException("Iterator is out of bounds.");
        }
        return _list.Get(_current);
    }

    /// <summary>
    /// Moves the cursor to the next element.
    /// </summary>
    public void Next()
    {
        _current++;
    }

    /// <summary>
    /// Checks if the traversal is complete.
    /// </summary>
    public bool IsDone()
    {
        return _current >= _list.Count;
    }
}

// --- 6. The Client (Program) ---
public class Program
{
    /// <summary>
    /// Generic method that can print any collection as long as it provides an IIterator.
    /// The client is decoupled from the concrete EmployeeList structure.
    /// </summary>
    public static void PrintEmployees(IIterator<Employee> iterator)
    {
        Console.WriteLine("\nStarting Traversal:");
        for (iterator.First(); !iterator.IsDone(); iterator.Next())
        {
            iterator.CurrentItem().Print();
        }
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("--- Iterator Design Pattern Demo (C#) ---");

        // 1. Create the Concrete Aggregate
        EmployeeList employees = new EmployeeList();
        
        // Add a new employee later, the traversal still works without changing client code
        employees.Add(new Employee("David Lee", "Manager"));

        // 2. Request an iterator from the Aggregate via the Factory Method
        // The client only deals with the abstract IIterator interface.
        IIterator<Employee> iterator = employees.CreateIterator();
        
        // 3. Use the iterator to traverse the collection
        PrintEmployees(iterator);

        Console.WriteLine("\nTraversal Complete. The client never knew the collection was a List<Employee>.");
    }
}
