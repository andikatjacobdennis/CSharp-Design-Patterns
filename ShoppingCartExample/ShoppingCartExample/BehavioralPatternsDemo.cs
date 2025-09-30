using System;
using System.Collections.Generic;

namespace ShoppingCart.Behavioral
{
    public static class BehavioralPatternsDemo
    {
        public static void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("🔄 BEHAVIORAL PATTERNS DEMO");
                Console.WriteLine("===========================");
                Console.WriteLine("1. Chain of Responsibility - Discounts");
                Console.WriteLine("2. Command - Cart Operations");
                Console.WriteLine("3. Interpreter - Promo Codes");
                Console.WriteLine("4. Iterator - Cart Items");
                Console.WriteLine("5. Mediator - Cart Communication");
                Console.WriteLine("6. Memento - Cart State");
                Console.WriteLine("7. Observer - Price Changes");
                Console.WriteLine("8. State - Cart Status");
                Console.WriteLine("9. Strategy - Pricing");
                Console.WriteLine("A. Template Method - Checkout");
                Console.WriteLine("B. Visitor - Cart Calculations");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Choose pattern: ");
                
                var choice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                
                switch (choice)
                {
                    case '1': ChainOfResponsibilityDemo(); break;
                    case '2': CommandDemo(); break;
                    case '3': InterpreterDemo(); break;
                    case '4': IteratorDemo(); break;
                    case '5': MediatorDemo(); break;
                    case '6': MementoDemo(); break;
                    case '7': ObserverDemo(); break;
                    case '8': StateDemo(); break;
                    case '9': StrategyDemo(); break;
                    case 'A': TemplateMethodDemo(); break;
                    case 'B': VisitorDemo(); break;
                    case '0': return;
                    default: Console.WriteLine("Invalid choice!"); break;
                }
                
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        
        static void ChainOfResponsibilityDemo()
        {
            Console.WriteLine("\n⛓️ CHAIN OF RESPONSIBILITY - Discount Validation");
            var discountChain = new StudentDiscountHandler();
            discountChain.SetNext(new SeasonalDiscountHandler())
                        .SetNext(new LoyaltyDiscountHandler());
            
            var user = new User { IsStudent = true, LoyaltyPoints = 150 };
            var cart = new Cart { TotalAmount = 200.00m };
            var request = new DiscountRequest(user, cart, "SUMMER2024");
            
            discountChain.Handle(request);
            Console.WriteLine($"Final price after discounts: ${cart.TotalAmount}");
        }
        
        static void CommandDemo()
        {
            Console.WriteLine("\n🎮 COMMAND - Cart Operations");
            var cart = new CommandCart();
            var invoker = new CartInvoker();
            
            // Execute commands
            invoker.Execute(new AddItemCommand(cart, "Laptop", 999.99m));
            invoker.Execute(new AddItemCommand(cart, "Mouse", 29.99m));
            invoker.Execute(new UpdateQuantityCommand(cart, "Laptop", 2));
            
            // Undo last operation
            invoker.Undo();
            invoker.ShowHistory();
        }
        
        static void InterpreterDemo()
        {
            Console.WriteLine("\n🔤 INTERPRETER - Promo Codes");
            var context = new PromoContext { CartTotal = 150.00m };
            
            // Test different promo codes
            var promos = new[] { "SAVE10", "FREESHIP50", "BUY2GET1", "INVALID" };
            
            foreach (var promo in promos)
            {
                var expression = PromoExpression.Parse(promo);
                expression.Interpret(context);
                Console.WriteLine($"Promo '{promo}': Total = ${context.CartTotal}");
                context.CartTotal = 150.00m; // Reset for next test
            }
        }
        
        static void IteratorDemo()
        {
            Console.WriteLine("\n➡️ ITERATOR - Cart Items");
            var collection = new ProductCollection();
            collection.AddProduct(new Product("Laptop", 999.99m));
            collection.AddProduct(new Product("Mouse", 29.99m));
            collection.AddProduct(new Product("Keyboard", 79.99m));
            
            var iterator = collection.CreateIterator();
            Console.WriteLine("Cart Items:");
            while (iterator.HasNext())
            {
                var product = iterator.Next();
                product.Display();
            }
        }
        
        static void MediatorDemo()
        {
            Console.WriteLine("\n📞 MEDIATOR - Cart Communication");
            var mediator = new CartMediator();
            var cart = new ShoppingCart(mediator);
            var inventory = new Inventory(mediator);
            var pricing = new Pricing(mediator);
            
            mediator.Cart = cart;
            mediator.Inventory = inventory;
            mediator.Pricing = pricing;
            
            // Add item - mediator coordinates all components
            cart.AddItem("Laptop");
        }
        
        static void MementoDemo()
        {
            //Console.WriteLine("\n💾 MEMENTO - Cart State");
            //var cart = new MementoCaretaker();
            //var originator = new CartOriginator();
            
            //// Save states
            //originator.State = "Empty";
            //cart.SaveState(originator.CreateMemento());
            
            //originator.State = "Active: Laptop, Mouse";
            //cart.SaveState(originator.CreateMemento());
            
            //originator.State = "Checkout: Processing payment";
            //cart.SaveState(originator.CreateMemento());
            
            //// Restore previous state
            //originator.RestoreMemento(cart.Undo());
            //Console.WriteLine($"Restored state: {originator.State}");
        }
        
        static void ObserverDemo()
        {
            Console.WriteLine("\n👀 OBSERVER - Price Changes");
            var product = new ObservableProduct("Laptop", 999.99m);
            var user1 = new UserObserver("John");
            var user2 = new UserObserver("Sarah");
            
            product.Attach(user1);
            product.Attach(user2);
            
            // Price change notifies all observers
            product.Price = 899.99m;
            
            product.Detach(user1);
            product.Price = 849.99m;
        }
        
        static void StateDemo()
        {
            Console.WriteLine("\n🎭 STATE - Cart Status");
            var cart = new StateShoppingCart();
            
            cart.AddItem("Laptop"); // Empty -> Active
            cart.Checkout();        // Active -> Checkout
            cart.ProcessPayment();  // Checkout -> Completed
        }
        
        static void StrategyDemo()
        {
            Console.WriteLine("\n🎯 STRATEGY - Pricing");
            var context = new PricingContext();
            var product = new Product("Laptop", 999.99m);
            
            // Regular pricing
            context.SetStrategy(new RegularPricing());
            Console.WriteLine($"Regular price: ${context.CalculatePrice(product)}");
            
            // Member pricing
            context.SetStrategy(new MemberPricing());
            Console.WriteLine($"Member price: ${context.CalculatePrice(product)}");
            
            // Bulk pricing
            context.SetStrategy(new BulkPricing());
            Console.WriteLine($"Bulk price (10 units): ${context.CalculatePrice(product, 10)}");
        }
        
        static void TemplateMethodDemo()
        {
            Console.WriteLine("\n📋 TEMPLATE METHOD - Checkout Process");
            CheckoutProcess usCheckout = new USCheckout();
            CheckoutProcess euCheckout = new EUCheckout();
            
            Console.WriteLine("US Checkout:");
            usCheckout.ProcessOrder(200.00m);
            
            Console.WriteLine("\nEU Checkout:");
            euCheckout.ProcessOrder(200.00m);
        }
        
        static void VisitorDemo()
        {
            Console.WriteLine("\n👤 VISITOR - Cart Calculations");
            var cart = new VisitorCart();
            cart.AddItem(new Product("Laptop", 999.99m));
            cart.AddItem(new Product("Mouse", 29.99m));
            
            var priceVisitor = new PriceCalculationVisitor();
            var taxVisitor = new TaxCalculationVisitor();
            var inventoryVisitor = new InventoryCheckVisitor();
            
            Console.WriteLine("Price Calculation:");
            cart.Accept(priceVisitor);
            
            Console.WriteLine("\nTax Calculation:");
            cart.Accept(taxVisitor);
            
            Console.WriteLine("\nInventory Check:");
            cart.Accept(inventoryVisitor);
        }
    }

    // Chain of Responsibility Implementation
    public class DiscountRequest
    {
        public User User { get; }
        public Cart Cart { get; }
        public string PromoCode { get; }
        
        public DiscountRequest(User user, Cart cart, string promoCode)
        {
            User = user;
            Cart = cart;
            PromoCode = promoCode;
        }
    }

    public abstract class DiscountHandler
    {
        protected DiscountHandler next;
        
        public DiscountHandler SetNext(DiscountHandler handler)
        {
            next = handler;
            return handler;
        }
        
        public virtual void Handle(DiscountRequest request)
        {
            next?.Handle(request);
        }
    }

    public class StudentDiscountHandler : DiscountHandler
    {
        public override void Handle(DiscountRequest request)
        {
            if (request.User.IsStudent)
            {
                request.Cart.TotalAmount *= 0.9m; // 10% student discount
                Console.WriteLine("Applied 10% student discount");
            }
            base.Handle(request);
        }
    }

    public class SeasonalDiscountHandler : DiscountHandler
    {
        public override void Handle(DiscountRequest request)
        {
            if (request.PromoCode == "SUMMER2024")
            {
                request.Cart.TotalAmount *= 0.95m; // 5% seasonal discount
                Console.WriteLine("Applied 5% seasonal discount");
            }
            base.Handle(request);
        }
    }

    public class LoyaltyDiscountHandler : DiscountHandler
    {
        public override void Handle(DiscountRequest request)
        {
            if (request.User.LoyaltyPoints > 100)
            {
                request.Cart.TotalAmount *= 0.98m; // 2% loyalty discount
                Console.WriteLine("Applied 2% loyalty discount");
            }
            base.Handle(request);
        }
    }

    // Command Implementation
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public class AddItemCommand : ICommand
    {
        private CommandCart cart;
        private string productName;
        private decimal price;
        
        public AddItemCommand(CommandCart cart, string productName, decimal price)
        {
            this.cart = cart;
            this.productName = productName;
            this.price = price;
        }
        
        public void Execute()
        {
            cart.AddItem(productName, price);
        }
        
        public void Undo()
        {
            cart.RemoveItem(productName);
        }
    }

    public class UpdateQuantityCommand : ICommand
    {
        private CommandCart cart;
        private string productName;
        private int oldQuantity;
        private int newQuantity;
        
        public UpdateQuantityCommand(CommandCart cart, string productName, int quantity)
        {
            this.cart = cart;
            this.productName = productName;
            this.newQuantity = quantity;
        }
        
        public void Execute()
        {
            oldQuantity = cart.GetQuantity(productName);
            cart.UpdateQuantity(productName, newQuantity);
        }
        
        public void Undo()
        {
            cart.UpdateQuantity(productName, oldQuantity);
        }
    }

    public class CartInvoker
    {
        private Stack<ICommand> history = new Stack<ICommand>();
        
        public void Execute(ICommand command)
        {
            command.Execute();
            history.Push(command);
        }
        
        public void Undo()
        {
            if (history.Count > 0)
            {
                var command = history.Pop();
                command.Undo();
                Console.WriteLine("Undo completed");
            }
        }
        
        public void ShowHistory()
        {
            Console.WriteLine($"Command history: {history.Count} operations");
        }
    }

    // Interpreter Implementation
    public class PromoContext
    {
        public decimal CartTotal { get; set; }
    }

    public interface IPromoExpression
    {
        void Interpret(PromoContext context);
    }

    public class Save10Expression : IPromoExpression
    {
        public void Interpret(PromoContext context)
        {
            context.CartTotal *= 0.9m;
            Console.WriteLine("Applied 10% discount");
        }
    }

    public class FreeShip50Expression : IPromoExpression
    {
        public void Interpret(PromoContext context)
        {
            if (context.CartTotal >= 50)
            {
                context.CartTotal -= 10; // Free shipping worth $10
                Console.WriteLine("Applied free shipping");
            }
        }
    }

    public class PromoExpression
    {
        public static IPromoExpression Parse(string promoCode)
        {
            return promoCode.ToUpper() switch
            {
                "SAVE10" => new Save10Expression(),
                "FREESHIP50" => new FreeShip50Expression(),
                _ => new InvalidExpression()
            };
        }
    }

    public class InvalidExpression : IPromoExpression
    {
        public void Interpret(PromoContext context)
        {
            Console.WriteLine("Invalid promo code");
        }
    }

    // Iterator Implementation
    public interface IIterator
    {
        bool HasNext();
        Product Next();
    }

    public interface ICollection
    {
        IIterator CreateIterator();
    }

    public class ProductCollection : ICollection
    {
        private List<Product> products = new List<Product>();
        
        public void AddProduct(Product product)
        {
            products.Add(product);
        }
        
        public IIterator CreateIterator()
        {
            return new ProductIterator(products);
        }
    }

    public class ProductIterator : IIterator
    {
        private List<Product> products;
        private int position = 0;
        
        public ProductIterator(List<Product> products)
        {
            this.products = products;
        }
        
        public bool HasNext()
        {
            return position < products.Count;
        }
        
        public Product Next()
        {
            return products[position++];
        }
    }

    // Mediator Implementation
    public interface ICartMediator
    {
        void Notify(object sender, string eventType);
    }

    public class CartMediator : ICartMediator
    {
        public ShoppingCart Cart { get; set; }
        public Inventory Inventory { get; set; }
        public Pricing Pricing { get; set; }
        
        public void Notify(object sender, string eventType)
        {
            if (eventType == "ItemAdded" && sender is ShoppingCart cart)
            {
                Inventory.CheckStock(cart.GetLastItem());
                Pricing.CalculatePrice(cart.GetLastItem());
            }
        }
    }

    public class ShoppingCart
    {
        private ICartMediator mediator;
        private List<string> items = new List<string>();
        
        public ShoppingCart(ICartMediator mediator)
        {
            this.mediator = mediator;
        }
        
        public void AddItem(string item)
        {
            items.Add(item);
            Console.WriteLine($"Added {item} to cart");
            mediator.Notify(this, "ItemAdded");
        }
        
        public string GetLastItem() => items.Count > 0 ? items[^1] : null;
    }

    // Memento Implementation
    public class CartMemento
    {
        public string State { get; }
        
        public CartMemento(string state)
        {
            State = state;
        }
    }

    public class CartOriginator
    {
        public string State { get; set; }
        
        public CartMemento CreateMemento()
        {
            Console.WriteLine($"Saving state: {State}");
            return new CartMemento(State);
        }
        
        public void RestoreMemento(CartMemento memento)
        {
            State = memento.State;
            Console.WriteLine($"Restored state: {State}");
        }
    }

    public class MementoCaretaker
    {
        private Stack<CartMemento> history = new Stack<CartMemento>();
        
        public void SaveState(CartOriginator originator)
        {
            history.Push(originator.CreateMemento());
        }
        
        public CartMemento Undo()
        {
            return history.Count > 0 ? history.Pop() : null;
        }
    }

    // Observer Implementation
    public interface IObserver
    {
        void Update(string message);
    }

    public interface IObservable
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    public class ObservableProduct : IObservable
    {
        private string name;
        private decimal price;
        private List<IObserver> observers = new List<IObserver>();
        
        public ObservableProduct(string name, decimal price)
        {
            this.name = name;
            this.price = price;
        }
        
        public decimal Price
        {
            get => price;
            set
            {
                if (price != value)
                {
                    price = value;
                    Console.WriteLine($"Price changed to ${price}");
                    Notify();
                }
            }
        }
        
        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }
        
        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }
        
        public void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update($"Price of {name} changed to ${price}");
            }
        }
    }

    public class UserObserver : IObserver
    {
        private string name;
        
        public UserObserver(string name)
        {
            this.name = name;
        }
        
        public void Update(string message)
        {
            Console.WriteLine($"{name} notified: {message}");
        }
    }

    // State Implementation
    public interface ICartState
    {
        void AddItem(StateShoppingCart cart, string item);
        void Checkout(StateShoppingCart cart);
        void ProcessPayment(StateShoppingCart cart);
    }

    public class EmptyState : ICartState
    {
        public void AddItem(StateShoppingCart cart, string item)
        {
            Console.WriteLine($"Adding {item} - cart is now active");
            cart.SetState(new ActiveState());
        }
        
        public void Checkout(StateShoppingCart cart)
        {
            Console.WriteLine("Cannot checkout empty cart");
        }
        
        public void ProcessPayment(StateShoppingCart cart)
        {
            Console.WriteLine("No payment to process");
        }
    }

    public class ActiveState : ICartState
    {
        public void AddItem(StateShoppingCart cart, string item)
        {
            Console.WriteLine($"Added {item} to active cart");
        }
        
        public void Checkout(StateShoppingCart cart)
        {
            Console.WriteLine("Proceeding to checkout");
            cart.SetState(new CheckoutState());
        }
        
        public void ProcessPayment(StateShoppingCart cart)
        {
            Console.WriteLine("Must checkout first");
        }
    }

    public class CheckoutState : ICartState
    {
        public void AddItem(StateShoppingCart cart, string item)
        {
            Console.WriteLine("Cannot add items during checkout");
        }
        
        public void Checkout(StateShoppingCart cart)
        {
            Console.WriteLine("Already in checkout");
        }
        
        public void ProcessPayment(StateShoppingCart cart)
        {
            Console.WriteLine("Payment processed - order completed");
            cart.SetState(new CompletedState());
        }
    }

    public class CompletedState : ICartState
    {
        public void AddItem(StateShoppingCart cart, string item)
        {
            Console.WriteLine("Order completed - start new cart");
        }
        
        public void Checkout(StateShoppingCart cart)
        {
            Console.WriteLine("Order already completed");
        }
        
        public void ProcessPayment(StateShoppingCart cart)
        {
            Console.WriteLine("Payment already processed");
        }
    }

    public class StateShoppingCart
    {
        private ICartState currentState;
        
        public StateShoppingCart()
        {
            currentState = new EmptyState();
        }
        
        public void SetState(ICartState state)
        {
            currentState = state;
        }
        
        public void AddItem(string item) => currentState.AddItem(this, item);
        public void Checkout() => currentState.Checkout(this);
        public void ProcessPayment() => currentState.ProcessPayment(this);
    }

    // Strategy Implementation
    public interface IPricingStrategy
    {
        decimal CalculatePrice(Product product, int quantity = 1);
    }

    public class RegularPricing : IPricingStrategy
    {
        public decimal CalculatePrice(Product product, int quantity = 1)
        {
            return product.Price * quantity;
        }
    }

    public class MemberPricing : IPricingStrategy
    {
        public decimal CalculatePrice(Product product, int quantity = 1)
        {
            return product.Price * quantity * 0.9m; // 10% member discount
        }
    }

    public class BulkPricing : IPricingStrategy
    {
        public decimal CalculatePrice(Product product, int quantity = 1)
        {
            var basePrice = product.Price * quantity;
            return quantity >= 10 ? basePrice * 0.8m : basePrice; // 20% bulk discount
        }
    }

    public class PricingContext
    {
        private IPricingStrategy strategy;
        
        public void SetStrategy(IPricingStrategy strategy)
        {
            this.strategy = strategy;
        }
        
        public decimal CalculatePrice(Product product, int quantity = 1)
        {
            return strategy?.CalculatePrice(product, quantity) ?? product.Price * quantity;
        }
    }

    // Template Method Implementation
    public abstract class CheckoutProcess
    {
        public void ProcessOrder(decimal amount)
        {
            CalculateTax(amount);
            CalculateShipping(amount);
            ProcessPayment(amount);
            SendConfirmation();
        }
        
        protected abstract void CalculateTax(decimal amount);
        protected abstract void CalculateShipping(decimal amount);
        
        protected virtual void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing payment: ${amount}");
        }
        
        protected virtual void SendConfirmation()
        {
            Console.WriteLine("Order confirmation sent");
        }
    }

    public class USCheckout : CheckoutProcess
    {
        protected override void CalculateTax(decimal amount)
        {
            var tax = amount * 0.08m; // 8% US tax
            Console.WriteLine($"US Tax: ${tax}");
        }
        
        protected override void CalculateShipping(decimal amount)
        {
            var shipping = amount > 100 ? 0 : 9.99m;
            Console.WriteLine($"US Shipping: ${shipping}");
        }
    }

    public class EUCheckout : CheckoutProcess
    {
        protected override void CalculateTax(decimal amount)
        {
            var tax = amount * 0.2m; // 20% VAT
            Console.WriteLine($"EU VAT: ${tax}");
        }
        
        protected override void CalculateShipping(decimal amount)
        {
            var shipping = amount > 50 ? 0 : 14.99m;
            Console.WriteLine($"EU Shipping: ${shipping}");
        }
    }

    // Visitor Implementation
    public interface ICartVisitor
    {
        void Visit(Product product);
    }

    public interface ICartElement
    {
        void Accept(ICartVisitor visitor);
    }

    public class PriceCalculationVisitor : ICartVisitor
    {
        private decimal total = 0;
        
        public void Visit(Product product)
        {
            total += product.Price;
            Console.WriteLine($"  {product.Name}: ${product.Price}");
        }
        
        public void DisplayTotal()
        {
            Console.WriteLine($"Total Price: ${total}");
        }
    }

    public class TaxCalculationVisitor : ICartVisitor
    {
        private decimal totalTax = 0;
        
        public void Visit(Product product)
        {
            var tax = product.Price * 0.1m; // 10% tax
            totalTax += tax;
            Console.WriteLine($"  {product.Name} tax: ${tax}");
        }
        
        public void DisplayTotalTax()
        {
            Console.WriteLine($"Total Tax: ${totalTax}");
        }
    }

    public class InventoryCheckVisitor : ICartVisitor
    {
        public void Visit(Product product)
        {
            Console.WriteLine($"  Checking inventory for {product.Name}: In stock");
        }
    }

    public class VisitorCart
    {
        private List<ICartElement> elements = new List<ICartElement>();
        
        public void AddItem(Product product)
        {
            elements.Add(product);
        }
        
        public void Accept(ICartVisitor visitor)
        {
            foreach (var element in elements)
            {
                element.Accept(visitor);
            }
            
            // Display totals for specific visitors
            if (visitor is PriceCalculationVisitor priceVisitor)
            {
                priceVisitor.DisplayTotal();
            }
            else if (visitor is TaxCalculationVisitor taxVisitor)
            {
                taxVisitor.DisplayTotalTax();
            }
        }
    }

    // Common classes used across patterns
    public class User
    {
        public string Name { get; set; }
        public bool IsStudent { get; set; }
        public int LoyaltyPoints { get; set; }
    }

    public class Cart
    {
        public decimal TotalAmount { get; set; }
    }

    public class Product : ICartElement
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        
        public void Display()
        {
            Console.WriteLine($"{Name} - ${Price}");
        }
        
        public void Accept(ICartVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class CommandCart
    {
        private Dictionary<string, (decimal Price, int Quantity)> items = new Dictionary<string, (decimal, int)>();
        
        public void AddItem(string productName, decimal price)
        {
            if (items.ContainsKey(productName))
            {
                var item = items[productName];
                items[productName] = (price, item.Quantity + 1);
            }
            else
            {
                items[productName] = (price, 1);
            }
            Console.WriteLine($"Added {productName} - Total: {items[productName].Quantity}");
        }
        
        public void RemoveItem(string productName)
        {
            if (items.ContainsKey(productName))
            {
                items.Remove(productName);
                Console.WriteLine($"Removed {productName}");
            }
        }
        
        public void UpdateQuantity(string productName, int quantity)
        {
            if (items.ContainsKey(productName))
            {
                var item = items[productName];
                items[productName] = (item.Price, quantity);
                Console.WriteLine($"Updated {productName} quantity to {quantity}");
            }
        }
        
        public int GetQuantity(string productName)
        {
            return items.ContainsKey(productName) ? items[productName].Quantity : 0;
        }
    }

    public class Inventory
    {
        private ICartMediator mediator;
        
        public Inventory(ICartMediator mediator)
        {
            this.mediator = mediator;
        }
        
        public void CheckStock(string item)
        {
            Console.WriteLine($"Checking stock for {item}: Available");
        }
    }

    public class Pricing
    {
        private ICartMediator mediator;
        
        public Pricing(ICartMediator mediator)
        {
            this.mediator = mediator;
        }
        
        public void CalculatePrice(string item)
        {
            Console.WriteLine($"Calculating price for {item}: $999.99");
        }
    }
}