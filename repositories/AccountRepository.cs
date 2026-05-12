using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PracticeWeb.DataBase;
using PracticeWeb.DTOs;
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

    public  async Task<User?> UserAsync(string? name, int? UserUID)
    {
        if(name is not null)
            return  await _db.Users.FirstOrDefaultAsync(u => u.Name == name);
        else if(UserUID.HasValue)
            
            return await _db.Users.FindAsync(UserUID);
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