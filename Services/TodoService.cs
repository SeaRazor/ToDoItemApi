using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem> CreateAsync(TodoItem todo);
    Task<IEnumerable<TodoItem>> GetExpiredAsync();
    Task<IEnumerable<TodoItem>> GetByDaysToExpireAsync(int daysToExpire);
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
        return await _context.TodoItems.ToListAsync();
    }

    public async Task<TodoItem> CreateAsync(TodoItem todo)
    {
        _context.TodoItems.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task<IEnumerable<TodoItem>> GetExpiredAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.TodoItems
            .Where(t => !t.IsCompleted && t.DaysToExpire <= 0)
            .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetByDaysToExpireAsync(int daysToExpire)
    {
        return await _context.TodoItems
            .Where(t => t.DaysToExpire == daysToExpire)
            .ToListAsync();
    }
} 