using Microsoft.AspNetCore.Mvc;
using PracticeWeb.DTOs;
using PracticeWeb.Interfaces;
using PracticeWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using PracticeWeb.Interface;
namespace PracticeWeb.Router;
[ApiController]
[Route("API/Group")]
public class GroupServices : ControllerBase
{
    private readonly IGroupService _GroupServices;
    public GroupServices(IGroupService groupService) => _GroupServices = groupService;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateGroup([FromBody] List<MemberDto> dto)
    {
        var UserUid = User.GetUserId();
        if(!UserUid.HasValue) return Unauthorized();
        await _GroupServices.AddMember(UserUid.Value, dto);
        return Ok("Success!");
    }
}