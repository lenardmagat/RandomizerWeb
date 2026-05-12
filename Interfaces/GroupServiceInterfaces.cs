using PracticeWeb.DTOs;
using PracticeWeb.Model;
namespace PracticeWeb.Interface

{
    public interface IGroupService
    {
        Task CreateGroup();
        Task AddMember(int UserUid, List<MemberDto> members);
    }
    public interface IGroupRepository
    {
     Task AddMemberAsync(List<Member> dto);
     Task<List<Member>> GetMembersAsync(int UserUid);   
     Task SaveChangesAsync();
    }
}