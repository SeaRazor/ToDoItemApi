using ToDoApi.Models;
using ToDoApi.Services;

namespace ToDoApi.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/todos", async (ITodoService todoService, int? daysToExpire) => 
        {
            if (daysToExpire.HasValue)
            {
                return await todoService.GetByDaysToExpireAsync(daysToExpire.Value);
            }
            return await todoService.GetAllAsync();
        });

        app.MapGet("/todos/expired", async (ITodoService todoService) => 
            await todoService.GetExpiredAsync());

        app.MapPost("/todos", async (ITodoService todoService, TodoItem todo) => 
        {
            var createdTodo = await todoService.CreateAsync(todo);
            return Results.Created($"/todos/{createdTodo.Id}", createdTodo);
        });
    }
} 