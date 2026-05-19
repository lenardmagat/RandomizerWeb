using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PracticeWeb.DataBase;
using PracticeWeb.DTOs;
using PracticeWeb.ErrorHandling;
using PracticeWeb.Interfaces;
using PracticeWeb.Model;
namespace PracticeWeb.Repository;
public class AccountRepository : IAccountRepository
{
    private readonly DbManager _db;
    public AccountRepository(DbManager db) => _db = db;
     
    public async Task AddAsync(User dto){
        await _db.Users.AddAsync(dto);
        await _db.SaveChangesAsync();
    }

    public  async Task<Result<User?>> UserAsync(string? name, int? UserUID)
    {
        if(name is not null)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Name == name);
            if(user is null) return Result<User?>.Failure("Username can't be found.", 404);
            return  Result<User?>.Success(user);
            }
        else if(UserUID.HasValue)
        {
            User? user = await _db.Users.FindAsync(UserUID);
            if(user is null) return Result<User?>.Failure("UserId can't be found.", 401);
            return Result<User?>.Success(user);
            }
        throw new InvalidOperationException("Credential cannot be found.");
    }
    public async Task<bool> IsUserExisting(string name)
    {
        if(await _db.Users.FirstOrDefaultAsync(u => u.Name == name) is not null) return true; else return false;
    }
    public async Task SavechangesAsync()
    {
        await _db.SaveChangesAsync();
    }
} 