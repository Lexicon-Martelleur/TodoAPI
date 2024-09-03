using Microsoft.EntityFrameworkCore;
using TodoAPI.Constants;
using TodoAPI.DbContext.Contexts;
using TodoAPI.DbContext.Seeds;
using TodoAPI.Lib;

namespace TodoAPI.Extensions;

internal static class WebApplicationExtension
{
    internal static async Task UseDataSeedAsyncExtension(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<TodoContext>();
        var securitySerivce = serviceProvider.GetRequiredService<ISecurityService>();


        await context.Database.MigrateAsync();
        await SeedTodoDB.RunAsync(context, securitySerivce);
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

    internal static void UseSwaggerExtension(this WebApplication application)
    {
        application.UseSwagger();
        application.UseSwaggerUI(setupAction =>
        {
            var descriptions = application.DescribeApiVersions();
            foreach (var description in descriptions)
            {
                setupAction.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }
}
