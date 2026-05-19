using PracticeWeb.ErrorHandling;
using PracticeWeb.DTOs;
using PracticeWeb.Model;
using PracticeWeb.Interface;
using System.Runtime.InteropServices;
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
        if(!ExistingMembers.IsSuccess || ExistingMembers.Value is null) 
            return Result.Failure(ExistingMembers.Error 
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
    public async Task<Result<GetGroupResponse>> CreateGroup(int? Useruid, CreateGroupRequest dto)
    {
        int? UserUid = null;
        if(Useruid.HasValue) UserUid = Useruid;
        Group group = new Group{Name = dto.GroupName, Status = "Active", UserId = UserUid, NoOfGroups = dto.NumberOfGroups};
        var members = dto.Members.ToList();
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(members));
        int index = 0;
        List<GroupMember> groupMembers = new();
        foreach(var member in members)
        {
            if(index == dto.NumberOfGroups) index = 0;
            index++;
            groupMembers.Add(new GroupMember{Name = member, Owner = group, GroupNumber = index});
        }
        var Groupid = await _groupRepository.SaveGroupAndMembers(group, groupMembers);
        var response = await GetGroupData(hashId: null,GroupId:Groupid.Value);
        if(!Groupid.IsSuccess || !response.IsSuccess) return Result<GetGroupResponse>.Failure("Can't process the data", 405);
        return response;
    }
    public async Task<Result<GetGroupResponse>> GetGroupData(string? hashId, int? GroupId)
    {
        int _GroupId;
        if(hashId != null) _GroupId = await Task.Run(() => _Security.DecodeHashids(hashId));
        else if(GroupId.HasValue) _GroupId = GroupId.Value;
        else return Result<GetGroupResponse>.Failure("No given data to process", 404);
        Result<Group> GroupData = await _groupRepository.FindGroupasync(_GroupId);
        if(!GroupData.IsSuccess || GroupData.Value is null) 
            return Result<GetGroupResponse>.Failure(GroupData.Error ?? "Invalid Credentials", 405);
        if(GroupData.Value.Status != "Active")
            return Result<GetGroupResponse>.Failure("Invalid Credential", 404);
        Result<List<GroupMember>> membersData = await _groupRepository.FindMembersasync(_GroupId);
        if(!membersData.IsSuccess || membersData.Value is null) 
            return Result<GetGroupResponse>.Failure(membersData.Error ?? "Invalid Credentials", membersData.StatusCode);
        var members = new Dictionary<int, List<string>>();
        foreach(var member in membersData.Value)
        {
            CollectionsMarshal.GetValueRefOrAddDefault(members, member.GroupNumber, out _) ??= new List<string>();
            members[member.GroupNumber].Add(member.Name);
        }
        foreach(var groupmember in members.Values) groupmember.Sort();
        var response = new GetGroupResponse(
            HashedId:await Task.Run(() => _Security.CreateHashids(GroupData.Value.GroupId)),
            GroupName: GroupData.Value.Name, 
            Owner: GroupData.Value.Owner?.Name ?? "Anonymous", 
            NumberOfGroups: GroupData.Value.NoOfGroups, 
            Members:members
            ); 
        return Result<GetGroupResponse>.Success(response);
    }
    public async Task<Result<List<GetGroupsDataResponse>>> GetGroupsData(int UserUid)
    {
        Result<List<Group>> groups = await _groupRepository.FindGroupsData(UserUid);
        if(!groups.IsSuccess || groups.Value is null)
            return Result<List<GetGroupsDataResponse>>.Failure(groups.Error ?? "Invalid Credentials", groups.StatusCode);
        
        List<GetGroupsDataResponse> response = new();
        foreach(var group in groups.Value)
        {
            response.Add(new GetGroupsDataResponse(
                GroupName: group.Name, 
                GroupHashedId: await Task.Run(() => _Security.CreateHashids(group.GroupId)),
                NumberOfGroups: group.NoOfGroups)
                );
        }
        return Result<List<GetGroupsDataResponse>>.Success(response);
    }

}