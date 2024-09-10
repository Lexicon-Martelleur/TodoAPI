using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using TodoAPI.Config;
using TodoAPI.Constants;
using TodoAPI.DbContext.Contexts;
using TodoAPI.DbContext.Repositories;
using TodoAPI.Entities;
using TodoAPI.Lib;
using TodoAPI.Models.Profiles;
using TodoAPI.Models.Repositories;
using TodoAPI.Models.Services;
using TodoAPI.Services;

namespace TodoAPI.Extensions;

internal static class WebApplicationBuilderExtension
{
    internal static void AddCustomLoggingExtension(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/todoinfo.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    internal static void AddGlobalErrorHandlingExtension(this WebApplicationBuilder builder)
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

    internal static void AddAndConfigureControllersExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            // If client explicit define format in accept header
            // then respond with 406 Not Acceptable if server not support format.
            options.ReturnHttpNotAcceptable = true;
        })
            .AddNewtonsoftJson() // Instead of System.Text.Json, used to support patch request
            .AddXmlDataContractSerializerFormatters(); // Used to support XML format.
    }
    
    internal static void AddDBContextExtension(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }

        var connectionString = builder.Configuration.GetConnectionString(
            "DefaultConnection"
        ) ?? throw new InvalidOperationException("Default connection string not found.");

        builder.Services.AddDbContext<TodoContext>(options =>
        {
            options.UseSqlServer(connectionString);
            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
    }

    internal static void AddServicesExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(configuration =>
        {
            configuration.AddProfile<TodoProfile>();
        });

        builder.Services.AddScoped<ITodoService, TodoService>();
        builder.Services.AddScoped<ITodoRepository, TodoRepository>();
        
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

        builder.Services.AddScoped<IClaimService, ClaimService>();

        builder.Services.AddScoped<ISecurityService, SecurityService>();

        builder.Services.AddScoped<
            IPasswordHasher<UserAuthenticationEntity>,
            PasswordHasher<UserAuthenticationEntity>>();
    }

    internal static void AddCORSPolicyExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(ApplicationConfiguration.CorsPolicies.Dev, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders(CustomHeader.Pagination);
            });

            options.AddPolicy(ApplicationConfiguration.CorsPolicies.Prod, builder =>
            {
                builder.WithOrigins("https://my-todo.org")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders(CustomHeader.Pagination);
            });
        });
    }

    internal static void AddAuthenticationExtension(this WebApplicationBuilder builder)
    {
        var secretByteArray = SecurityConfig.GetTokenSecret(builder.Configuration);
        builder.Services.AddAuthentication(SecurityConfig.AuthenticationType)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = SecurityConfig.GetTokenIssuer(builder.Configuration),
                    ValidAudience = SecurityConfig.GetTokenAudience(builder.Configuration),
                    IssuerSigningKey = new SymmetricSecurityKey(secretByteArray)
                };
            });
    }

    internal static void AddAuthorizationExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationRules.UserPolicy, policy => policy.RequireRole(AuthorizationRules.UserRole));
        });
    }

    internal static void AddApiVersioningExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(setupAction =>
        {
            setupAction.ReportApiVersions = true;
            setupAction.AssumeDefaultVersionWhenUnspecified = true;
            setupAction.DefaultApiVersion = new ApiVersion(
                API.MAJOR_VERSION_ONE, API.MINOR_VERSION_ONE);
        }).AddMvc().AddApiExplorer(setupAction =>
        {
            setupAction.SubstituteApiVersionInUrl = true;
        });

        AddOpenAPIExtension(builder);
    }

    /// <summary>
    /// Used to document the api using OpenAPI and Swagger.
    /// </summary>
    /// Should be called at the end of <see cref="AddApiVersioningExtension"/>
    /// <param name="builder"></param>
    private static void AddOpenAPIExtension(WebApplicationBuilder builder)
    {
        var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider()
            .GetRequiredService<IApiVersionDescriptionProvider>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(setupAction =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                setupAction.SwaggerDoc(
                    $"{description.GroupName}",
                    new()
                    {
                        Title = "Todo API",
                        Version = description.ApiVersion.ToString(),
                        Description = "An api to manage your daily todos."
                    });
            }

            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

            setupAction.IncludeXmlComments(xmlCommentsFullPath);

            setupAction.AddSecurityDefinition("TodoAPIBearerAuth", new()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "Set HTTP Authorization header to Bearer with a valid token"
            });

            setupAction.AddSecurityRequirement(new() 
            {
                {
                    new()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "TodoAPIBearerAuth"
                        }
                    },
                    new List<string>()
                }
            });
        });
    }
}
