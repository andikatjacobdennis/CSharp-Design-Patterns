using System;

namespace ProxyPatternExample
{
    // The Subject interface
    public abstract class Graphic
    {
        public abstract void Draw(Point at);
        public abstract Point GetExtent();
    }

    // The RealSubject
    public class Image : Graphic
    {
        private string _fileName;
        private Point _extent;

        public Image(string fileName)
        {
            _fileName = fileName;
            LoadImage();
        }

        private void LoadImage()
        {
            // Simulate expensive image loading
            Console.WriteLine($"Loading image from {_fileName}...");
            _extent = new Point(1920, 1080); // Example extent
        }

        public override void Draw(Point at)
        {
            Console.WriteLine($"Drawing image {_fileName} at {at.X}, {at.Y}");
        }

        public override Point GetExtent()
        {
            return _extent;
        }
    }

    // The Proxy
    public class ImageProxy : Graphic
    {
        private string _fileName;
        private Image _realImage;
        private Point _extent;

        public ImageProxy(string fileName)
        {
            _fileName = fileName;
            _extent = new Point(0, 0); // Unknown until loaded
            _realImage = null;
        }

        private Image GetImage()
        {
            if (_realImage == null)
            {
                _realImage = new Image(_fileName);
                if (_extent.X == 0 && _extent.Y == 0)
                {
                    _extent = _realImage.GetExtent();
                }
            }
            return _realImage;
        }

        public override void Draw(Point at)
        {
            GetImage().Draw(at);
        }

        public override Point GetExtent()
        {
            return _extent;
        }
    }

    // Helper struct to represent a point
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    // Client
    class Program
    {
        static void Main(string[] args)
        {
            Graphic image1 = new ImageProxy("picture1.jpg");
            Graphic image2 = new ImageProxy("picture2.jpg");

            // At this point, images are not loaded yet
            Console.WriteLine($"Image1 extent: {image1.GetExtent().X}x{image1.GetExtent().Y}");

            // Loading occurs only when Draw is called
            image1.Draw(new Point(100, 200));
            image2.Draw(new Point(50, 50));
        }
    }
}
