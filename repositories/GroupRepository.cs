using PracticeWeb.Interfaces;
using PracticeWeb.Model;
namespace PracticeWeb.Repository;
using Microsoft.EntityFrameworkCore;
using PracticeWeb.DataBase;
using PracticeWeb.Interface;

public class GroupRepository : IGroupRepository
{
    private readonly DbManager _db;
    public GroupRepository(DbManager db) => _db = db;
    public async Task AddMemberAsync(List<Member> dto)
    {
        await _db.Members.AddRangeAsync(dto);
        return;
    }
    public async Task<List<Member>> GetMembersAsync(int UserUid)
    {
        List<Member> members = await _db.Members.Where(u => u.UserId == UserUid).OrderBy(u => u.Name).ToListAsync();
        return members;
    }
    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}