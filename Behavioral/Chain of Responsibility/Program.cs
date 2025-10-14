using System;

abstract class HelpHandler
{
    protected HelpHandler successor;
    protected string helpTopic;

    public HelpHandler(HelpHandler successor, string topic = null)
    {
        this.successor = successor;
        this.helpTopic = topic;
    }

    public virtual void HandleHelp()
    {
        if (successor != null)
        {
            successor.HandleHelp();
        }
        else
        {
            Console.WriteLine("No help available.");
        }
    }

    protected bool HasHelp()
    {
        return !string.IsNullOrEmpty(helpTopic);
    }
}

class Button : HelpHandler
{
    public Button(HelpHandler successor, string topic = null) : base(successor, topic) { }

    public override void HandleHelp()
    {
        if (HasHelp())
        {
            Console.WriteLine(helpTopic);
        }
        else
        {
            base.HandleHelp();
        }
    }
}

class Dialog : HelpHandler
{
    public Dialog(HelpHandler successor, string topic = null) : base(successor, topic) { }

    public override void HandleHelp()
    {
        if (HasHelp())
        {
            Console.WriteLine(helpTopic);
        }
        else
        {
            base.HandleHelp();
        }
    }
}

class Application : HelpHandler
{
    public Application(string topic = null) : base(null, topic) { }

    public override void HandleHelp()
    {
        if (HasHelp())
        {
            Console.WriteLine(helpTopic);
        }
        else
        {
            base.HandleHelp();
        }
    }
}

class Program
{
    static void Main()
    {
        // Create the chain: Button -> Dialog -> Application
        Application app = new Application("General Application Help");
        Dialog dialog = new Dialog(app, "Print Dialog Help");
        Button button = new Button(dialog, "Print Button Help");

        // Start the help request from the button
        button.HandleHelp();
    }
}
