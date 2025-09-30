using System;

namespace AdapterPatternExample
{
    // Target interface
    public abstract class Shape
    {
        public abstract void BoundingBox(out int bottomLeftX, out int bottomLeftY, out int topRightX, out int topRightY);
        public abstract bool IsEmpty();
        public abstract Manipulator CreateManipulator();
    }

    // Manipulator for Shapes
    public abstract class Manipulator
    {
        public abstract void Manipulate();
    }

    // Adaptee
    public class TextView
    {
        public void GetOrigin(out int x, out int y)
        {
            x = 10; 
            y = 20;
        }

        public void GetExtent(out int width, out int height)
        {
            width = 100; 
            height = 50;
        }

        public bool IsEmpty()
        {
            return false;
        }
    }

    // Object Adapter
    public class TextShape : Shape
    {
        private TextView _textView;

        public TextShape(TextView textView)
        {
            _textView = textView;
        }

        public override void BoundingBox(out int bottomLeftX, out int bottomLeftY, out int topRightX, out int topRightY)
        {
            int originX, originY, width, height;
            _textView.GetOrigin(out originX, out originY);
            _textView.GetExtent(out width, out height);

            bottomLeftX = originX;
            bottomLeftY = originY;
            topRightX = originX + width;
            topRightY = originY + height;
        }

        public override bool IsEmpty()
        {
            return _textView.IsEmpty();
        }

        public override Manipulator CreateManipulator()
        {
            return new TextManipulator(this);
        }
    }

    // Manipulator Implementation
    public class TextManipulator : Manipulator
    {
        private Shape _shape;

        public TextManipulator(Shape shape)
        {
            _shape = shape;
        }

        public override void Manipulate()
        {
            Console.WriteLine("Manipulating shape...");
        }
    }

    // Client
    class Program
    {
        static void Main(string[] args)
        {
            TextView textView = new TextView();
            Shape textShape = new TextShape(textView);

            textShape.BoundingBox(out int x1, out int y1, out int x2, out int y2);
            Console.WriteLine($"BoundingBox: ({x1},{y1}) to ({x2},{y2})");

            Console.WriteLine($"IsEmpty: {textShape.IsEmpty()}");

            Manipulator manipulator = textShape.CreateManipulator();
            manipulator.Manipulate();
        }
    }
}
