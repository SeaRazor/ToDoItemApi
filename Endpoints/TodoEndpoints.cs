using Microsoft.AspNetCore.Mvc;
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
                return await todoService.GetActualAsync(daysToExpire.Value);
            }
            return await todoService.GetAllAsync();
        });

        app.MapGet("/todos/expired", async (ITodoService todoService) => 
            await todoService.GetExpiredAsync());
        
        app.MapGet("/todos/actual", async (ITodoService todoService, [FromQuery] int numberOfDays) => 
            await todoService.GetActualAsync(numberOfDays));

        app.MapPost("/todos", async (ITodoService todoService, TodoItem todo) => 
        {
            var createdTodo = await todoService.CreateAsync(todo);
            return Results.Created($"/todos/{createdTodo.Id}", createdTodo);
        });

        app.MapDelete("/todos/{id}", async (ITodoService todoService, int id) =>
        {
            var deletionResult = await todoService.DeleteAsync(id);
            return deletionResult is null ? Results.NotFound() : Results.Ok($"ToDo {id} was deleted successfully");
        });
        app.MapPut("/todos/{id}", async (ITodoService todoService, int id, [FromBody]TodoItem todo) =>
        {
            var updatedTodo = await todoService.UpdateAsync(id, todo);
            return updatedTodo is null ? Results.NotFound() : Results.Ok(updatedTodo);

        });

    }
} 