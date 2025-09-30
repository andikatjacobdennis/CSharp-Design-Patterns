## 1. Definitions of Design Patterns

| Category | Pattern | Definition | Analogy |
|----------|---------|-------------------|--------------|
| **CREATIONAL** | | **Getting Objects Born** | **How you get new things** |
| | Abstract Factory | Creates families of related objects | **IKEA catalog** - get whole matching furniture sets |
| | Builder | Builds complex objects step by step | **Lego kit** - follow instructions to build something big |
| | Factory Method | Lets subclasses decide what to create | **HR Department** - they decide who to hire for specific jobs |
| | Prototype | Creates new objects by copying | **Xerox machine** - just duplicate an existing object |
| | Singleton | Only one instance can exist | **The Company CEO** - there can only be one |
| **STRUCTURAL** | | **Building Object Structures** | **How you arrange things** |
| | Adapter | Makes incompatible interfaces work together | **Power plug adapter** - makes US plug work in EU socket |
| | Bridge | Separates abstraction from implementation | **Remote control** - same buttons work on different TV brands |
| | Composite | Treats single and groups of objects the same | **Organization chart** - manager with employees under them |
| | Decorator | Adds features to objects dynamically | **Layered clothing** - add jacket, scarf, gloves as needed |
| | Facade | Provides simple interface to complex system | **Hotel reception** - one desk handles all complex services |
| | Flyweight | Shares objects to save memory | **Library book** - one physical book, many people can borrow |
| | Proxy | Controls access to another object | **Security guard** - checks who can enter the building |
| **BEHAVIORAL** | | **Managing Object Communication** | **How things talk to each other** |
| | Chain of Responsibility | Passes request through chain of handlers | **Bureaucratic paperwork** - passed desk to desk until handled |
| | Command | Turns requests into stand-alone objects | **Restaurant order** - waiter gives chef a slip to execute |
| | Interpreter | Interprets and executes language grammar | **Google Translate** - understands and acts on language commands |
| | Iterator | Accesses elements of collection sequentially | **TV remote "Next"** - goes through channels one by one |
| | Mediator | Simplifies communication between objects | **Group chat admin** - everyone talks through one person |
| | Memento | Saves and restores object state | **Ctrl+Z / Undo** - goes back to previous state |
| | Observer | Notifies dependents of state changes | **YouTube subscription** - get notified when new video posts |
| | State | Changes behavior when state changes | **Traffic light** - stop (red) vs go (green) behavior |
| | Strategy | Lets you switch between algorithms | **GPS navigation** - choose fastest or cheapest route |
| | Template Method | Defines skeleton with customizable steps | **Job application** - same form, everyone fills different details |
| | Visitor | Adds operations without changing classes | **Insurance assessor** - checks every room in your house |

## 2. Difference between Factory Method and Builder

Factory Method is about **what type** of object to create, while Builder is about **how to construct** a complex object. 

Factory Method lets subclasses decide which class to instantiate, while Builder helps create objects step-by-step when they have many parts or configurations.
