using System;

public sealed class Singleton
{
    // Lazy initialization ensures thread safety and lazy loading
    private static readonly Lazy<Singleton> _instance = new Lazy<Singleton>(() => new Singleton());

    // Private constructor prevents instantiation from other classes
    private Singleton()
    {
        Console.WriteLine("Singleton instance created.");
    }

    // Public static property to provide global access
    public static Singleton Instance
    {
        get
        {
            return _instance.Value;
        }
    }

    // Example method
    public void DoSomething()
    {
        Console.WriteLine("Doing something...");
    }
}

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
