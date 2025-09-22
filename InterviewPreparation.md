## 1. Definitions of Design Patterns

| **Category**   | **Pattern**             | **Purpose**                                                             |
| -------------- | ----------------------- | ----------------------------------------------------------------------- |
| **Creational** | Abstract Factory        | Provide an interface for creating families of related or dependent objects without specifying their concrete classes.              |
|                | Builder                 | Separates object construction from its representation                   |
|                | Factory Method          | Creates instances of derived classes                                    |
|                | Prototype               | Produces a copy or clone of a fully initialized instance                |
|                | Singleton               | Ensures a class has only one instance                                   |
| **Structural** | Adapter                 | Aligns interfaces of different classes                                  |
|                | Bridge                  | Decouples an object’s interface from its implementation                 |
|                | Composite               | Organizes objects into tree structures of simple and composite elements |
|                | Decorator               | Dynamically adds responsibilities to objects                            |
|                | Facade                  | Provides a unified interface to a subsystem                             |
|                | Flyweight               | Uses fine-grained objects for efficient sharing                         |
|                | Proxy                   | Represents or controls access to another object                         |
| **Behavioral** | Chain of Responsibility | Passes a request along a chain of objects                               |
|                | Command                 | Encapsulates a request as an object                                     |
|                | Interpreter             | Implements language elements within a program                           |
|                | Iterator                | Provides sequential access to elements in a collection                  |
|                | Mediator                | Simplifies communication between classes                                |
|                | Memento                 | Captures and restores an object’s internal state                        |
|                | Observer                | Notifies multiple classes about state changes                           |
|                | State                   | Changes an object’s behavior when its state changes                     |
|                | Strategy                | Encapsulates an algorithm within a class                                |
|                | Template Method         | Defers specific steps of an algorithm to subclasses                     |
|                | Visitor                 | Adds new operations to classes without modifying them                   |

## 2. Difference between Factory Method and Builder

Factory Method is about **what type** of object to create, while Builder is about **how to construct** a complex object. 

Factory Method lets subclasses decide which class to instantiate, while Builder helps create objects step-by-step when they have many parts or configurations.
