using System;

namespace BridgePatternDemo
{
    // Implementor: declares the interface for implementation classes
    public interface IWindowImp
    {
        void ImpOpen();
        void ImpClose();
        void ImpDrawRect(int x, int y, int width, int height);
        void ImpDrawText(string text, int x, int y);
    }

    // ConcreteImplementor A: e.g., an X Window System implementation (simulated)
    public class XWindowImp : IWindowImp
    {
        public void ImpOpen()
        {
            Console.WriteLine("[XWindowImp] Opening window using X Window System primitives.");
        }

        public void ImpClose()
        {
            Console.WriteLine("[XWindowImp] Closing window (X).");
        }

        public void ImpDrawRect(int x, int y, int width, int height)
        {
            Console.WriteLine($"[XWindowImp] Drawing rectangle at ({x},{y}) size {width}x{height} (X draw API call).");
        }

        public void ImpDrawText(string text, int x, int y)
        {
            Console.WriteLine($"[XWindowImp] Drawing text '{text}' at ({x},{y}) (X text API call).");
        }
    }

    // ConcreteImplementor B: e.g., a Presentation Manager / Windows implementation (simulated)
    public class PMWindowImp : IWindowImp
    {
        public void ImpOpen()
        {
            Console.WriteLine("[PMWindowImp] Opening window using Presentation Manager primitives.");
        }

        public void ImpClose()
        {
            Console.WriteLine("[PMWindowImp] Closing window (PM).");
        }

        public void ImpDrawRect(int x, int y, int width, int height)
        {
            Console.WriteLine($"[PMWindowImp] Drawing rectangle at ({x},{y}) size {width}x{height} (PM draw API call).");
        }

        public void ImpDrawText(string text, int x, int y)
        {
            Console.WriteLine($"[PMWindowImp] Drawing text '{text}' at ({x},{y}) (PM text API call).");
        }
    }

    // Abstraction: defines the abstraction's interface and maintains a reference to an Implementor
    public abstract class Window
    {
        protected IWindowImp _imp;

        protected Window(IWindowImp imp)
        {
            _imp = imp ?? throw new ArgumentNullException(nameof(imp));
        }

        // High-level operations -- implemented in terms of the Implementor's primitives
        public virtual void Open()
        {
            _imp.ImpOpen();
        }

        public virtual void Close()
        {
            _imp.ImpClose();
        }

        public abstract void DrawContents();

        // Operations forwarding to implementor primitives
        public void SetBounds(int x, int y, int width, int height)
        {
            // maybe do some abstraction-level validation or transformation
            _imp.ImpDrawRect(x, y, width, height);
        }

        public void DrawText(string text, int x, int y)
        {
            _imp.ImpDrawText(text, x, y);
        }

        // Allow changing the implementor at runtime
        public void SetImplementor(IWindowImp newImp)
        {
            _imp = newImp ?? throw new ArgumentNullException(nameof(newImp));
        }
    }

    // RefinedAbstraction A: an application window
    public class ApplicationWindow : Window
    {
        private readonly string _title;

        public ApplicationWindow(IWindowImp imp, string title)
            : base(imp)
        {
            _title = title;
        }

        public override void DrawContents()
        {
            Console.WriteLine($"[ApplicationWindow] Drawing contents of '{_title}'.");
            // Use the implementor primitives
            SetBounds(10, 10, 300, 200);
            DrawText(_title, 20, 30);
        }
    }

    // RefinedAbstraction B: an icon window
    public class IconWindow : Window
    {
        private readonly string _iconName;

        public IconWindow(IWindowImp imp, string iconName)
            : base(imp)
        {
            _iconName = iconName;
        }

        public override void DrawContents()
        {
            Console.WriteLine($"[IconWindow] Drawing icon '{_iconName}'.");
            // Icons are small
            SetBounds(0, 0, 64, 64);
            DrawText(_iconName, 8, 32);
        }
    }

    // Optional: a factory that returns a platform-appropriate implementor
    public static class WindowImpFactory
    {
        public enum Platform
        {
            X,
            PM
        }

        // In a real app this might read from configuration or detect OS
        public static IWindowImp Create(Platform platform)
        {
            switch (platform)
            {
                case Platform.X:
                    return new XWindowImp();
                case Platform.PM:
                    return new PMWindowImp();
                default:
                    throw new NotSupportedException($"Platform {platform} not supported.");
            }
        }
    }

    // Client code example
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Bridge Pattern Demo (C#)\n");

            // Create implementors (could be chosen at runtime)
            IWindowImp xImp = WindowImpFactory.Create(WindowImpFactory.Platform.X);
            IWindowImp pmImp = WindowImpFactory.Create(WindowImpFactory.Platform.PM);

            // Create abstractions that use implementors
            Window appWindow = new ApplicationWindow(xImp, "My Application");
            Window iconWindow = new IconWindow(pmImp, "MyIcon.png");

            // Operate through the abstraction
            appWindow.Open();
            appWindow.DrawContents();
            appWindow.Close();

            Console.WriteLine();

            iconWindow.Open();
            iconWindow.DrawContents();
            iconWindow.Close();

            Console.WriteLine();

            // Demonstrate switching implementor at runtime
            Console.WriteLine("Switching the ApplicationWindow implementor at runtime to PM implementation...\n");
            appWindow.SetImplementor(pmImp);
            appWindow.Open();
            appWindow.DrawContents();
            appWindow.Close();

            Console.WriteLine("Demo complete.");
        }
    }
}
