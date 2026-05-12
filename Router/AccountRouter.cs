using Microsoft.AspNetCore.Mvc;
using PracticeWeb.DTOs;
using PracticeWeb.Interfaces;

namespace PracticeWeb.Router;
[ApiController]
[Route("API/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountServices _AccountService;
    public AccountController(IAccountServices accountServices) => _AccountService = accountServices;
    
    [HttpPost("Create")]
    public async Task<IActionResult> CreateEndpoint(AccountCredentials dto)
    {
        await _AccountService.CreateAccount(dto);
        return Ok("");
    }
    [HttpPost("Login")]
    public async Task<IActionResult> LoginEndpoint(AccountCredentials dto)
    {
        var user = await _AccountService.Login(dto);
        return Ok(user);
    }
}

