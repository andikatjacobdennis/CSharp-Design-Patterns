using System;

// Abstract Class: Defines the Template Method (the algorithm skeleton) 
// and the primitive/hook operations.
abstract class ReportGenerator
{
    /// <summary>
    /// The Template Method. This method defines the invariant sequence of steps
    /// for the algorithm and is typically sealed or non-virtual to prevent subclasses
    /// from changing the algorithm's structure.
    /// </summary>
    public void GenerateReport()
    {
        Console.WriteLine("--- Starting Report Generation ---");

        // 1. Hook operation (optional step, subclasses may override)
        PreProcessData(); 
        
        // 2. Abstract primitive operation (mandatory step, customized by subclass)
        string data = FetchData(); 
        
        // 3. Concrete operation (invariant step, defined in abstract class)
        SortData(ref data); 
        
        // 4. Abstract primitive operation (mandatory step, customized by subclass)
        FormatDocument(data); 
        
        Console.WriteLine("--- Report Generation Finished ---\n");
    }

    // --- Primitive Operations (Mandatory Steps) ---
    // Declared as abstract; subclasses MUST provide an implementation.
    protected abstract string FetchData();
    protected abstract void FormatDocument(string data);

    // --- Hook Operation (Optional Step) ---
    // Declared as virtual; subclasses CAN override to inject custom logic.
    protected virtual void PreProcessData() 
    {
        // Default implementation does nothing.
        // PDFGenerator will override this, ExcelGenerator will not.
    }

    // --- Concrete Operation (Invariant Step) ---
    // This step is fixed and shared among all subclasses.
    private void SortData(ref string data)
    {
        Console.WriteLine("Invariant Step: Applying standard data sorting and cleaning.");
        data += " [SORTED]"; // Simple marker to show the step happened
    }
}

// Concrete Class 1: Implements the steps for a PDF Report.
class PDFReportGenerator : ReportGenerator
{
    protected override string FetchData()
    {
        Console.WriteLine("Primitive: Fetching data from the Primary Database for PDF generation.");
        return "Customer Sales Data";
    }

    protected override void FormatDocument(string data)
    {
        Console.WriteLine($"Primitive: Rendering '{data}' into a formatted PDF file.");
    }
    
    // Overrides the hook to provide specific pre-processing logic.
    protected override void PreProcessData()
    {
        Console.WriteLine("Hook Overridden: Setting up custom PDF encryption and watermarks.");
    }
}

// Concrete Class 2: Implements the steps for an Excel Report.
class ExcelReportGenerator : ReportGenerator
{
    protected override string FetchData()
    {
        Console.WriteLine("Primitive: Fetching data from the External API for Excel generation.");
        return "Quarterly Financial Metrics";
    }

    protected override void FormatDocument(string data)
    {
        Console.WriteLine($"Primitive: Exporting '{data}' into a clean Excel spreadsheet format.");
    }
    
    // Note: This subclass does NOT override PreProcessData, so the default empty 
    // implementation from the base class is used, skipping that step.
}

/// <summary>
/// Client class to demonstrate the Template Method pattern.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        // Generate the PDF Report
        Console.WriteLine("Client: Requesting a PDF Report generation.");
        ReportGenerator pdfReport = new PDFReportGenerator();
        pdfReport.GenerateReport();

        // Generate the Excel Report
        Console.WriteLine("Client: Requesting an Excel Report generation.");
        ReportGenerator excelReport = new ExcelReportGenerator();
        excelReport.GenerateReport();
    }
}
