# **Interpreter Pattern** in **C#**

## Overview

This project demonstrates the **Interpreter Pattern** using a practical example of **evaluating Boolean expressions**.

The **Interpreter Pattern** is a **behavioral** pattern that **defines a grammar for a language, represents sentences in that language as abstract syntax trees, and interprets them**.

In this example, we have:

* **`BooleanExp`**: Abstract class representing any Boolean expression.
* **`VariableExp`**: Represents a Boolean variable.
* **`Constant`**: Represents the Boolean constants `true` and `false`.
* **`AndExp`**, **`OrExp`**, **`NotExp`**: Represent composite expressions combining subexpressions.
* **`Context`**: Stores variable assignments and provides lookup for variable values.
* **`Client`**: Builds the expression tree and evaluates it.

---

## Structure

### Diagram

![UML Diagram illustrating the pattern](structure.png)

### 1. Core Interface / Abstract Class

* **`BooleanExp`**: Declares `Evaluate`, `Replace`, and `Copy` methods for all expressions.

### 2. Concrete Implementations

* **`VariableExp`**: Implements a named Boolean variable.
* **`Constant`**: Implements a Boolean constant.
* **`AndExp`**, **`OrExp`**, **`NotExp`**: Implement composite operations on Boolean expressions.

### 3. Client

* **`Client`**: Creates an abstract syntax tree representing a Boolean expression and evaluates it using a `Context`.

---

## Example Usage

```csharp
Context context = new Context();
VariableExp x = new VariableExp("X");
VariableExp y = new VariableExp("Y");

// Expression: (true AND X) OR (Y AND (NOT X))
BooleanExp expression = new OrExp(
    new AndExp(new Constant(true), x),
    new AndExp(y, new NotExp(x))
);

// Assign values to variables
context.Assign(x, false);
context.Assign(y, true);

// Evaluate the expression
bool result = expression.Evaluate(context);
Console.WriteLine(result); // Output: True

// Replace Y with NOT Z
VariableExp z = new VariableExp("Z");
BooleanExp replacement = expression.Replace("Y", new NotExp(z));
context.Assign(z, true);
Console.WriteLine(replacement.Evaluate(context)); // Output: False
```

### Output

```cmd
True
False
```

---

## Benefits

* **Easy to extend grammar**: New expression types can be added without modifying existing code.
* **Supports multiple interpretations**: Expressions can be evaluated, transformed, or pretty-printed.
* **Encapsulates grammar rules**: Each class represents a part of the language grammar, making maintenance easier for simple languages.

---

## Common Use Cases

* **Mathematical or logical expression evaluation**
* **Parsing and interpreting domain-specific languages (DSLs)**
* **Regular expression matching**
* **Rule engines or constraint solvers**
