# C# Design Patterns

This repository provides C# implementations of the 23 classic Gang of Four (GoF) design patterns, as described in the book *Design Patterns: Elements of Reusable Object-Oriented Software* by Erich Gamma, Richard Helm, Ralph Johnson, and John Vlissides. These patterns offer reusable solutions to common software design problems, categorized into creational, structural, and behavioral types.

The implementations are written in modern C# and aim to be clear, concise, and easy to understand. Each pattern includes sample code demonstrating its structure, participants, collaborations, and usage.

## Why Use Design Patterns?
Design patterns help you create more flexible, maintainable, and scalable object-oriented software. They encapsulate best practices for solving recurring design challenges, promoting code reuse and reducing complexity.

## Patterns Implemented
The patterns are organized by category, following the structure from the GoF book.

### Creational Patterns
These patterns deal with object creation mechanisms, abstracting the instantiation process to make systems more flexible.

- **Abstract Factory**: Provide an interface for creating families of related or dependent objects without specifying their concrete classes.
- **Builder**: Separates the construction of a complex object from its representation, allowing the same construction process to create different representations.
- **Factory Method**: Defines an interface for creating an object, but lets subclasses decide which class to instantiate.
- **Prototype**: Specifies the kinds of objects to create using a prototypical instance, and creates new objects by copying this prototype.
- **Singleton**: Ensures a class has only one instance and provides a global point of access to it.

### Structural Patterns
These patterns focus on composing classes and objects to form larger structures while keeping them flexible and efficient.

- **Adapter**: Converts the interface of a class into another interface that clients expect, allowing incompatible classes to work together.
- **Bridge**: Decouples an abstraction from its implementation so that the two can vary independently.
- **Composite**: Composes objects into tree structures to represent part-whole hierarchies, letting clients treat individual objects and compositions uniformly.
- **Decorator**: Attaches additional responsibilities to an object dynamically, providing a flexible alternative to subclassing for extending functionality.
- **Facade**: Provides a unified interface to a set of interfaces in a subsystem, simplifying usage.
- **Flyweight**: Uses sharing to support large numbers of fine-grained objects efficiently.
- **Proxy**: Provides a surrogate or placeholder for another object to control access to it.

### Behavioral Patterns
These patterns address how classes and objects interact and distribute responsibilities.

- **Chain of Responsibility**: Avoids coupling the sender of a request to its receiver by giving more than one object a chance to handle the request.
- **Command**: Encapsulates a request as an object, thereby allowing parameterization of clients with queues, requests, and operations.
- **Interpreter**: Given a language, defines a representation for its grammar along with an interpreter that uses the representation to interpret sentences.
- **Iterator**: Provides a way to access the elements of an aggregate object sequentially without exposing its underlying representation.
- **Mediator**: Defines an object that encapsulates how a set of objects interact, promoting loose coupling.
- **Memento**: Without violating encapsulation, captures and externalizes an object's internal state so that the object can be restored to this state later.
- **Observer**: Defines a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.
- **State**: Allows an object to alter its behavior when its internal state changes, appearing as if the object changed its class.
- **Strategy**: Defines a family of algorithms, encapsulates each one, and makes them interchangeable.
- **Template Method**: Defines the skeleton of an algorithm in a method, deferring some steps to subclasses.
- **Visitor**: Represent an operation to be performed on the elements of an object structure. Visitor lets you define a new operation without changing the classes of the elements on which it operates.

## Repository Structure
- Each pattern is in its own directory (e.g., `Creational/AbstractFactory/`), containing:
  - Source code files (e.g., `.cs` files for classes and examples).
  - A brief README.md explaining the pattern's intent, motivation, applicability, and sample usage.
- Root-level files:
  - `Program.cs`: Entry point for running examples (if applicable).
  - Tests or demo projects (planned for future updates).

## Getting Started
1. Clone the repository:
   ```
   git clone https://github.com/andikatjacobdennis/CSharp-Design-Patterns.git
   ```
2. Open the solution in Visual Studio or your preferred IDE.
3. Build and run individual pattern examples or the main program to see demonstrations.
4. Requirements: .NET 8.0 or later.

## Contributing
Contributions are welcome! If you'd like to add improvements, fix bugs, or provide better examples:
- Fork the repository.
- Create a feature branch.
- Submit a pull request with a clear description of changes.

Please follow standard C# coding conventions and include unit tests where possible.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments
- Inspired by the original GoF book.
- Thanks to the open-source community for ongoing discussions on design patterns.

For questions or suggestions, open an issue on GitHub.
