using PracticeWeb.Interfaces;
using PracticeWeb.DTOs;
using PracticeWeb.Model;
using PracticeWeb.Interface;
using PracticeWeb.core;
namespace PracticeWeb.Services;
public class GroupServices : IGroupService
{
    private IHasher _Security;
    private IGroupRepository _groupRepository;
    public GroupServices(IGroupRepository groupRepository, IHasher security)
    {
        _Security = security;
        _groupRepository = groupRepository;
    }
    
    public async Task CreateGroup()
    {
        
    }
    public async Task AddMember(int UserUid, List<MemberDto> members)
    {
        List<Member> ExistingMembers = await _groupRepository.GetMembersAsync(UserUid);
        List<Member> newMembers = new();
        var IncomingMembers = members.Select(u => u.Name).ToHashSet();
        foreach(var existingMember in ExistingMembers)
        {
            if (!IncomingMembers.Contains(existingMember.Name))
            {
                existingMember.Status = "Inactive";
            }
        }
        var ExistingNames = ExistingMembers.Select(u => u.Name).ToHashSet();
        foreach(var dto in members)
        {
            if (!ExistingNames.Contains(dto.Name))
            {
                newMembers.Add(new Member{Name = dto.Name, UserId = UserUid, Status = "Active"});
            }
        }
        await _groupRepository.AddMemberAsync(newMembers);
        await _groupRepository.SaveChangesAsync();
        return;
    }

}