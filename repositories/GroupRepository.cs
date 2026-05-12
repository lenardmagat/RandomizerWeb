using PracticeWeb.Interfaces;
using PracticeWeb.Model;
namespace PracticeWeb.Repository;

using System.Security.Authentication;
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
    public async Task<int> SaveGroupAndMembers(Group group, List<GroupMember> groupMembers)
    {
        // await _db.Groups.AddAsync(group);
        await _db.GroupMembers.AddRangeAsync(groupMembers);
        await _db.SaveChangesAsync();
        return groupMembers[0].GroupId;
    }
    public async Task<Group> FindGroupasync(int GroupId)
    {
        Group? group = await _db.Groups.FindAsync(GroupId) ?? throw new InvalidCredentialException("Invalid Group Id.");
        return group;
    }
    public async Task<List<GroupMember>> FindMembersasync(int GroupId)
    {
        List<GroupMember>? groupMembers = await _db.GroupMembers.Where(m => m.GroupId == GroupId).ToListAsync()
        ?? throw new InvalidCredentialException("Invalid Group Id.");
        return groupMembers;
    }
    public async Task<List<Group>> FindGroupsData(int UserUid)
    {
        return await _db.Groups.Where(u => u.UserId == UserUid).ToListAsync();
    }
}