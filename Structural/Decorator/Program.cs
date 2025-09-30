using System;

namespace DecoratorPatternDemo
{
    // -----------------------
    // Component
    // -----------------------
    public abstract class VisualComponent
    {
        public abstract void Draw();
    }

    // -----------------------
    // ConcreteComponent
    // -----------------------
    public class TextView : VisualComponent
    {
        public override void Draw()
        {
            Console.WriteLine("Drawing TextView");
        }
    }

    // -----------------------
    // Decorator
    // -----------------------
    public abstract class Decorator : VisualComponent
    {
        protected VisualComponent _component;

        protected Decorator(VisualComponent component)
        {
            _component = component;
        }

        public override void Draw()
        {
            _component.Draw(); // forward call
        }
    }

    // -----------------------
    // ConcreteDecorators
    // -----------------------

    public class BorderDecorator : Decorator
    {
        private int _borderWidth;

        public BorderDecorator(VisualComponent component, int borderWidth)
            : base(component)
        {
            _borderWidth = borderWidth;
        }

        public override void Draw()
        {
            base.Draw(); // draw wrapped component
            DrawBorder(_borderWidth);
        }

        private void DrawBorder(int width)
        {
            Console.WriteLine($"Drawing border with width {width}");
        }
    }

    public class ScrollDecorator : Decorator
    {
        public ScrollDecorator(VisualComponent component)
            : base(component)
        {
        }

        public override void Draw()
        {
            base.Draw(); // draw wrapped component
            DrawScrollBar();
        }

        private void DrawScrollBar()
        {
            Console.WriteLine("Drawing scrollbar");
        }
    }

    // -----------------------
    // Client
    // -----------------------
    class Program
    {
        static void Main(string[] args)
        {
            // Create a simple TextView
            VisualComponent textView = new TextView();

            Console.WriteLine("Simple TextView:");
            textView.Draw();

            Console.WriteLine("\nTextView with Border:");
            VisualComponent borderedTextView = new BorderDecorator(textView, 2);
            borderedTextView.Draw();

            Console.WriteLine("\nTextView with Scrollbar and Border:");
            VisualComponent decoratedTextView =
                new BorderDecorator(
                    new ScrollDecorator(textView), 3
                );
            decoratedTextView.Draw();

            Console.ReadLine();
        }
    }
}
