// CompositePattern.cs
// A reusable C# template that demonstrates the Composite Design Pattern.
// Namespace: CompositePatternDemo
// To run: create a new console project (dotnet new console) and replace Program.cs with this file.

using System;
using System.Collections.Generic;

namespace CompositePatternDemo
{
    // Component: declares the interface for objects in the composition.
    // It provides default implementations for child-management that throw
    // NotSupportedException to favor safety (leaves can't add children).
    public abstract class Graphic
    {
        private Graphic _parent;

        // Optional parent reference (helps with traversal and cache invalidation)
        public Graphic Parent
        {
            get => _parent;
            internal set => _parent = value; // internal so only composites can set it
        }

        // Operation that all components must implement (e.g., Draw)
        public abstract void Draw(string indent = "");

        // Child management operations
        // Default implementations throw, making it explicit that leaves
        // cannot have children unless overridden by a Composite subclass.
        public virtual void Add(Graphic g)
        {
            throw new NotSupportedException("Add not supported on this component.");
        }

        public virtual void Remove(Graphic g)
        {
            throw new NotSupportedException("Remove not supported on this component.");
        }

        // A helper to test whether this component is a composite.
        // Returns null by default; Composite overrides to return itself.
        public virtual CompositeGraphic AsComposite() => null;

        // Optional: a way to get children for traversal without exposing concrete types.
        // Default returns empty sequence.
        public virtual IEnumerable<Graphic> Children()
        {
            yield break;
        }
    }

    // Leaf: represents end objects in the tree (no children)
    public class Line : Graphic
    {
        public string Name { get; }
        public Line(string name) { Name = name; }

        public override void Draw(string indent = "")
        {
            Console.WriteLine($"{indent}- Line: {Name}");
        }
    }

    public class Text : Graphic
    {
        public string Content { get; }
        public Text(string content) { Content = content; }

        public override void Draw(string indent = "")
        {
            Console.WriteLine($"{indent}- Text: \"{Content}\"");
        }
    }

    // Composite: stores child components and implements child-related ops
    public class CompositeGraphic : Graphic
    {
        private readonly List<Graphic> _children = new List<Graphic>();
        public string Name { get; }

        public CompositeGraphic(string name) { Name = name; }

        public override void Add(Graphic g)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));
            // Prevent cycles: do not add an ancestor as a child
            for (var current = this; current != null; current = current.Parent)
            {
                if (ReferenceEquals(current, g))
                    throw new InvalidOperationException("Cannot add an ancestor as a child (would create a cycle).");
            }

            _children.Add(g);
            g.Parent = this; // maintain invariant
        }

        public override void Remove(Graphic g)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));
            if (_children.Remove(g))
            {
                g.Parent = null;
            }
            else
            {
                throw new InvalidOperationException("The provided component is not a child of this composite.");
            }
        }

        public override CompositeGraphic AsComposite() => this;

        public override IEnumerable<Graphic> Children() => _children;

        public override void Draw(string indent = "")
        {
            Console.WriteLine($"{indent}+ Composite: {Name}");
            var childIndent = indent + "  ";
            foreach (var child in _children)
            {
                child.Draw(childIndent);
            }
        }
    }

    // Client usage demonstrating uniform treatment of Leaf and Composite
    public static class DemoClient
    {
        public static void Run()
        {
            // Build a graphic scene tree
            var picture = new CompositeGraphic("Root Picture");

            var header = new CompositeGraphic("Header");
            header.Add(new Text("Welcome to Composite Demo"));

            var body = new CompositeGraphic("Body");
            body.Add(new Line("Line 1"));
            body.Add(new Line("Line 2"));

            var group = new CompositeGraphic("Grouped Shapes");
            group.Add(new Line("Diagonal"));
            group.Add(new Text("Grouped Label"));

            body.Add(group);

            picture.Add(header);
            picture.Add(body);

            // Treat everything uniformly: draw the whole picture
            Console.WriteLine("Drawing scene:");
            picture.Draw();

            Console.WriteLine();

            // Attempting to add to a leaf throws an exception (safety)
            try
            {
                var leaf = new Line("Lonely Line");
                // leaf.Add(new Text("Should fail")); // would throw
                // but we can attempt and catch
                leaf.Add(new Text("Should fail"));
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("Caught expected exception when adding to leaf: " + ex.Message);
            }

            Console.WriteLine();

            // Demonstrate safe Add via AsComposite()
            Graphic maybeComposite = picture.Children().GetEnumerator().MoveNext() ? (Graphic)picture : (Graphic)picture; // trivial example

            if (picture.AsComposite() is CompositeGraphic comp)
            {
                comp.Add(new Text("Footer added via AsComposite"));
            }

            Console.WriteLine("Drawing updated scene:");
            picture.Draw();

            // Demonstrate Remove
            Console.WriteLine();
            Console.WriteLine("Removing 'Header' from root and drawing again:");
            picture.Remove(header);
            picture.Draw();
        }
    }

    // Program entry — small runner
    class Program
    {
        static void Main(string[] args)
        {
            DemoClient.Run();
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
