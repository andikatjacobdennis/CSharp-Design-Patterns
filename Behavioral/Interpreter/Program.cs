using System;

// 1. Context (Contains information global to the interpreter)
// For this simple math example, Context is minimal but crucial for the pattern structure.
// In real-world scenarios, it might hold variable assignments (e.g., 'x=5'), input streams, etc.
public class Context
{
    public string GlobalInfo { get; set; } = "Interpretation context initialized.";
}

// 2. AbstractExpression (Declares the Interpret operation)
// This is the common interface for all nodes in the Abstract Syntax Tree (AST).
public abstract class AbstractExpression
{
    // The Interpret method takes the Context and returns the result of the expression.
    public abstract int Interpret(Context context);
}

// 3. TerminalExpression (Represents terminal symbols, e.g., literals/numbers)
public class NumberExpression : AbstractExpression
{
    private readonly int _number;

    // Stores the literal value for this terminal node.
    public NumberExpression(int number)
    {
        _number = number;
    }

    // Interprets the terminal symbol: simply returns its stored value.
    public override int Interpret(Context context)
    {
        // For demonstration, we print the interpretation trace.
        Console.WriteLine($"[Number] Interpreting: {_number}");
        return _number;
    }
}

// 3. NonterminalExpression (Represents nonterminal symbols, e.g., operators)
public class AddExpression : AbstractExpression
{
    private readonly AbstractExpression _leftOperand;
    private readonly AbstractExpression _rightOperand;

    // Nonterminal nodes hold references to their sub-expressions (operands).
    public AddExpression(AbstractExpression left, AbstractExpression right)
    {
        _leftOperand = left;
        _rightOperand = right;
    }

    // Interprets the nonterminal symbol by recursively calling Interpret on its operands.
    public override int Interpret(Context context)
    {
        int leftResult = _leftOperand.Interpret(context);
        int rightResult = _rightOperand.Interpret(context);
        
        int result = leftResult + rightResult;
        
        // For demonstration, we print the operation result.
        Console.WriteLine($"[Add] Calculating: {leftResult} + {rightResult} = {result}");
        return result;
    }
}

// 3. NonterminalExpression (Another operator)
public class SubtractExpression : AbstractExpression
{
    private readonly AbstractExpression _leftOperand;
    private readonly AbstractExpression _rightOperand;

    public SubtractExpression(AbstractExpression left, AbstractExpression right)
    {
        _leftOperand = left;
        _rightOperand = right;
    }

    // Interprets the nonterminal symbol by recursively calling Interpret on its operands.
    public override int Interpret(Context context)
    {
        int leftResult = _leftOperand.Interpret(context);
        int rightResult = _rightOperand.Interpret(context);
        
        int result = leftResult - rightResult;
        
        // For demonstration, we print the operation result.
        Console.WriteLine($"[Subtract] Calculating: {leftResult} - {rightResult} = {result}");
        return result;
    }
}

// 4. Client (Builds the AST and initiates interpretation)
public class Program
{
    public static void Main(string[] args)
    {
        // Define the expression we want to interpret: (4 - 2) + 5
        
        Console.WriteLine("--- Building Abstract Syntax Tree (AST) for: (4 - 2) + 5 ---");
        
        // Step 1: Create terminal expressions for the numbers
        AbstractExpression four = new NumberExpression(4);
        AbstractExpression two = new NumberExpression(2);
        AbstractExpression five = new NumberExpression(5);

        // Step 2: Create the inner nonterminal expression (4 - 2)
        AbstractExpression subtractExp = new SubtractExpression(four, two);

        // Step 3: Create the root nonterminal expression (4 - 2) + 5
        AbstractExpression rootExpression = new AddExpression(subtractExp, five);

        Console.WriteLine("AST built successfully.");
        Console.WriteLine("\n--- Interpretation Phase ---\n");
        
        Context context = new Context();
        Console.WriteLine($"Context status: {context.GlobalInfo}\n");

        // Interpret the entire expression by calling Interpret on the root node
        int finalResult = rootExpression.Interpret(context);

        Console.WriteLine("\n--- Final Result ---");
        Console.WriteLine($"The result of the expression (4 - 2) + 5 is: {finalResult}");
    }
}
