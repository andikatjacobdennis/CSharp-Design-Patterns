using System;
using System.Collections.Generic;

// Context stores variable assignments
public class Context
{
    private readonly Dictionary<string, bool> _variables = new();

    public bool Lookup(string name)
    {
        return _variables.ContainsKey(name) && _variables[name];
    }

    public void Assign(VariableExp variable, bool value)
    {
        _variables[variable.Name] = value;
    }
}

// Abstract expression
public abstract class BooleanExp
{
    public abstract bool Evaluate(Context context);
    public abstract BooleanExp Replace(string name, BooleanExp exp);
    public abstract BooleanExp Copy();
}

// Variable expression
public class VariableExp : BooleanExp
{
    public string Name { get; }

    public VariableExp(string name)
    {
        Name = name;
    }

    public override bool Evaluate(Context context)
    {
        return context.Lookup(Name);
    }

    public override BooleanExp Replace(string name, BooleanExp exp)
    {
        if (Name == name)
            return exp.Copy();
        return new VariableExp(Name);
    }

    public override BooleanExp Copy()
    {
        return new VariableExp(Name);
    }
}

// Constant expression
public class Constant : BooleanExp
{
    private readonly bool _value;

    public Constant(bool value)
    {
        _value = value;
    }

    public override bool Evaluate(Context context)
    {
        return _value;
    }

    public override BooleanExp Replace(string name, BooleanExp exp)
    {
        return new Constant(_value);
    }

    public override BooleanExp Copy()
    {
        return new Constant(_value);
    }
}

// AND expression
public class AndExp : BooleanExp
{
    private readonly BooleanExp _operand1;
    private readonly BooleanExp _operand2;

    public AndExp(BooleanExp operand1, BooleanExp operand2)
    {
        _operand1 = operand1;
        _operand2 = operand2;
    }

    public override bool Evaluate(Context context)
    {
        return _operand1.Evaluate(context) && _operand2.Evaluate(context);
    }

    public override BooleanExp Replace(string name, BooleanExp exp)
    {
        return new AndExp(_operand1.Replace(name, exp), _operand2.Replace(name, exp));
    }

    public override BooleanExp Copy()
    {
        return new AndExp(_operand1.Copy(), _operand2.Copy());
    }
}

// OR expression
public class OrExp : BooleanExp
{
    private readonly BooleanExp _operand1;
    private readonly BooleanExp _operand2;

    public OrExp(BooleanExp operand1, BooleanExp operand2)
    {
        _operand1 = operand1;
        _operand2 = operand2;
    }

    public override bool Evaluate(Context context)
    {
        return _operand1.Evaluate(context) || _operand2.Evaluate(context);
    }

    public override BooleanExp Replace(string name, BooleanExp exp)
    {
        return new OrExp(_operand1.Replace(name, exp), _operand2.Replace(name, exp));
    }

    public override BooleanExp Copy()
    {
        return new OrExp(_operand1.Copy(), _operand2.Copy());
    }
}

// NOT expression
public class NotExp : BooleanExp
{
    private readonly BooleanExp _operand;

    public NotExp(BooleanExp operand)
    {
        _operand = operand;
    }

    public override bool Evaluate(Context context)
    {
        return !_operand.Evaluate(context);
    }

    public override BooleanExp Replace(string name, BooleanExp exp)
    {
        return new NotExp(_operand.Replace(name, exp));
    }

    public override BooleanExp Copy()
    {
        return new NotExp(_operand.Copy());
    }
}

// Client
class Program
{
    static void Main()
    {
        Context context = new Context();
        VariableExp x = new VariableExp("X");
        VariableExp y = new VariableExp("Y");

        // Expression: (true AND X) OR (Y AND (NOT X))
        BooleanExp expression = new OrExp(
            new AndExp(new Constant(true), x),
            new AndExp(y, new NotExp(x))
        );

        context.Assign(x, false);
        context.Assign(y, true);

        bool result = expression.Evaluate(context);
        Console.WriteLine(result); // True

        // Replace Y with NOT Z
        VariableExp z = new VariableExp("Z");
        BooleanExp replacement = expression.Replace("Y", new NotExp(z));
        context.Assign(z, true);

        Console.WriteLine(replacement.Evaluate(context)); // False
    }
}
