using PracticeWeb.DTOs;
using PracticeWeb.Model;
using PracticeWeb.ErrorHandling;
namespace PracticeWeb.Interfaces
{
    public interface IAccountServices
    {
        Task<Result> CreateAccount(AccountCredentials dto);
        Task<Result<string?>> Login(AccountCredentials dto);
        Task<Result> UpdateAccount(int userUid, ChangePasswordCredentials dto);
        
    }

    public interface IAccountRepository
    {
        Task AddAsync(User data);
        Task<Result<User?>>UserAsync(string? Name = null, int? UserUID = null);
        Task<bool> IsUserExisting(string name);
        Task SavechangesAsync();
    }
}