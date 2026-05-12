using PracticeWeb.DTOs;
using PracticeWeb.Model;
namespace PracticeWeb.Interface

{
    public interface IGroupService
    {
        Task CreateGroup();
        Task AddMember(int UserUid, List<MemberDto> members);
        Task<GetGroupResponse> CreateGroup(int? Useruid, CreateGroupRequest dto);
        Task<GetGroupResponse> GetGroupData(string? hashId, int? GroupId);
        Task<List<GetGroupsDataResponse>> GetGroupsData(int UserUid);
    }
    public interface IGroupRepository
    {
     Task AddMemberAsync(List<Member> dto);
     Task<List<Member>> GetMembersAsync(int UserUid);   
     Task SaveChangesAsync();
     Task<int> SaveGroupAndMembers(Group group, List<GroupMember> groupMembers);
     Task<Group> FindGroupasync(int GroupId);
     Task<List<GroupMember>> FindMembersasync(int GroupId);
     Task<List<Group>> FindGroupsData(int UserUid);
    }
}