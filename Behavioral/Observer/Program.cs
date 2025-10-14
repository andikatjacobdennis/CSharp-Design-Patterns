using System;
using System.Collections.Generic;
using System.Threading;

// 1. Core Interfaces

/// <summary>
/// The Observer interface declares the update method, which is used by subjects.
/// T is the type of the subject being observed.
/// </summary>
public interface IObserver<T>
{
    /// <summary>
    /// Receives an update from the subject. The subject passes itself to the observer (Pull Model).
    /// </summary>
    /// <param name="subject">The subject that changed state.</param>
    void Update(T subject);
}

/// <summary>
/// The Observable (Subject) interface declares methods for managing observers.
/// T is the type of the concrete subject.
/// </summary>
public interface IObservable<T>
{
    /// <summary>
    /// Attaches an observer to the subject.
    /// </summary>
    void Attach(IObserver<T> observer);
    
    /// <summary>
    /// Detaches an observer from the subject.
    /// </summary>
    void Detach(IObserver<T> observer);
    
    /// <summary>
    /// Notifies all attached observers about an event.
    /// </summary>
    void NotifyObservers();
}

// 2. Concrete Subject

/// <summary>
/// The Concrete Subject (ClockTimer) stores the state of interest and notifies observers upon state change.
/// </summary>
public class ClockTimer : IObservable<ClockTimer>
{
    private List<IObserver<ClockTimer>> _observers = new List<IObserver<ClockTimer>>();
    private int _hour, _minute, _second;

    public ClockTimer()
    {
        // Initialize time
        _hour = 0;
        _minute = 0;
        _second = 0;
    }

    // Public methods for Observers to query (Pull Model)
    public int GetHour() => _hour;
    public int GetMinute() => _minute;
    public int GetSecond() => _second;

    // Observer management methods
    public void Attach(IObserver<ClockTimer> observer)
    {
        if (observer != null && !_observers.Contains(observer))
        {
            _observers.Add(observer);
            Console.WriteLine($"[ClockTimer]: Observer attached successfully.");
        }
    }

    public void Detach(IObserver<ClockTimer> observer)
    {
        if (_observers.Remove(observer))
        {
            Console.WriteLine($"[ClockTimer]: Observer detached successfully.");
        }
    }

    public void NotifyObservers()
    {
        // Iterates over a copy to prevent issues if an observer detaches itself during the update
        var observersCopy = new List<IObserver<ClockTimer>>(_observers);
        foreach (var observer in observersCopy)
        {
            observer.Update(this);
        }
    }

    /// <summary>
    /// Simulates the internal time update and triggers notification.
    /// This demonstrates the "subject calls Notify after state change" implementation approach.
    /// </summary>
    public void Tick()
    {
        // Advance time state
        _second++;
        if (_second >= 60)
        {
            _second = 0;
            _minute++;
            if (_minute >= 60)
            {
                _minute = 0;
                _hour = (_hour + 1) % 24;
            }
        }
        
        // Ensure state is self-consistent before notification
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n--- ClockTimer State Changed to: {_hour:D2}:{_minute:D2}:{_second:D2} ---");
        Console.ResetColor();
        
        // Notify all attached observers
        NotifyObservers();
    }
}

// 3. Concrete Observers

/// <summary>
/// Displays the time in a HH:MM:SS format.
/// </summary>
public class DigitalClock : IObserver<ClockTimer>
{
    private ClockTimer _subject;

    public DigitalClock(ClockTimer subject)
    {
        _subject = subject;
        _subject.Attach(this);
    }
    
    /// <summary>
    /// Implements the update logic for the digital clock.
    /// It queries the subject for the current state.
    /// </summary>
    public void Update(ClockTimer subject)
    {
        // Check if the subject is the one we are interested in (important for observing multiple subjects)
        if (subject == _subject) 
        {
            int hour = subject.GetHour();
            int minute = subject.GetMinute();
            int second = subject.GetSecond();
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[DigitalClock]: Displaying Time: {hour:D2}:{minute:D2}:{second:D2}");
            Console.ResetColor();
        }
    }
}

/// <summary>
/// Simulates updating an analog clock display.
/// </summary>
public class AnalogClock : IObserver<ClockTimer>
{
    private ClockTimer _subject;

    public AnalogClock(ClockTimer subject)
    {
        _subject = subject;
        _subject.Attach(this);
    }

    /// <summary>
    /// Implements the update logic for the analog clock.
    /// </summary>
    public void Update(ClockTimer subject)
    {
        if (subject == _subject)
        {
            int minute = subject.GetMinute();
            int second = subject.GetSecond();
            
            // Calculation for analog clock hand position (6 degrees per minute/second)
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[AnalogClock]: Drawing Hands: Minute hand at {minute * 6} degrees, Second hand at {second * 6} degrees.");
            Console.ResetColor();
        }
    }
}

// 4. Client Program
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Observer Pattern Demonstration (Clock Example) ---");
        
        // 1. Create the Concrete Subject
        ClockTimer timer = new ClockTimer();

        // 2. Create and attach two Concrete Observers
        DigitalClock digitalClock = new DigitalClock(timer);
        AnalogClock analogClock = new AnalogClock(timer);

        Console.WriteLine("\nSetup Complete. Observers are attached to the ClockTimer.");

        // 3. Simulate state changes (ticks)
        Console.WriteLine("\n--- TICK 1 ---");
        timer.Tick();
        
        Console.WriteLine("\n--- TICK 2 ---");
        timer.Tick();
        
        // 4. Detach one observer
        Console.WriteLine("\n--- Detaching AnalogClock ---");
        timer.Detach(analogClock);

        // 5. Simulate another state change (only DigitalClock will respond)
        Console.WriteLine("\n--- TICK 3 (AnalogClock should not update) ---");
        timer.Tick();
    }
}
