using PracticeWeb.Interfaces;
using PracticeWeb.DTOs;
using PracticeWeb.Model;
using PracticeWeb.Interface;
using System.Security.Authentication;
using PracticeWeb.ErrorHandling;
namespace PracticeWeb.Services;
public class AccountServices : IAccountServices
{  
    private IAccountRepository _repo;
    private IHasher _security;
    public AccountServices(IAccountRepository repo, IHasher security){
        _repo = repo;
        _security = security;
    }
    public async Task<Result> CreateAccount(AccountCredentials dto)
    {
        if(await _repo.IsUserExisting(dto.Name)) Result.Failure("Username already Exist.", 205);
        User newUser = new User {Name = dto.Name, HashedPassword = _security.HashPassword(dto.Password), status = "Active"};
        await _repo.AddAsync(newUser);
        return Result.Success(201);
    }
    public async Task<Result<string?>> Login(AccountCredentials dto)
    {
        Result<User?> user = await _repo.UserAsync(dto.Name);
        if(user.StatusCode == 404 || user.Value is null) 
            return Result<string?>.Failure(user.Error ?? "Can't found given credentials.", 404);
        if(!_security.VerifyPassword(dto.Password, user.Value.HashedPassword)) 
            return Result<string?>.Failure("Wrong password.", 401);
        string token = _security.CreateToken(user.Value.UserId);
        return Result<string?>.Success(token);
    }
    public async Task<Result> UpdateAccount(int userUid, ChangePasswordCredentials dto)
    {
        Result<User?> user = await _repo.UserAsync(UserUID:userUid);
        if(!user.IsSuccess ||  user.Value is null)
            return Result.Failure("Can't found given credentials", 404);
        if(!_security.VerifyPassword(dto.password, user.Value.HashedPassword)) throw new InvalidCredentialException("Wrong password.");
        user.Value.HashedPassword = _security.HashPassword(dto.NewPassword);
        await _repo.SavechangesAsync();
        return Result.Success();
    }
}   