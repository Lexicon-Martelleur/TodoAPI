using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Constants;
using TodoAPI.DBContext;
using TodoAPI.Entities;

namespace TodoAPI.Extensions;

internal static class WebApplicationExtension
{
    public static async Task UseDataSeedAsyncExtension(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<TodoContext>();
        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<UserAuthenticationEntity>>();
        await context.Database.MigrateAsync();
        await SeedTodoDB.RunAsync(context, passwordHasher);
    }

    internal static void TestDBConnectionExtension(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
        try
        {
            dbContext.Database.OpenConnection();
            dbContext.Database.CloseConnection();
            Console.WriteLine("Successfully connected to the database.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to connect to the database: {e.Message}");
        }
    }

    internal static void UseCORSPolicyExtension(this WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseCors(ApplicationConfiguration.CorsPolicies.Dev);
        }
        else
        {
            application.UseCors(ApplicationConfiguration.CorsPolicies.Prod);
        }
    }
}
