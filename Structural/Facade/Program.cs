// FacadePattern_CompilerExample.cs
// C# template demonstrating the Facade design pattern using the compiler example
// from the Gang of Four text. This file is a single-file console application.

using System;
using System.Collections.Generic;
using System.IO;

namespace FacadePatternExample
{
    #region Subsystem Classes (Simplified)

    // Token - a minimal token representation
    public class Token
    {
        public string Text { get; }
        public Token(string text) => Text = text;
        public override string ToString() => Text;
    }

    // Bytecode - represents emitted machine code / instructions
    public class Bytecode
    {
        public string Instruction { get; }
        public Bytecode(string instruction) => Instruction = instruction;
        public override string ToString() => Instruction;
    }

    // BytecodeStream - collects bytecodes
    public class BytecodeStream
    {
        private readonly List<Bytecode> _codes = new();
        public void Emit(Bytecode code) => _codes.Add(code);
        public IEnumerable<Bytecode> GetAll() => _codes;
    }

    // Scanner - converts raw input into tokens (very simplified)
    public class Scanner
    {
        private readonly TextReader _reader;
        public Scanner(TextReader reader) => _reader = reader;

        // For demo purposes, produce a sequence of whitespace-separated tokens
        public IEnumerable<Token> Scan()
        {
            string input = _reader.ReadToEnd();
            if (string.IsNullOrWhiteSpace(input)) yield break;

            var pieces = input.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in pieces)
                yield return new Token(p);
        }
    }

    // ProgramNode - a node in the parse tree (very small composite)
    public abstract class ProgramNode
    {
        public List<ProgramNode> Children { get; } = new();
        public virtual void Add(ProgramNode child) => Children.Add(child);
        public abstract void Traverse(CodeGenerator generator);
    }

    // Example concrete nodes
    public class StatementNode : ProgramNode
    {
        public string Text { get; }
        public StatementNode(string text) => Text = text;
        public override void Traverse(CodeGenerator generator) => generator.Visit(this);
    }

    public class ExpressionNode : ProgramNode
    {
        public string ExpressionText { get; }
        public ExpressionNode(string text) => ExpressionText = text;
        public override void Traverse(CodeGenerator generator) => generator.Visit(this);
    }

    // ProgramNodeBuilder - builds a minimal parse tree from tokens
    public class ProgramNodeBuilder
    {
        private readonly ProgramNode _root = new StatementNode("Root");
        public void AddStatement(string text) => _root.Add(new StatementNode(text));
        public ProgramNode GetRootNode() => _root;
    }

    // Parser - builds the parse tree from tokens using the builder
    public class Parser
    {
        public void Parse(IEnumerable<Token> tokens, ProgramNodeBuilder builder)
        {
            // For demonstration: every token becomes a statement node
            foreach (var t in tokens)
            {
                builder.AddStatement(t.Text);
            }
        }
    }

    // CodeGenerator - visitor-like base class
    public abstract class CodeGenerator
    {
        protected readonly BytecodeStream Output;
        protected CodeGenerator(BytecodeStream output) => Output = output;

        public virtual void Visit(StatementNode node)
        {
            // Default behavior: emit a "statement" instruction
            Output.Emit(new Bytecode($"STATEMENT: {node.Text}"));
            foreach (var child in node.Children)
                child.Traverse(this);
        }

        public virtual void Visit(ExpressionNode node)
        {
            Output.Emit(new Bytecode($"EXPR: {node.ExpressionText}"));
            foreach (var child in node.Children)
                child.Traverse(this);
        }
    }

    // Two example concrete code generators
    public class StackMachineCodeGenerator : CodeGenerator
    {
        public StackMachineCodeGenerator(BytecodeStream output) : base(output) { }
        public override void Visit(StatementNode node)
        {
            Output.Emit(new Bytecode($"PUSH \"{node.Text}\""));
            Output.Emit(new Bytecode("CALL execute_statement"));
            base.Visit(node);
        }
    }

    public class RiscCodeGenerator : CodeGenerator
    {
        public RiscCodeGenerator(BytecodeStream output) : base(output) { }
        public override void Visit(StatementNode node)
        {
            Output.Emit(new Bytecode($"MOV r0, \"{node.Text}\""));
            Output.Emit(new Bytecode("BL execute_statement"));
            base.Visit(node);
        }
    }

    #endregion

    #region Facade: Compiler

    // The Facade class that provides a simple interface to the compiler subsystem.
    public class Compiler
    {
        // Allow the client to supply a code generator factory, but provide a sensible default.
        private readonly Func<BytecodeStream, CodeGenerator> _generatorFactory;

        public Compiler(Func<BytecodeStream, CodeGenerator>? generatorFactory = null)
        {
            _generatorFactory = generatorFactory ?? (output => new RiscCodeGenerator(output));
        }

        // A simple compile method: takes source text and returns the emitted bytecodes
        public BytecodeStream Compile(string source)
        {
            using var reader = new StringReader(source);

            // 1. Lexing / scanning
            var scanner = new Scanner(reader);
            var tokens = scanner.Scan();

            // 2. Parsing / building AST
            var builder = new ProgramNodeBuilder();
            var parser = new Parser();
            parser.Parse(tokens, builder);

            // 3. Code generation
            var output = new BytecodeStream();
            var generator = _generatorFactory(output);

            var parseTree = builder.GetRootNode();
            parseTree.Traverse(generator);

            return output;
        }
    }

    #endregion

    #region Demo / Usage

    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Facade Pattern — Compiler Example (C#)");
            Console.WriteLine();

            string source = "print hello world; add 1 2; return 0;";
            Console.WriteLine("Source:\n" + source + "\n");

            // Use default Compiler (RISC generator)
            var compiler = new Compiler();
            var bytecodes = compiler.Compile(source);

            Console.WriteLine("Bytecodes (RISC):");
            foreach (var b in bytecodes.GetAll())
                Console.WriteLine("  " + b);

            Console.WriteLine();

            // Use a different generator by configuring the facade
            var compilerWithStack = new Compiler(output => new StackMachineCodeGenerator(output));
            var bytecodesStack = compilerWithStack.Compile(source);

            Console.WriteLine("Bytecodes (StackMachine):");
            foreach (var b in bytecodesStack.GetAll())
                Console.WriteLine("  " + b);

            Console.WriteLine();
            Console.WriteLine("Done.");
        }
    }

    #endregion
}
