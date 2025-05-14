using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem> CreateAsync(TodoItem todo);
    Task<IEnumerable<TodoItem>> GetExpiredAsync();
    Task<IEnumerable<TodoItem>> GetActualAsync(int daysToExpire);
    
    Task<TodoItem?> UpdateAsync(int id, TodoItem updatedToDo);
    Task<int?> DeleteAsync(int id);
}

public class TodoService : ITodoService
{
    private readonly TodoContext _context;

    public TodoService(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await _context.TodoItems.OrderByDescending(t => t.DateCreated).ToListAsync();
    }

    public async Task<TodoItem> CreateAsync(TodoItem todo)
    {
        var newTodo  = todo with { DateCreated = DateTime.UtcNow };
        _context.TodoItems.Add(newTodo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task<IEnumerable<TodoItem>> GetExpiredAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.TodoItems
            .Where(t => t.IsCompleted || t.DaysToExpire <= 0)
            .OrderByDescending(t => t.DateCreated)
            .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetActualAsync(int daysToExpire)
    {
        return await _context.TodoItems
            .Where(t => !t.IsCompleted && t.DaysToExpire >= daysToExpire)
            .OrderByDescending(t => t.DateCreated)
            .ToListAsync();
    }


    public async Task<TodoItem?> UpdateAsync(int id, TodoItem updatedToDo)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null) return null;
        var updatedTodoItem = todo with
        {
            Title = updatedToDo.Title,
            DaysToExpire = updatedToDo.DaysToExpire,
            IsCompleted = updatedToDo.IsCompleted
        };
        _context.Entry(todo).CurrentValues.SetValues(updatedTodoItem);
        await _context.SaveChangesAsync();
        return todo;
    }
    public async Task<int?> DeleteAsync(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null) return null;
        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();
        return id;
    }
} 