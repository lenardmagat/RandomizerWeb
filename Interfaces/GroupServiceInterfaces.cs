using PracticeWeb.DTOs;
using PracticeWeb.Model;
using PracticeWeb.ErrorHandling;
namespace PracticeWeb.Interface

{
    public interface IGroupService
    {
        Task CreateGroup();
        Task<Result> AddMember(int UserUid, List<MemberDto> members);
        Task<Result<GetGroupResponse>> CreateGroup(int? Useruid, CreateGroupRequest dto);
        Task<Result<GetGroupResponse>> GetGroupData(string? hashId, int? GroupId);
        Task<Result<List<GetGroupsDataResponse>>> GetGroupsData(int UserUid);
    }
    public interface IGroupRepository
    {
     Task <Result>AddMemberAsync(List<Member> dto);
     Task<Result<List<Member>>> GetMembersAsync(int UserUid);   
     Task <Result> SaveChangesAsync();
     Task<Result<int>> SaveGroupAndMembers(Group group, List<GroupMember> groupMembers);
     Task<Result<Group>> FindGroupasync(int GroupId);
     Task<Result<List<GroupMember>>> FindMembersasync(int GroupId);
     Task<Result<List<Group>>> FindGroupsData(int UserUid);
    }
}