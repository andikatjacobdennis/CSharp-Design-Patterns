using System;

// Abstract Products
public interface IButton
{
    void Render();
}

public interface ITextBox
{
    void Render();
}

// Concrete Products for Light Theme
public class LightButton : IButton
{
    public void Render() => Console.WriteLine("Rendering Light Button");
}

public class LightTextBox : ITextBox
{
    public void Render() => Console.WriteLine("Rendering Light TextBox");
}

// Concrete Products for Dark Theme
public class DarkButton : IButton
{
    public void Render() => Console.WriteLine("Rendering Dark Button");
}

public class DarkTextBox : ITextBox
{
    public void Render() => Console.WriteLine("Rendering Dark TextBox");
}

// Abstract Factory
public interface IThemeFactory
{
    IButton CreateButton();
    ITextBox CreateTextBox();
}

// Concrete Factories
public class LightThemeFactory : IThemeFactory
{
    public IButton CreateButton() => new LightButton();
    public ITextBox CreateTextBox() => new LightTextBox();
}

public class DarkThemeFactory : IThemeFactory
{
    public IButton CreateButton() => new DarkButton();
    public ITextBox CreateTextBox() => new DarkTextBox();
}

// Client
public class UI
{
    private readonly IButton _button;
    private readonly ITextBox _textBox;

    public UI(IThemeFactory factory)
    {
        _button = factory.CreateButton();
        _textBox = factory.CreateTextBox();
    }

    public void Render()
    {
        _button.Render();
        _textBox.Render();
    }
}

// Example usage
class Program
{
    static void Main()
    {
        IThemeFactory themeFactory;

        // Suppose user selected "Dark Theme"
        themeFactory = new DarkThemeFactory();

        var ui = new UI(themeFactory);
        ui.Render();

        // Switch to Light Theme
        themeFactory = new LightThemeFactory();
        ui = new UI(themeFactory);
        ui.Render();
    }
}
