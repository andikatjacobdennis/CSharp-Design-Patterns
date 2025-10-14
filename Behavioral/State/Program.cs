using System;

// The State Pattern allows an object (Context) to alter its behavior when its 
// internal state changes. The object delegates state-specific behavior to one 
// of its State objects.

// 1. Abstract State Class (State)
// Defines the interface for all concrete state implementations.
public abstract class TCPState
{
    // These methods represent the requests that the TCPConnection can handle, 
    // and their behavior changes based on the current state.
    public virtual void ActiveOpen(TCPConnection t) { Console.WriteLine("Action 'ActiveOpen' not valid in this state."); }
    public virtual void PassiveOpen(TCPConnection t) { Console.WriteLine("Action 'PassiveOpen' not valid in this state."); }
    public virtual void Close(TCPConnection t) { Console.WriteLine("Action 'Close' not valid in this state."); }
    public virtual void Send(TCPConnection t) { Console.WriteLine("Action 'Send' not valid in this state."); }

    // Helper method for the Concrete States to easily change the Context's state.
    protected void ChangeState(TCPConnection t, TCPState s)
    {
        t.ChangeState(s);
    }
}

// 2. Concrete State Classes (ConcreteState)
// Implemented as Singletons because they are stateless (only behavior, no instance data).

public class TCPClosed : TCPState
{
    private static TCPClosed _instance;
    private TCPClosed() { }
    public static TCPClosed Instance()
    {
        // Singleton implementation
        if (_instance == null) _instance = new TCPClosed();
        return _instance;
    }

    public override void ActiveOpen(TCPConnection t)
    {
        Console.WriteLine("CLOSED: Sending SYN, receiving SYN/ACK. Connection established.");
        // State transition: Closed -> Established
        ChangeState(t, TCPEstablished.Instance());
    }

    public override void PassiveOpen(TCPConnection t)
    {
        Console.WriteLine("CLOSED: Entering passive listening mode.");
        // State transition: Closed -> Listen
        ChangeState(t, TCPListen.Instance());
    }

    public override void Close(TCPConnection t)
    {
        Console.WriteLine("CLOSED: Connection is already closed.");
    }
}

public class TCPEstablished : TCPState
{
    private static TCPEstablished _instance;
    private TCPEstablished() { }
    public static TCPEstablished Instance()
    {
        if (_instance == null) _instance = new TCPEstablished();
        return _instance;
    }

    public override void Close(TCPConnection t)
    {
        Console.WriteLine("ESTABLISHED: Sending FIN, receiving ACK. Closing connection...");
        // State transition: Established -> Listen (Simplified for demo)
        ChangeState(t, TCPListen.Instance());
    }
    
    public override void Send(TCPConnection t)
    {
        Console.WriteLine("ESTABLISHED: Successfully transmitting data octet stream.");
    }
}

public class TCPListen : TCPState
{
    private static TCPListen _instance;
    private TCPListen() { }
    public static TCPListen Instance()
    {
        if (_instance == null) _instance = new TCPListen();
        return _instance;
    }

    public override void Send(TCPConnection t)
    {
        // In the listening state, receiving an incoming request (SYN) can cause a transition.
        Console.WriteLine("LISTEN: Received SYN request. Sending SYN/ACK. Connection established.");
        // State transition: Listen -> Established
        ChangeState(t, TCPEstablished.Instance());
    }
}


// 3. Context Class (Context)
// Maintains the state object and delegates all state-specific requests to it.
public class TCPConnection
{
    private TCPState _state;

    public TCPConnection()
    {
        // Set the initial state
        _state = TCPClosed.Instance();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n[CONNECTION INITIALIZED] Initial State: {GetCurrentStateName()}");
        Console.ResetColor();
    }

    // This method is called by the ConcreteState objects to change the Context's state.
    public void ChangeState(TCPState s)
    {
        _state = s;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[STATE TRANSITION] Current State is now: {GetCurrentStateName()}");
        Console.ResetColor();
    }

    public string GetCurrentStateName()
    {
        return _state.GetType().Name;
    }

    // Client operations, all delegated to the current state object.
    public void ActiveOpen() { _state.ActiveOpen(this); }
    public void PassiveOpen() { _state.PassiveOpen(this); }
    public void Close() { _state.Close(this); }
    public void Send() { _state.Send(this); }
}

// Client Code to demonstrate the pattern
public class Program
{
    public static void Main(string[] args)
    {
        // Create the connection object (Context)
        TCPConnection connection = new TCPConnection();

        Console.WriteLine("\n===================================");
        Console.WriteLine("      Scenario 1: Active Open");
        Console.WriteLine("===================================");
        
        // 1. Start in Closed state
        connection.ActiveOpen(); // CLOSED -> ESTABLISHED
        
        // 2. Behavior changes in Established state
        connection.Send();       // Successful data transmission
        
        // 3. Transition to Listen state
        connection.Close();      // ESTABLISHED -> LISTEN
        
        
        Console.WriteLine("\n===================================");
        Console.WriteLine("      Scenario 2: Incoming Request");
        Console.WriteLine("===================================");

        // 4. In Listen state, 'Send' (simulating receiving a client SYN) causes a transition
        connection.Send();       // LISTEN -> ESTABLISHED
        
        
        Console.WriteLine("\n===================================");
        Console.WriteLine("      Scenario 3: Invalid Action");
        Console.WriteLine("===================================");

        // 5. ActiveOpen is not valid in Established state
        connection.ActiveOpen(); // Invalid action, behavior defined in TCPEstablished (inherits from TCPState default)

        // 6. Finally, close the connection
        connection.Close();      // ESTABLISHED -> LISTEN
    }
}
