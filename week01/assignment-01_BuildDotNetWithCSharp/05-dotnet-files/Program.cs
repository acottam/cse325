using System.Text;
using System.Text.Json;

// Get the current directory
Console.WriteLine(Directory.GetCurrentDirectory());

// Get the path to the user's Documents folder
string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

// Combine the Documents folder path with a file name
Console.WriteLine(docPath);

// Combine the Documents folder path with a file name using Path.Combine
Console.WriteLine($"stores{Path.DirectorySeparatorChar}201");

// This is the recommended way to combine paths, as it will use the correct directory separator for the operating system
Console.WriteLine(Path.Combine("stores", "201")); // outputs: stores/201

// outputs: sales.json
Console.WriteLine(Path.GetExtension("sales.json")); // outputs: .json

// Create a FileInfo object to get detailed information about a file
string fileName = $"stores{Path.DirectorySeparatorChar}201{Path.DirectorySeparatorChar}sales{Path.DirectorySeparatorChar}sales.json";

// The FileInfo class provides properties and methods to get information about a file, such as its name, size, creation date, etc.
FileInfo info = new FileInfo(fileName);

// The FileInfo class provides properties and methods to get information about a file, such as its name, size, creation date, etc.
Console.WriteLine($"Full Name: {info.FullName}{Environment.NewLine}Directory: {info.Directory}{Environment.NewLine}Extension: {info.Extension}{Environment.NewLine}Create Date: {info.CreationTime}"); // And many more

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

GenerateSalesSummaryReport(salesFiles, salesTotalDir);

// This method will search for all files in the specified folder and its subfolders, and return a list of files that end with "sales.json"
IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        // The file name will contain the full path, so only check the end of it
        if (file.EndsWith("sales.json"))
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonSerializer.Deserialize<SalesData>(salesJson);
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

void GenerateSalesSummaryReport(IEnumerable<string> salesFiles, string outputDir)
{
    var sb = new StringBuilder();
    double grandTotal = 0;

    sb.AppendLine("Sales Summary");
    sb.AppendLine("----------------------------");

    var details = new List<string>();

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonSerializer.Deserialize<SalesData>(salesJson);
        double fileTotal = data?.Total ?? 0;
        grandTotal += fileTotal;
        details.Add($"  {Path.GetFileName(file)}: {fileTotal.ToString("C")}");
    }

    sb.AppendLine($" Total Sales: {grandTotal.ToString("C")}");
    sb.AppendLine();
    sb.AppendLine(" Details:");
    foreach (var detail in details)
    {
        sb.AppendLine(detail);
    }

    File.WriteAllText(Path.Combine(outputDir, "salesSummaryReport.txt"), sb.ToString());
    Console.WriteLine(sb.ToString());
}

record SalesData (double Total);