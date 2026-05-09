# W01 Assignment: Build .NET Applications with C# - Notes

## Part 1: Web API Evidence (Create a web API with ASP.NET Core controllers)

**Controller file:** `week01/assignment-01_BuildDotNetWithCSharp/06-build-web-api-aspnet-core/Controllers/PizzaController.cs`  
**Service file:** `week01/assignment-01_BuildDotNetWithCSharp/06-build-web-api-aspnet-core/Services/PizzaService.cs`  
**Model file:** `week01/assignment-01_BuildDotNetWithCSharp/06-build-web-api-aspnet-core/Models/Pizza.cs`

### Existing Content + Additional Record

The API starts with 4 pizzas seeded in `PizzaService.cs` (the 4th is the additional record):

```
GET /pizza → 200 OK
[
  {"id":1,"name":"Classic Italian","isGlutenFree":false},
  {"id":2,"name":"Veggie","isGlutenFree":true},
  {"id":3,"name":"Pepperoni","isGlutenFree":false},
  {"id":4,"name":"Hawaiian","isGlutenFree":false}
]
```

### API Verification (Request/Response Examples)

#### GET /pizza — 200 OK
```
GET http://localhost:5238/pizza/
Response: 200 OK
Body: [{"id":1,"name":"Classic Italian","isGlutenFree":false},{"id":2,"name":"Veggie","isGlutenFree":true},{"id":3,"name":"Pepperoni","isGlutenFree":false},{"id":4,"name":"Hawaiian","isGlutenFree":false}]
```

#### POST /pizza — 201 Created
```
POST http://localhost:5238/pizza/
Content-Type: application/json
Body: {"name": "Buffalo Chicken", "isGlutenFree": false}

Response: 201 Created
Body: {"id":5,"name":"Buffalo Chicken","isGlutenFree":false}
```

#### PUT /pizza/5 — 204 No Content
```
PUT http://localhost:5238/pizza/5
Content-Type: application/json
Body: {"id": 5, "name": "Buffalo Chicken Deluxe", "isGlutenFree": false}

Response: 204 No Content
```

#### DELETE /pizza/5 — 204 No Content
```
DELETE http://localhost:5238/pizza/5

Response: 204 No Content
```

## Part 2: Sales Summary Function

**Source file:** `week01/assignment-01_BuildDotNetWithCSharp/05-dotnet-files/Program.cs`

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
        var relativePath = Path.GetRelativePath(storesDirectory, file);
        details.Add($"  {relativePath}: {fileTotal.ToString("C")}");
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

### Output:
```
Sales Summary
----------------------------
 Total Sales: $2,012.20

 Details:
  sales.json: $88.88
  204/sales.json: $88.88
  203/sales.json: $99.00
  202/sales.json: $1,234.22
  201/sales.json: $501.22
```
