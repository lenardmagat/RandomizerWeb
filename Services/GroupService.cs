using PracticeWeb.ErrorHandling;
using PracticeWeb.DTOs;
using PracticeWeb.Model;
using PracticeWeb.Interface;
using System.Runtime.InteropServices;
using System.Security.Authentication;
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
    public async Task<Result> AddMember(int UserUid, List<MemberDto> members)
    {
        Result<List<Member>> ExistingMembers = await _groupRepository.GetMembersAsync(UserUid);
        if(!ExistingMembers.IsSuccess || ExistingMembers.Value is null) return Result.Failure(ExistingMembers.Error 
                                                                        ?? "Cannot fetch data from given credentials",
                                                                        ExistingMembers.StatusCode);
        List<Member> newMembers = new();
        var IncomingMembers = members.Select(u => u.Name).ToHashSet();
        foreach(var existingMember in ExistingMembers.Value)
        {
            if (!IncomingMembers.Contains(existingMember.Name))
            {
                existingMember.Status = "Inactive";
            }
        }
        var ExistingNames = ExistingMembers.Value.Select(u => u.Name).ToHashSet();
        foreach(var dto in members)
        {
            if (!ExistingNames.Contains(dto.Name))
            {
                newMembers.Add(new Member{Name = dto.Name, UserId = UserUid, Status = "Active"});
            }
        }
        var resultAddMembers = await _groupRepository.AddMemberAsync(newMembers);
        var resultSaveChanges = await _groupRepository.SaveChangesAsync();
        if(!resultAddMembers.IsSuccess || !resultSaveChanges.IsSuccess) return Result.Failure("Failed to add members", 205);
        return Result.Success();
    }
    public async Task<GetGroupResponse> CreateGroup(int? Useruid, CreateGroupRequest dto)
    {
        int? UserUid = null;
        if(Useruid.HasValue) UserUid = Useruid;
        Group group = new Group{Name = dto.GroupName, Status = "Active", UserId = UserUid, NoOfGroups = dto.NumberOfGroups};
        int index =0;
        List<GroupMember> groupMembers = new();
        foreach(var member in dto.Members)
        {
            if(index == dto.NumberOfGroups) index = 0;
            index++;
            groupMembers.Add(new GroupMember{Name = member, Owner = group, GroupNumber = index});
        }
        int Groupid = await _groupRepository.SaveGroupAndMembers(group, groupMembers);
        var response = await GetGroupData(hashId: null,GroupId:Groupid);
        return response;
    }
    public async Task<GetGroupResponse> GetGroupData(string? hashId, int? GroupId)
    {
        int _GroupId;
        if(hashId != null) _GroupId = _Security.DecodeHashids(hashId);
        else if(GroupId.HasValue) _GroupId = GroupId.Value;
        else throw new InvalidCastException("No parameters");
        Group GroupData = await _groupRepository.FindGroupasync(_GroupId);
        if(GroupData.Status != "Active") throw new InvalidCredentialException("Invalid Credential");
        List<GroupMember> membersData = await _groupRepository.FindMembersasync(_GroupId);
        var members = new Dictionary<int, List<string>>();
        foreach(var member in membersData)
        {
            CollectionsMarshal.GetValueRefOrAddDefault(members, member.GroupId, out _) ??= new List<string>();
            members[member.GroupId].Add(member.Name);
        }
        var response = new GetGroupResponse(HashedId:_Security.CreateHashids(GroupData.GroupId),
                                            GroupName: GroupData.Name, 
                                            Owner: GroupData.Owner?.Name ?? "Anonymous", 
                                            NumberOfGroups: GroupData.NoOfGroups, 
                                            Members:members
                                             ); 
        return response;
    }
    public async Task<List<GetGroupsDataResponse>> GetGroupsData(int UserUid)
    {
        List<Group>? groups = await _groupRepository.FindGroupsData(UserUid) ?? throw new Exception("No Data to Process");
        
        List<GetGroupsDataResponse> response = new();
        foreach(var group in groups)
        {
            response.Add(new GetGroupsDataResponse(GroupName: group.Name, 
                                                    GroupHashedId: _Security.CreateHashids(group.GroupId),
                                                    NumberOfGroups: group.NoOfGroups));
        }
        return response;
    }

}