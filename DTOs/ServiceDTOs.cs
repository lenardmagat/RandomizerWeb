using System.ComponentModel.DataAnnotations;
namespace PracticeWeb.DTOs;

public record MemberDto(
    [Required] string Name,
    int? UserUid = null
    
);

public record CreateGroupRequest(
    [Required][StringLength(12, MinimumLength =5)] string GroupName,
    [Required][MinLength(1, ErrorMessage = "Atleast one member is required")]List<MemberDto> Members,
    [Required] int NumberOfGroups
);
