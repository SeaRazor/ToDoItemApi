using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;

namespace ToDoApi.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTodoContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TodoContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
        await context.Database.EnsureCreatedAsync();
    }
} 