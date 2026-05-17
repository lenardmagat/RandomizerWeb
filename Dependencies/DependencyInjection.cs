using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PracticeWeb.Interfaces;
using PracticeWeb.Repository;
using PracticeWeb.Services;
using PracticeWeb.DataBase;
using PracticeWeb.Interface;
using PracticeWeb.core;
using HashidsNet;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace PracticeWeb.Configuration
{
    public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString, IConfiguration configuration)//string connectionString
    {
        services.AddDbContext<DbManager>(options => options.UseNpgsql(connectionString)); //connectionString
        services.AddScoped<IAccountServices, AccountServices>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IGroupService, GroupServices>();
        services.AddSingleton<IHashids>(_ => new Hashids(configuration["GroupSecretKey"], 8));
        services.AddSingleton<IHasher>(sp =>
        {   var hashids = sp.GetRequiredService<IHashids>();
            string? keystring = configuration[""] ?? throw new InvalidConfigurationException("JWT key string is missing.");
            string? issuer = configuration[""] ?? throw new InvalidConfigurationException("Issuer key string is missing.");
            string? audience = configuration[""] ?? throw new InvalidConfigurationException("Audience key string is missing.");
            return new Security(hashids, keystring, issuer, audience);      
        }
        );
        
        return services;
    }
}
}