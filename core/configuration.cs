using System.Security.Authentication;
using PracticeWeb.Configuration;
using PracticeWeb.Middleware;
using Serilog;   
namespace configuration
{
    class Configurations
    {
        static public WebApplication webApplication()
        {
            var builder = WebApplication.CreateBuilder();
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                        .CreateLogger();
            builder.Services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
            builder.Host.UseSerilog();
            var connection = builder.Configuration["JWT_KEY"] ?? throw new InvalidCredentialException("JWT Key is missing.");
            builder.Services.AddAuthentication(options =>
            {
                // These default schemes tell ASP.NET Core to look for a JWT token by default
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {  
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT_ISSUER"],    
                    ValidAudience = builder.Configuration["JWT_AUDIENCE"], 
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(connection)) 
                };
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            string? connectionString = builder.Configuration["DataBaseConnection"];
            if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("Data Base connection is missing");
            builder.Services.AddApplicationServices(connectionString, builder.Configuration);
            var app = builder.Build();
            return app;
            }
    }
}