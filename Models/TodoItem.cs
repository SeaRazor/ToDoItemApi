namespace ToDoApi.Models;

public record TodoItem(int Id, string Title, bool IsCompleted, int DaysToExpire, DateTime DateCreated); 