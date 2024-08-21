using Microsoft.EntityFrameworkCore;
using Serilog;
using TodoAPI.DBContext;
using TodoAPI.Models.Repositories;
using TodoAPI.Models.Services;

namespace TodoAPI;

public class Program
{
    private static readonly (string Dev, string Prod) CorsPolicies = (
        Dev: "DevCorsPolicy",
        Prod: "ProdCorsPolicy"
    );
    
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/todoinfo.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);
        
        builder.Host.UseSerilog();

        builder.Services.AddProblemDetails();

        builder.Services.AddControllers();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen();

        AddAppServices(builder);

        AddCorsPolicy(builder);
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        UseCorsPolicy(app);

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

    private static void AddAppServices (WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddScoped<ITodoService, TodoService>();
        builder.Services.AddScoped<ITodoRepository, TodoRepository>();

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine($"Connection String: {connectionString}");
        }
        builder.Services.AddDbContext<TodoContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

    private static void AddCorsPolicy (WebApplicationBuilder builder)
    {

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicies.Dev, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            options.AddPolicy(CorsPolicies.Prod, builder =>
            {
                builder.WithOrigins("https://yourproductiondomain.com");
            });
        });
    }

    private static void UseCorsPolicy(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseCors(CorsPolicies.Dev);
        }
        else
        {
            app.UseCors(CorsPolicies.Prod);
        }
    }

}
