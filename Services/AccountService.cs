using PracticeWeb.Interfaces;
using PracticeWeb.DTOs;
using PracticeWeb.Model;
using PracticeWeb.Interface;
using System.Security.Authentication;
namespace PracticeWeb.Services;
public class AccountServices : IAccountServices
{  
    private IAccountRepository _repo;
    private IHasher _security;
    public AccountServices(IAccountRepository repo, IHasher security){
        _repo = repo;
        _security = security;
    }
    public async Task CreateAccount(AccountCredentials dto)
    {
        if(await _repo.IsUserExisting(dto.Name)) throw new InvalidOperationException("Username already exist.");
        User newUser = new User {Name = dto.Name, HashedPassword = _security.HashPassword(dto.Password), status = "Active"};
        await _repo.AddAsync(newUser);
    }
    public async Task<string?> Login(AccountCredentials dto)
    {
        User? user = await _repo.UserAsync(dto.Name);
        if(user is null) throw new InvalidOperationException("Username is not existing.");
        if(!_security.VerifyPassword(dto.Password, user.HashedPassword)) throw new InvalidOperationException("Wrong Password");
        string token = _security.CreateToken(user.UserId);
        return token;
    }
    public async Task UpdateAccount(int userUid, ChangePasswordCredentials dto)
    {
        User? user = await _repo.UserAsync(UserUID:userUid) ?? throw new InvalidOperationException("Wrong Password");
        if(!_security.VerifyPassword(dto.password, user.HashedPassword)) throw new InvalidCredentialException("Wrong password.");
        user.HashedPassword = _security.HashPassword(dto.NewPassword);
        await _repo.SavechangesAsync();
        return;
    }
}   