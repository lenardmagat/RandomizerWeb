using Microsoft.AspNetCore.Mvc;
using PracticeWeb.DTOs;
using PracticeWeb.Interfaces;
using PracticeWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using PracticeWeb.ErrorHandling;
namespace PracticeWeb.Routers;
[ApiController]
[Route("API/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountServices _AccountService;
    public AccountController(IAccountServices accountServices) => _AccountService = accountServices;
    
    [HttpPost("Create")]
    public async Task<IActionResult> CreateEndpoint(AccountCredentials dto)
    {
        Result result= await _AccountService.CreateAccount(dto);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, new
            {
                error = result.Error,
                timestamp = DateTime.UtcNow
                }
                );
        }
        return Ok(result);
    }
    [HttpPost("Login")]
    public async Task<IActionResult> LoginEndpoint(AccountCredentials dto)
    {
        Result<string?> user = await _AccountService.Login(dto);
        if (!user.IsSuccess)
        {
            return StatusCode(user.StatusCode, new
            {
                error = user.Error,
                timestamp = DateTime.UtcNow
            }
                );
        }
        return Ok(user);
    }
    [HttpPatch("Update-Profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] ChangePasswordCredentials dto)
    {
        int? UserUid = User.GetUserId();
        if(!UserUid.HasValue) return Unauthorized();
        Result result = await _AccountService.UpdateAccount(UserUid.Value, dto);
        if(!result.IsSuccess)
            return StatusCode(result.StatusCode, new
            {
                error = result.Error,
                timestamp = DateTime.UtcNow
            }
                );
        return Ok(result);
    }

}

