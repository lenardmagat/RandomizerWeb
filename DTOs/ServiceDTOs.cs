using System.ComponentModel.DataAnnotations;
namespace PracticeWeb.DTOs;

public record MemberDto(
    [Required] string Name
    
);

public record CreateGroupRequest(
    [Required][StringLength(12, MinimumLength =5)] string GroupName,
    [Required][MinLength(1, ErrorMessage = "Atleast one member is required")]List<string> Members,
    [Required] int NumberOfGroups
);
public record GetGroupResponse(
    [Required] string HashedId,
    [Required] string GroupName,
    string? Owner,
    [Required] int NumberOfGroups,
    [Required] Dictionary<int, List<string>> Members
);
public record GetGroupsDataResponse(
    string GroupName,
    string GroupHashedId,
    int NumberOfGroups
);