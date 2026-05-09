# W01 Assignment: Build .NET Applications with C# - Notes

## Part 1: Web API Evidence (Create a web API with ASP.NET Core controllers)

**Controller file:** `06-build-web-api-aspnet-core/Controllers/PizzaController.cs`  
**Service file:** `06-build-web-api-aspnet-core/Services/PizzaService.cs`  
**Model file:** `06-build-web-api-aspnet-core/Models/Pizza.cs`

### Existing Content + Additional Record

The API starts with 3 default pizzas seeded in `PizzaService.cs`:

```
GET /pizza → 200 OK
[
  {"id":1,"name":"Classic Italian","isGlutenFree":false},
  {"id":2,"name":"Veggie","isGlutenFree":true},
  {"id":3,"name":"Pepperoni","isGlutenFree":false}
]
```

After adding a new pizza via POST:

```
POST /pizza → 201 Created
Request Body: {"name": "Hawaii", "isGlutenFree": false}
Response Body: {"id":4,"name":"Hawaii","isGlutenFree":false}
```

GET all pizzas now returns the additional record:

```
GET /pizza → 200 OK
[
  {"id":1,"name":"Classic Italian","isGlutenFree":false},
  {"id":2,"name":"Veggie","isGlutenFree":true},
  {"id":3,"name":"Pepperoni","isGlutenFree":false},
  {"id":4,"name":"Hawaii","isGlutenFree":false}
]
```

### Additional API Operations

```
PUT /pizza/4 → 204 No Content
Request Body: {"id":4,"name":"Pepperoni Deluxe","isGlutenFree":false}

DELETE /pizza/4 → 204 No Content
```

## Part 2: Sales Summary Function

**Source file:** `05-dotnet-files/Program.cs`

```csharp
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
```
