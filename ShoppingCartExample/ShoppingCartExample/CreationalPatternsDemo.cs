using System;
using System.Collections.Generic;

namespace ShoppingCart.Creational
{
    public static class CreationalPatternsDemo
    {
        public static void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("🏗️ CREATIONAL PATTERNS DEMO");
                Console.WriteLine("==========================");
                Console.WriteLine("1. Abstract Factory - Regional Shopping");
                Console.WriteLine("2. Builder - Complex Orders");
                Console.WriteLine("3. Factory Method - Product Types");
                Console.WriteLine("4. Prototype - Product Templates");
                Console.WriteLine("5. Singleton - Cart Manager");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Choose pattern: ");
                
                var choice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                
                switch (choice)
                {
                    case '1': AbstractFactoryDemo(); break;
                    case '2': BuilderDemo(); break;
                    case '3': FactoryMethodDemo(); break;
                    case '4': PrototypeDemo(); break;
                    case '5': SingletonDemo(); break;
                    case '0': return;
                    default: Console.WriteLine("Invalid choice!"); break;
                }
                
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        
        static void AbstractFactoryDemo()
        {
            Console.WriteLine("\n🎯 ABSTRACT FACTORY - Regional Shopping");
            Console.WriteLine("Creating US shopping experience:");
            var usFactory = new USShoppingFactory();
            var usCart = usFactory.CreateCart();
            var usProduct = usFactory.CreateProduct("Laptop", 999.99m);
            usCart.AddItem(usProduct);
            usCart.Display();
            
            Console.WriteLine("\nCreating EU shopping experience:");
            var euFactory = new EUShoppingFactory();
            var euCart = euFactory.CreateCart();
            var euProduct = euFactory.CreateProduct("Laptop", 999.99m);
            euCart.AddItem(euProduct);
            euCart.Display();
        }
        
        static void BuilderDemo()
        {
            Console.WriteLine("\n🔨 BUILDER - Complex Orders");
            var order = new OrderBuilder()
                .AddItem(new Product("Laptop", 999.99m), 1)
                .AddItem(new Product("Mouse", 29.99m), 2)
                .WithGiftWrapping()
                .WithInsurance()
                .WithExpeditedShipping()
                .Build();
            
            order.DisplayOrder();
        }
        
        static void FactoryMethodDemo()
        {
            Console.WriteLine("\n🏭 FACTORY METHOD - Product Types");
            ProductFactory factory = new DigitalProductFactory();
            var digitalProduct = factory.CreateProduct("E-book", 19.99m);
            digitalProduct.Display();
            
            factory = new PhysicalProductFactory();
            var physicalProduct = factory.CreateProduct("Book", 29.99m);
            physicalProduct.Display();
        }
        
        static void PrototypeDemo()
        {
            //Console.WriteLine("\n📋 PROTOTYPE - Product Templates");
            //var templateProduct = new Product("T-Shirt Template", 19.99m) 
            //{ 
            //    Category = "Clothing",
            //    Description = "Basic t-shirt template"
            //};
            
            //var redShirt = templateProduct.Clone();
            //redShirt.Name = "Red T-Shirt";
            //redShirt.Price = 24.99m;
            
            //var blueShirt = templateProduct.Clone();
            //blueShirt.Name = "Blue T-Shirt";
            //blueShirt.Price = 26.99m;
            
            //Console.WriteLine("Original Template:");
            //templateProduct.Display();
            //Console.WriteLine("\nCloned Products:");
            //redShirt.Display();
            //blueShirt.Display();
        }
        
        static void SingletonDemo()
        {
            Console.WriteLine("\n👑 SINGLETON - Cart Manager");
            var manager1 = ShoppingCartManager.Instance;
            var manager2 = ShoppingCartManager.Instance;
            
            Console.WriteLine($"Manager 1 ID: {manager1.GetHashCode()}");
            Console.WriteLine($"Manager 2 ID: {manager2.GetHashCode()}");
            Console.WriteLine($"Same instance: {manager1 == manager2}");
            
            manager1.CreateCart("User123");
            manager2.CreateCart("User456");
            manager1.DisplayAllCarts();
        }
    }

    // Abstract Factory Implementation
    public interface IShoppingFactory
    {
        IShoppingCart CreateCart();
        IProduct CreateProduct(string name, decimal price);
    }

    public class USShoppingFactory : IShoppingFactory
    {
        public IShoppingCart CreateCart() => new USShoppingCart();
        public IProduct CreateProduct(string name, decimal price) => new Product(name, price);
    }

    public class EUShoppingFactory : IShoppingFactory
    {
        public IShoppingCart CreateCart() => new EUShoppingCart();
        public IProduct CreateProduct(string name, decimal price) => new Product(name, price);
    }

    public interface IShoppingCart
    {
        void AddItem(IProduct product);
        void Display();
    }

    public class USShoppingCart : IShoppingCart
    {
        private List<IProduct> items = new List<IProduct>();
        
        public void AddItem(IProduct product)
        {
            items.Add(product);
            Console.WriteLine($"Added {product.Name} to US cart");
        }
        
        public void Display()
        {
            Console.WriteLine("US Shopping Cart - Prices in USD, US tax rules");
            foreach (var item in items)
                item.Display();
        }
    }

    public class EUShoppingCart : IShoppingCart
    {
        private List<IProduct> items = new List<IProduct>();
        
        public void AddItem(IProduct product)
        {
            items.Add(product);
            Console.WriteLine($"Added {product.Name} to EU cart");
        }
        
        public void Display()
        {
            Console.WriteLine("EU Shopping Cart - Prices in EUR, VAT tax rules");
            foreach (var item in items)
                item.Display();
        }
    }

    // Builder Implementation
    public class Order
    {
        public List<OrderItem> Items { get; } = new List<OrderItem>();
        public bool HasGiftWrapping { get; set; }
        public bool HasInsurance { get; set; }
        public bool HasExpeditedShipping { get; set; }
        
        public void DisplayOrder()
        {
            Console.WriteLine("🛍️ ORDER DETAILS:");
            decimal total = 0;
            foreach (var item in Items)
            {
                decimal itemTotal = item.Product.Price * item.Quantity;
                Console.WriteLine($"  {item.Product.Name} x{item.Quantity}: ${itemTotal}");
                total += itemTotal;
            }
            
            if (HasGiftWrapping) { total += 5.99m; Console.WriteLine("  Gift Wrapping: $5.99"); }
            if (HasInsurance) { total += 9.99m; Console.WriteLine("  Insurance: $9.99"); }
            if (HasExpeditedShipping) { total += 14.99m; Console.WriteLine("  Expedited Shipping: $14.99"); }
            
            Console.WriteLine($"💵 TOTAL: ${total}");
        }
    }

    public class OrderItem
    {
        public IProduct Product { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderBuilder
    {
        private Order order = new Order();
        
        public OrderBuilder AddItem(IProduct product, int quantity)
        {
            order.Items.Add(new OrderItem { Product = product, Quantity = quantity });
            Console.WriteLine($"Added {product.Name} x{quantity}");
            return this;
        }
        
        public OrderBuilder WithGiftWrapping()
        {
            order.HasGiftWrapping = true;
            Console.WriteLine("Added gift wrapping");
            return this;
        }
        
        public OrderBuilder WithInsurance()
        {
            order.HasInsurance = true;
            Console.WriteLine("Added insurance");
            return this;
        }
        
        public OrderBuilder WithExpeditedShipping()
        {
            order.HasExpeditedShipping = true;
            Console.WriteLine("Added expedited shipping");
            return this;
        }
        
        public Order Build() => order;
    }

    // Factory Method Implementation
    public abstract class ProductFactory
    {
        public abstract IProduct CreateProduct(string name, decimal price);
    }

    public class DigitalProductFactory : ProductFactory
    {
        public override IProduct CreateProduct(string name, decimal price)
            => new DigitalProduct(name, price);
    }

    public class PhysicalProductFactory : ProductFactory
    {
        public override IProduct CreateProduct(string name, decimal price)
            => new PhysicalProduct(name, price);
    }

    public class DigitalProduct : Product
    {
        public DigitalProduct(string name, decimal price) : base(name, price) { }
        
        public override void Display()
        {
            Console.WriteLine($"💾 DIGITAL: {Name} - ${Price} (Instant download)");
        }
    }

    public class PhysicalProduct : Product
    {
        public PhysicalProduct(string name, decimal price) : base(name, price) { }
        
        public override void Display()
        {
            Console.WriteLine($"📦 PHYSICAL: {Name} - ${Price} (Ships in 2-3 days)");
        }
    }

    // Prototype Implementation
    public interface IProduct : ICloneable
    {
        string Name { get; set; }
        decimal Price { get; set; }
        string Category { get; set; }
        string Description { get; set; }
        void Display();
    }

    public class Product : IProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        
        public virtual object Clone()
        {
            return new Product(Name, Price)
            {
                Category = Category,
                Description = Description
            };
        }
        
        public virtual void Display()
        {
            Console.WriteLine($"🏷️ {Name} - ${Price}");
        }
    }

    // Singleton Implementation
    public sealed class ShoppingCartManager
    {
        private static ShoppingCartManager instance;
        private static readonly object lockObject = new object();
        private Dictionary<string, IShoppingCart> userCarts = new Dictionary<string, IShoppingCart>();
        
        private ShoppingCartManager() { }
        
        public static ShoppingCartManager Instance
        {
            get
            {
                lock (lockObject)
                {
                    return instance ??= new ShoppingCartManager();
                }
            }
        }
        
        public void CreateCart(string userId)
        {
            if (!userCarts.ContainsKey(userId))
            {
                userCarts[userId] = new USShoppingCart();
                Console.WriteLine($"Created cart for user: {userId}");
            }
        }
        
        public void DisplayAllCarts()
        {
            Console.WriteLine($"Managing {userCarts.Count} user carts");
        }
    }
}