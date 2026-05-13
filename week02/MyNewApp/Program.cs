//
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

// Create a new web application builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1"));

// Middleware to log incoming requests and outgoing responses
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next(context);
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});

// In-memory data store for todos
var todos = new List<Todo>();

// Get All /todos
app.MapGet("/todos", () => todos);

// GET /todos
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) =>
{
    var targetTodo = todos.SingleOrDefault(t => t.Id == id);

    return targetTodo is null
        ? TypedResults.NotFound()
        : TypedResults.Ok(targetTodo);
});

// POST /todos
app.MapPost("/todos", (Todo task) =>
{
    todos.Add(task);
    return TypedResults.Created("/todos/{Id}", task);
})
// Add an endpoint filter to validate the incoming Todo item
.AddEndpointFilter(async (context, next) => {
    var taskArgument = context.GetArgument<Todo>(0);
    var errors = new Dictionary<string, string[]>();
    
    if (taskArgument.DueDate < DateTime.UtcNow)
    {
        errors.Add(nameof(taskArgument.DueDate), ["Due date must be in the future."]);
    }
    
    if (taskArgument.IsComplete)
    {
        errors.Add(nameof(taskArgument.IsComplete), ["Cannot mark a task as complete if its due date is in the future."]);
    }

    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    return await next(context);
});

// PUT /todos
app.MapPut("/todos/{id}", Results<Ok<Todo>, NotFound> (int id, Todo updatedTodo) =>
{
    var targetTodo = todos.SingleOrDefault(t => t.Id == id);

    if (targetTodo is null)
    {
        return TypedResults.NotFound();
    }

    targetTodo = updatedTodo;
    return TypedResults.Ok(targetTodo);
});

// Delete /todos
app.MapDelete("/todos/{id}", (int id) =>
{
    todos.RemoveAll(t => id == t.Id);
    return TypedResults.NoContent();
});

// Run the app
app.Run();

// Record type for Todo items
public record Todo(int Id, string Title, DateTime DueDate, bool IsComplete = false);