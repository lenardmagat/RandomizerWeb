using PracticeWeb.ErrorHandling;
using PracticeWeb.Model;
namespace PracticeWeb.Repository;

using System.Reflection.Metadata.Ecma335;

using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using PracticeWeb.DataBase;
using PracticeWeb.Interface;

public class GroupRepository : IGroupRepository
{
    private readonly DbManager _db;
    public GroupRepository(DbManager db) => _db = db;
    public async Task<Result> AddMemberAsync(List<Member> dto)
    {
        await _db.Members.AddRangeAsync(dto);
        return Result.Success();
    }
    public async Task<Result<List<Member>>> GetMembersAsync(int UserUid)
    {
        List<Member> members = await _db.Members
                                    .Where(u => u.UserId == UserUid)
                                    .OrderBy(u => u.Name)
                                    .ToListAsync();
        if(members is null) 
            return Result<List<Member>>.Failure("No fetched data from given UserId", 405);
        return Result<List<Member>>.Success(members);
    }
    public async Task<Result> SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
        return Result.Success();
    }
    public async Task<Result<int>> SaveGroupAndMembers(Group group, List<GroupMember> groupMembers)
    {
        // await _db.Groups.AddAsync(group);
        await _db.GroupMembers.AddRangeAsync(groupMembers);
        var SaveChangesresult = await _db.SaveChangesAsync();
        return Result<int>.Success(groupMembers[0].GroupId);
    }
    public async Task<Result<Group>> FindGroupasync(int GroupId)
    {
        Group? group = await _db.Groups.FindAsync(GroupId);
        if(group is null) return Result<Group>.Failure("Can't fetch data into given credentials", 405);
        return Result<Group>.Success(group);
    }
    public async Task<Result<List<GroupMember>>> FindMembersasync(int GroupId)
    {
        List<GroupMember>? groupMembers = await _db.GroupMembers.Where(m => m.GroupId == GroupId).ToListAsync();
        if(groupMembers is null) return Result<List<GroupMember>>.Failure("Can't fetch data into given credentials", 405); 
        return Result<List<GroupMember>>.Success(groupMembers);
    }
    public async Task<Result<List<Group>>> FindGroupsData(int UserUid)
    {
        var GroupsData = await _db.Groups.Where(u => u.UserId == UserUid).ToListAsync();
        if(GroupsData is null) return Result<List<Group>>.Failure("Can't fetch data on given credentials", 404);
        return Result<List<Group>>.Success(GroupsData);
    }
}