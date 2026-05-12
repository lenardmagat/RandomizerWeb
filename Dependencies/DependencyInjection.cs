using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PracticeWeb.Interfaces;
using PracticeWeb.Repository;
using PracticeWeb.Services;
using PracticeWeb.DataBase;
using PracticeWeb.Interface;
using PracticeWeb.core;
using Npgsql;

namespace PracticeWeb.Configuration
{
    public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString)//string connectionString
    {
        services.AddDbContext<DbManager>(options => options.UseNpgsql(connectionString)); //connectionString
        services.AddScoped<IAccountServices, AccountServices>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddSingleton<IHasher, Security>();
        return services;
    }
}
}