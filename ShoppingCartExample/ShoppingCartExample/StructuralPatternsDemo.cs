using System;
using System.Collections.Generic;

namespace ShoppingCart.Structural
{
    public static class StructuralPatternsDemo
    {
        public static void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("🏛️ STRUCTURAL PATTERNS DEMO");
                Console.WriteLine("===========================");
                Console.WriteLine("1. Adapter - Legacy Payment");
                Console.WriteLine("2. Bridge - Storage Types");
                Console.WriteLine("3. Composite - Product Bundles");
                Console.WriteLine("4. Decorator - Product Features");
                Console.WriteLine("5. Facade - Checkout System");
                Console.WriteLine("6. Flyweight - Product Data");
                Console.WriteLine("7. Proxy - Lazy Loading");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Choose pattern: ");
                
                var choice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                
                switch (choice)
                {
                    case '1': AdapterDemo(); break;
                    case '2': BridgeDemo(); break;
                    case '3': CompositeDemo(); break;
                    case '4': DecoratorDemo(); break;
                    case '5': FacadeDemo(); break;
                    case '6': FlyweightDemo(); break;
                    case '7': ProxyDemo(); break;
                    case '0': return;
                    default: Console.WriteLine("Invalid choice!"); break;
                }
                
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        
        static void AdapterDemo()
        {
            Console.WriteLine("\n🔌 ADAPTER - Legacy Payment System");
            var modernCart = new ModernShoppingCart();
            
            // Using legacy payment system via adapter
            var legacyPayment = new LegacyPaymentSystem();
            var adapter = new PaymentAdapter(legacyPayment);
            
            modernCart.ProcessPayment(adapter, 150.00m);
        }
        
        static void BridgeDemo()
        {
            Console.WriteLine("\n🌉 BRIDGE - Storage Types");
            ICartStorage dbStorage = new DatabaseStorage();
            ICartStorage sessionStorage = new SessionStorage();
            
            var dbCart = new ShoppingCart(dbStorage);
            var sessionCart = new ShoppingCart(sessionStorage);
            
            dbCart.AddItem("Laptop");
            sessionCart.AddItem("Mouse");
            
            dbCart.Save();
            sessionCart.Save();
        }
        
        static void CompositeDemo()
        {
            Console.WriteLine("\n🎁 COMPOSITE - Product Bundles");
            var singleProduct = new SingleProduct("Laptop", 999.99m);
            var bundle = new ProductBundle("Office Suite");
            
            bundle.Add(new SingleProduct("Word Processor", 199.99m));
            bundle.Add(new SingleProduct("Spreadsheet", 149.99m));
            bundle.Add(new SingleProduct("Presentation", 129.99m));
            
            var cart = new CompositeCart();
            cart.AddItem(singleProduct);
            cart.AddItem(bundle);
            
            cart.Display();
        }
        
        static void DecoratorDemo()
        {
            Console.WriteLine("\n🎀 DECORATOR - Product Features");
            IProduct basicProduct = new ConcreteProduct("Smartphone", 699.99m);
            
            // Add features dynamically
            IProduct giftWrapped = new GiftWrapDecorator(basicProduct);
            IProduct withInsurance = new InsuranceDecorator(giftWrapped);
            IProduct expedited = new ExpeditedShippingDecorator(withInsurance);
            
            Console.WriteLine("Basic Product:");
            basicProduct.Display();
            
            Console.WriteLine("\nWith All Features:");
            expedited.Display();
        }
        
        static void FacadeDemo()
        {
            Console.WriteLine("\n🏢 FACADE - Checkout System");
            var checkoutFacade = new CheckoutFacade();
            checkoutFacade.CompleteCheckout("user123", 250.00m);
        }
        
        static void FlyweightDemo()
        {
            Console.WriteLine("\n🔄 FLYWEIGHT - Product Data Sharing");
            var factory = new ProductFlyweightFactory();
            
            var product1 = factory.GetFlyweight("Laptop", "High-performance gaming laptop", "Electronics");
            var product2 = factory.GetFlyweight("Laptop", "High-performance gaming laptop", "Electronics");
            
            Console.WriteLine($"Same flyweight instance: {product1 == product2}");
            product1.Display("Gaming Laptop Pro", 1299.99m);
            product2.Display("Gaming Laptop Lite", 899.99m);
        }
        
        static void ProxyDemo()
        {
            Console.WriteLine("\n🛡️ PROXY - Lazy Loading Images");
            IProductImage highResImage = new ProductImageProxy("product_high_res.jpg");
            
            Console.WriteLine("Image not loaded yet...");
            Console.WriteLine("Displaying image (will load now):");
            highResImage.Display();
        }
    }

    // Adapter Implementation
    public interface IPaymentProcessor
    {
        void ProcessPayment(decimal amount);
    }

    public class ModernPaymentSystem : IPaymentProcessor
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing modern payment: ${amount}");
        }
    }

    // Legacy system with incompatible interface
    public class LegacyPaymentSystem
    {
        public void MakePayment(string currency, double amount)
        {
            Console.WriteLine($"Legacy payment: {currency} {amount}");
        }
    }

    public class PaymentAdapter : IPaymentProcessor
    {
        private LegacyPaymentSystem legacySystem;
        
        public PaymentAdapter(LegacyPaymentSystem legacySystem)
        {
            this.legacySystem = legacySystem;
        }
        
        public void ProcessPayment(decimal amount)
        {
            // Convert to legacy system's expected format
            legacySystem.MakePayment("USD", (double)amount);
        }
    }

    public class ModernShoppingCart
    {
        public void ProcessPayment(IPaymentProcessor processor, decimal amount)
        {
            processor.ProcessPayment(amount);
        }
    }

    // Bridge Implementation
    public interface ICartStorage
    {
        void SaveCart(List<string> items);
    }

    public class DatabaseStorage : ICartStorage
    {
        public void SaveCart(List<string> items)
        {
            Console.WriteLine($"Saving {items.Count} items to database");
        }
    }

    public class SessionStorage : ICartStorage
    {
        public void SaveCart(List<string> items)
        {
            Console.WriteLine($"Saving {items.Count} items to session");
        }
    }

    public abstract class CartAbstraction
    {
        protected ICartStorage storage;
        
        public CartAbstraction(ICartStorage storage)
        {
            this.storage = storage;
        }
        
        public abstract void AddItem(string item);
        public abstract void Save();
    }

    public class ShoppingCart : CartAbstraction
    {
        private List<string> items = new List<string>();
        
        public ShoppingCart(ICartStorage storage) : base(storage) { }
        
        public override void AddItem(string item)
        {
            items.Add(item);
            Console.WriteLine($"Added {item} to cart");
        }
        
        public override void Save()
        {
            storage.SaveCart(items);
        }
    }

    // Composite Implementation
    public interface ICartComponent
    {
        void Display();
        decimal GetPrice();
    }

    public class SingleProduct : ICartComponent
    {
        private string name;
        private decimal price;
        
        public SingleProduct(string name, decimal price)
        {
            this.name = name;
            this.price = price;
        }
        
        public void Display()
        {
            Console.WriteLine($"  Product: {name} - ${price}");
        }
        
        public decimal GetPrice() => price;
    }

    public class ProductBundle : ICartComponent
    {
        private string name;
        private List<ICartComponent> components = new List<ICartComponent>();
        
        public ProductBundle(string name)
        {
            this.name = name;
        }
        
        public void Add(ICartComponent component)
        {
            components.Add(component);
        }
        
        public void Display()
        {
            Console.WriteLine($"Bundle: {name}");
            foreach (var component in components)
            {
                component.Display();
            }
            Console.WriteLine($"Bundle Total: ${GetPrice()}");
        }
        
        public decimal GetPrice()
        {
            decimal total = 0;
            foreach (var component in components)
            {
                total += component.GetPrice();
            }
            return total * 0.9m; // 10% discount for bundles
        }
    }

    public class CompositeCart
    {
        private List<ICartComponent> items = new List<ICartComponent>();
        
        public void AddItem(ICartComponent item)
        {
            items.Add(item);
        }
        
        public void Display()
        {
            Console.WriteLine("🛒 COMPOSITE CART:");
            decimal total = 0;
            foreach (var item in items)
            {
                item.Display();
                total += item.GetPrice();
            }
            Console.WriteLine($"💵 GRAND TOTAL: ${total}");
        }
    }

    // Decorator Implementation
    public interface IProduct
    {
        string GetDescription();
        decimal GetPrice();
        void Display();
    }

    public class ConcreteProduct : IProduct
    {
        private string name;
        private decimal price;
        
        public ConcreteProduct(string name, decimal price)
        {
            this.name = name;
            this.price = price;
        }
        
        public string GetDescription() => name;
        public decimal GetPrice() => price;
        
        public void Display()
        {
            Console.WriteLine($"{name} - ${price}");
        }
    }

    public abstract class ProductDecorator : IProduct
    {
        protected IProduct product;
        
        public ProductDecorator(IProduct product)
        {
            this.product = product;
        }
        
        public virtual string GetDescription() => product.GetDescription();
        public virtual decimal GetPrice() => product.GetPrice();
        public virtual void Display() => product.Display();
    }

    public class GiftWrapDecorator : ProductDecorator
    {
        public GiftWrapDecorator(IProduct product) : base(product) { }
        
        public override string GetDescription() => $"{product.GetDescription()} + Gift Wrap";
        public override decimal GetPrice() => product.GetPrice() + 5.99m;
        
        public override void Display()
        {
            Console.WriteLine($"{GetDescription()} - ${GetPrice()}");
        }
    }

    public class InsuranceDecorator : ProductDecorator
    {
        public InsuranceDecorator(IProduct product) : base(product) { }
        
        public override string GetDescription() => $"{product.GetDescription()} + Insurance";
        public override decimal GetPrice() => product.GetPrice() + 9.99m;
        
        public override void Display()
        {
            Console.WriteLine($"{GetDescription()} - ${GetPrice()}");
        }
    }

    public class ExpeditedShippingDecorator : ProductDecorator
    {
        public ExpeditedShippingDecorator(IProduct product) : base(product) { }
        
        public override string GetDescription() => $"{product.GetDescription()} + Expedited Shipping";
        public override decimal GetPrice() => product.GetPrice() + 14.99m;
        
        public override void Display()
        {
            Console.WriteLine($"{GetDescription()} - ${GetPrice()}");
        }
    }

    // Facade Implementation
    public class CheckoutFacade
    {
        private PaymentSystem paymentSystem = new PaymentSystem();
        private InventorySystem inventorySystem = new InventorySystem();
        private ShippingSystem shippingSystem = new ShippingSystem();
        
        public void CompleteCheckout(string userId, decimal amount)
        {
            Console.WriteLine("🚀 STARTING CHECKOUT PROCESS...");
            
            // Process payment
            if (paymentSystem.ProcessPayment(userId, amount))
            {
                // Update inventory
                inventorySystem.UpdateInventory(userId);
                
                // Arrange shipping
                shippingSystem.ScheduleDelivery(userId);
                
                Console.WriteLine("✅ CHECKOUT COMPLETED SUCCESSFULLY!");
            }
            else
            {
                Console.WriteLine("❌ PAYMENT FAILED!");
            }
        }
    }

    public class PaymentSystem
    {
        public bool ProcessPayment(string userId, decimal amount)
        {
            Console.WriteLine($"💳 Processing payment of ${amount} for {userId}");
            return true;
        }
    }

    public class InventorySystem
    {
        public void UpdateInventory(string userId)
        {
            Console.WriteLine($"📦 Updating inventory for {userId}");
        }
    }

    public class ShippingSystem
    {
        public void ScheduleDelivery(string userId)
        {
            Console.WriteLine($"🚚 Scheduling delivery for {userId}");
        }
    }

    // Flyweight Implementation
    public class ProductFlyweight
    {
        public string Description { get; }
        public string Category { get; }
        
        public ProductFlyweight(string description, string category)
        {
            Description = description;
            Category = category;
        }
        
        public void Display(string name, decimal price)
        {
            Console.WriteLine($"🏷️ {name} - ${price}");
            Console.WriteLine($"   Description: {Description}");
            Console.WriteLine($"   Category: {Category}");
        }
    }

    public class ProductFlyweightFactory
    {
        private Dictionary<string, ProductFlyweight> flyweights = new Dictionary<string, ProductFlyweight>();
        
        public ProductFlyweight GetFlyweight(string key, string description, string category)
        {
            if (!flyweights.ContainsKey(key))
            {
                flyweights[key] = new ProductFlyweight(description, category);
                Console.WriteLine($"Created new flyweight for: {key}");
            }
            else
            {
                Console.WriteLine($"Reusing existing flyweight for: {key}");
            }
            
            return flyweights[key];
        }
    }

    // Proxy Implementation
    public interface IProductImage
    {
        void Display();
    }

    public class HighResProductImage : IProductImage
    {
        private string filename;
        
        public HighResProductImage(string filename)
        {
            this.filename = filename;
            LoadImageFromDisk();
        }
        
        private void LoadImageFromDisk()
        {
            Console.WriteLine($"🖼️ Loading high-resolution image: {filename}");
        }
        
        public void Display()
        {
            Console.WriteLine($"Displaying high-resolution image: {filename}");
        }
    }

    public class ProductImageProxy : IProductImage
    {
        private string filename;
        private HighResProductImage realImage;
        
        public ProductImageProxy(string filename)
        {
            this.filename = filename;
        }
        
        public void Display()
        {
            realImage ??= new HighResProductImage(filename);
            realImage.Display();
        }
    }
}