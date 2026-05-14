using Microsoft.AspNetCore.Mvc;
using PracticeWeb.DTOs;
using PracticeWeb.ErrorHandling;
using PracticeWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using PracticeWeb.Interface;
namespace PracticeWeb.Router;
[ApiController]
[Route("API/[controller]")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _GroupServices;
    public GroupController(IGroupService groupService) => _GroupServices = groupService;

    [HttpPost("AddUpdate")]
    [Authorize]
    public async Task<IActionResult> AddUpdateMember([FromBody] List<MemberDto> dto)
    {
        var UserUid = User.GetUserId();
        if(!UserUid.HasValue) return Unauthorized();
        var result= await _GroupServices.AddMember(UserUid.Value, dto);
        if(!result.IsSuccess)
            return StatusCode(result.StatusCode, new
            {
                error = result.Error,
                timestamps = DateTime.UtcNow
            }
            );
        return Ok("Success!");
    }
    [HttpPost("Create")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest dto)
    {
        var UserUid = User.GetUserId();
        GetGroupResponse response;
        if(UserUid is not null)  response = await _GroupServices.CreateGroup(UserUid.Value, dto);
        else  response =await _GroupServices.CreateGroup(Useruid: null, dto);
        return Ok(response);
    }
    [HttpGet("{GroupId}")]
    public async Task<IActionResult> GetGroup(string GroupId)
    {
        var response = await _GroupServices.GetGroupData(hashId:GroupId, GroupId:null);
        return Ok(response);
    }
    [HttpGet("GrousData")]
    [Authorize]
    public async Task<IActionResult> GetUserGroupsData()
    {
        var UserUid = User.GetUserId();
        if(!UserUid.HasValue) return Unauthorized();
        List<GetGroupsDataResponse> response = await _GroupServices.GetGroupsData(UserUid.Value);
        return Ok(response);
    }
}   