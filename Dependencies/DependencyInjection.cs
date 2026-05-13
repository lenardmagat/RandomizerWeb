using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PracticeWeb.Interfaces;
using PracticeWeb.Repository;
using PracticeWeb.Services;
using PracticeWeb.DataBase;
using PracticeWeb.Interface;
using PracticeWeb.core;
using Npgsql;
using HashidsNet;

namespace PracticeWeb.Configuration
{
    public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString, IConfiguration configuration)//string connectionString
    {
        DotNetEnv.Env.Load();
        services.AddDbContext<DbManager>(options => options.UseNpgsql(connectionString)); //connectionString
        services.AddScoped<IAccountServices, AccountServices>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IGroupService, GroupServices>();
        services.AddSingleton<IHasher, Security>();
        services.AddSingleton<IHashids>(_ => new Hashids(configuration["GroupSecretKey"], 8));
        return services;
    }
}
}