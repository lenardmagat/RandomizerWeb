using PracticeWeb.DataBase;
using PracticeWeb.configuration;
using Microsoft.EntityFrameworkCore;
using Serilog;
var app = Configurations.webApplication();

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
                const MY_JWT_TOKEN = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzIiwianRpIjoiY2E0OWIwODktMDJlOC00YTgyLWFiOTItNWJmZmVjZmNjODAxIiwibmJmIjoxNzc4NTg4MzM3LCJleHAiOjE3Nzg1OTU1MzcsImlhdCI6MTc3ODU4ODMzNywiaXNzIjoiUHJhY3RpY2VXZWIiLCJhdWQiOiJQcmFjdGljZVdlYlVzZXJzIn0.RBTQ92TaT1UCXvO6f7C8lOMUed85cnzNPPYeGej6V-4';
                
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
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DbManager>();
        context.Database.Migrate(); 
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database tables.");
    }
}
app.Run();