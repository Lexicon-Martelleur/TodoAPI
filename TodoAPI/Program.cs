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
        var app = CreateWebApplication(args);
        ConfigureWebApplicationPipeline(app);
    }

    private static WebApplication CreateWebApplication (string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        AddCustomLogging(builder);

        AddCustomGlobalErrorHandling(builder);

        AddAndConfigureAppControllers(builder);

        AddSwaggerService(builder);

        AddAppServices(builder);

        AddCorsPolicy(builder);

        return builder.Build();
    }

    private static void ConfigureWebApplicationPipeline(WebApplication app)
    {
        UseCorsPolicy(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
            app.UseExceptionHandler();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void AddCustomLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/todoinfo.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    private static void AddCustomGlobalErrorHandling(WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                if (context.Exception is NotImplementedException)
                {
                    context.ProblemDetails.Status = StatusCodes.Status503ServiceUnavailable;
                    context.ProblemDetails.Title = "Resource not implemented";
                    context.ProblemDetails.Detail = context.Exception.Message;
                }
                else if (context.Exception != null)
                {
                    context.ProblemDetails.Status = StatusCodes.Status500InternalServerError;
                    context.ProblemDetails.Title = "An unexpected error occurred";
                    context.ProblemDetails.Detail = "Please try again later.";
                }
            };
        });
    }

    private static void AddAndConfigureAppControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            // If client explicit define format in accept header
            // // then respond with 406 Not Acceptable if server not support format.
            options.ReturnHttpNotAcceptable = true;
        })
            .AddNewtonsoftJson() // Instead of System.Text.Json, used to support patch request
            .AddXmlDataContractSerializerFormatters(); // Used to support XML format.
    }

    private static void AddSwaggerService(WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();
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
            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
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
