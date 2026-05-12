using PracticeWeb.DTOs;
using PracticeWeb.Model;
namespace PracticeWeb.Interfaces
{
    public interface IAccountServices
    {
        Task CreateAccount(AccountCredentials dto);
        Task<string?> Login(AccountCredentials dto);
        
    }

    public interface IAccountRepository
    {
        Task AddAsync(User data);

        Task<User?>UserAsync(string name);
        Task<bool> IsUserExisting(string name);
    }
}