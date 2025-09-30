using ShoppingCart.Behavioral;
using ShoppingCart.Creational;
using ShoppingCart.Structural;
using System.Text;

namespace ShoppingCart
{
    class Program
    {
        static void Main(string[] args)
        {                
            // Set the console's output encoding to UTF-8
            Console.OutputEncoding = Encoding.UTF8;

            // Main loop
            while (true)
            {
                Console.Clear();
                DisplayMenu();
                
                var choice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                
                switch (choice)
                {
                    case '1': CreationalPatternsDemo.Run(); break;
                    case '2': StructuralPatternsDemo.Run(); break;
                    case '3': BehavioralPatternsDemo.Run(); break;
                    case '0': return;
                    default: Console.WriteLine("Invalid choice!"); break;
                }
                
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        
        static void DisplayMenu()
        {
            Console.WriteLine("🛒 SHOPPING CART DESIGN PATTERNS DEMO 🛒");
            Console.WriteLine("=========================================");
            Console.WriteLine("1. 🏗️  Creational Patterns");
            Console.WriteLine("2. 🏛️  Structural Patterns");
            Console.WriteLine("3. 🔄 Behavioral Patterns");
            Console.WriteLine("0. ❌ Exit");
            Console.WriteLine("=========================================");
            Console.Write("Choose an option: ");
        }
    }
}