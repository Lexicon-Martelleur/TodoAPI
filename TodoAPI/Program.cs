using Microsoft.EntityFrameworkCore;
using Serilog;
using TodoAPI.Data;
using TodoAPI.DBContext;

namespace TodoAPI;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/todoinfo.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddProblemDetails();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add store
        // Configure the HTTP request pipeline.
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }
        builder.Services.AddSingleton<TodoStore>(); // In Memory
        

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine($"Connection String: {connectionString}");
        }
        builder.Services.AddDbContext<TodoContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
            app.UseExceptionHandler();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
