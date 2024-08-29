using Microsoft.EntityFrameworkCore;
using Serilog;
using TodoAPI.Constants;
using TodoAPI.DBContext;
using TodoAPI.Extensions;
using TodoAPI.Models.Repositories;
using TodoAPI.Models.Services;

namespace TodoAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var app = CreateWebApplication(args);
        ConfigureWebApplicationPipeline(app);
    }

    private static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddCustomLoggingExtension();
        builder.AddGlobalErrorHandlingExtension();
        builder.AddAndConfigureControllersExtension();
        builder.AddSwaggerServiceExtension();
        builder.AddDBContextExtension();
        builder.AddServicesExtension();
        builder.AddCORSPolicyExtension();
        return builder.Build();
    }

    private static void ConfigureWebApplicationPipeline(WebApplication app)
    {
        app.TestDBConnectionExtension();
        app.UseCORSPolicyExtension();
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
}
