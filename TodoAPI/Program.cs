using TodoAPI.Extensions;

namespace TodoAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = CreateWebApplication(args);
        await ConfigureWebApplicationPipeline(app);
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
        
        builder.AddAuthenticationExtension();

        builder.AddAuthorizationExtension();
        
        return builder.Build();
    }

    private static async Task ConfigureWebApplicationPipeline(WebApplication app)
    {
        app.TestDBConnectionExtension();
        
        app.UseCORSPolicyExtension();
        
        if (app.Environment.IsDevelopment())
        {
            await app.UseDataSeedAsyncExtension();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
            app.UseExceptionHandler();
        }

        app.UseAuthentication();
        
        app.UseAuthorization();     

        app.MapControllers();
        
        app.Run();
    }
}
