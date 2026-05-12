using PracticeWeb.Configuration;
using PracticeWeb.Middleware;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
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
    });
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();