using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Data.Extensions;
using ToDoApi.Endpoints;
using ToDoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTodoContext(builder.Configuration);
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:5000",
                "http://localhost:5173");
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();



        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Ensure the database is created and migrations are applied.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
    context.Database.EnsureCreated();
}

app.MapTodoEndpoints();
app.UseCors("MyAllowSpecificOrigins");

app.Run();


