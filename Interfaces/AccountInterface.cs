using PracticeWeb.DTOs;
using PracticeWeb.Model;
namespace PracticeWeb.Interfaces
{
    public interface IAccountServices
    {
        Task CreateAccount(AccountCredentials dto);
        Task<string?> Login(AccountCredentials dto);
        Task UpdateAccount(int userUid, ChangePasswordCredentials dto);
        
    }

    public interface IAccountRepository
    {
        Task AddAsync(User data);
        Task<User?>UserAsync(string? Name = null, int? UserUID = null);
        Task<bool> IsUserExisting(string name);
        Task SavechangesAsync();
    }
}