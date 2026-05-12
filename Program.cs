using System.Runtime.InteropServices.Marshalling;
using System.Security.Authentication;
using PracticeWeb.Configuration;
using PracticeWeb.Middleware;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
DotNetEnv.Env.Load();
var connection = Environment.GetEnvironmentVariable("JWT_KEY");
if(connection is null) throw new InvalidCredentialException("JWT Key is missing.");
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
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),     // Replace with your actual issuer configuration
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"), // Replace with your actual audience configuration
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(connection)) // Replace with your secret key
    };
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
DotNetEnv.Env.Load();
string? connectionString = Environment.GetEnvironmentVariable("DataBaseConnection");
if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("Data Base connection is missing");
builder.Services.AddApplicationServices(connectionString); //connectionString
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
        options.RoutePrefix = string.Empty;
        options.HeadContent = @"
            <script>
                // Automatically attach your JWT token to every request made from this page
                // Just paste your token inside the quotes below!
                const MY_JWT_TOKEN = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzIiwianRpIjoiZTRiYTY5MzEtZGZkYy00YjI3LWI3NmUtY2E3NzEwZDJhYjA0IiwibmJmIjoxNzc4NTcwOTYyLCJleHAiOjE3Nzg1NzgxNjIsImlhdCI6MTc3ODU3MDk2MiwiaXNzIjoiUHJhY3RpY2VXZWIiLCJhdWQiOiJQcmFjdGljZVdlYlVzZXJzIn0.aIxEqOOSnAh4qpt-mX0N-VwQGIShAafuyw3wNFtrqiQ';
                
                if (MY_JWT_TOKEN && MY_JWT_TOKEN !== 'YOUR_JWT_TOKEN_HERE') {
                    const constantFetch = window.fetch;
                    window.fetch = async (...args) => {
                        args[1] = args[1] || {};
                        args[1].headers = args[1].headers || {};
                        args[1].headers['Authorization'] = `Bearer ${MY_JWT_TOKEN}`;
                        return constantFetch(...args);
                    };
                }
            </script>
        ";
    });
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();