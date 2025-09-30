## 1. Definitions of Design Patterns

| Category | Pattern | Definition | Analogy |
|----------|---------|-------------------|---------------------|
| **CREATIONAL** | | **How objects are created** |  |
| | Abstract Factory | Creates families of related objects | **Regional shopping experiences** - different tax, currency for US vs EU |
| | Builder | Builds complex objects step by step | **Build complex orders** with items, shipping, insurance |
| | Factory Method | Lets subclasses decide what to create | **Create different product types** - digital, physical, subscription |
| | Prototype | Creates new objects by copying | **Clone product templates** for quick creation |
| | Singleton | Only one instance can exist | **Cart manager** - single instance managing all carts |
| **STRUCTURAL** | | **How classes/objects are composed** |  |
| | Adapter | Makes incompatible interfaces work together | **Adapt legacy payment systems** to modern cart |
| | Bridge | Separates abstraction from implementation | **Separate cart** from storage (database, session, cache) |
| | Composite | Treats single and groups of objects the same | **Product bundles** - treat single products and bundles uniformly |
| | Decorator | Adds features to objects dynamically | **Add features to products** - gift wrap, insurance, shipping |
| | Facade | Provides simple interface to complex system | **Simple checkout** hiding payment, inventory, shipping |
| | Flyweight | Shares objects to save memory | **Share product data** (name, description) across items |
| | Proxy | Controls access to another object | **Lazy loading images** - load high-res only when viewed |
| **BEHAVIORAL** | | **How objects interact & communicate** |  |
| | Chain of Responsibility | Passes request through chain of handlers | **Discount validation chain** - student → seasonal → loyalty |
| | Command | Turns requests into stand-alone objects | **Cart operations** - add, remove, update as undoable commands |
| | Interpreter | Interprets and executes language grammar | **Interpret promo codes** - "SAVE10", "FREESHIP50" |
| | Iterator | Accesses elements of collection sequentially | **Iterate through cart items** for display or calculation |
| | Mediator | Simplifies communication between objects | **Cart mediator** between items, inventory, pricing |
| | Memento | Saves and restores object state | **Save/restore cart** for undo or session recovery |
| | Observer | Notifies dependents of state changes | **Notify users** on price changes or cart abandonment |
| | State | Changes behavior when state changes | **Cart states** - empty, active, checkout, completed |
| | Strategy | Lets you switch between algorithms | **Pricing strategies** - member, bulk, seasonal pricing |
| | Template Method | Defines skeleton with customizable steps | **Checkout process** with country-specific tax |
| | Visitor | Adds operations without modifying classes | **Cart visitors** for price, tax, inventory calculations |

## 2. Difference between Factory Method and Builder

Factory Method is about **what type** of object to create, while Builder is about **how to construct** a complex object. 

Factory Method lets subclasses decide which class to instantiate, while Builder helps create objects step-by-step when they have many parts or configurations.
