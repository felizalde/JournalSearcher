using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Contracts;
using Website.Models.Account;
using Website.Utils;

namespace Website.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> logger;
    private readonly IUserService _userService;

    public AuthController(ILogger<AuthController> logger, IUserService userService)
    {
        this.logger = logger;
        _userService = userService;
    }


    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    public async Task<IActionResult> Authenticate([FromBody] AuthRequest credentials)
    {
        var token = await _userService.AuthenticateAsync(credentials.Username, credentials.Password);

        if (string.IsNullOrEmpty(token)) return BadRequest(new { message = "Username or password invalid." });

        var user = await _userService.GetUserByUsername(credentials.Username);

        return Ok(new AuthResponse(user.Email, user.FirstName, user.LastName, token));
    }

    [AllowAnonymous]
    [HttpGet, Route("test-anonymous")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult TestAnon()
    {
        return Ok("Anonymous access worked!");
    }


    [HttpGet, Route("test-auth")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult TestAuth()
    {
        return Ok($"Authorized access worked! You signed in as {this.CurrentLoggedUsername()} ** Roles: {string.Join(',', this.CurrentLoggedUserRoles())}");
    }

    [AllowAnonymous]
    [HttpPost, Route("create-test-users")]
    public async Task<IActionResult> CreateMockUsers()
    {
        await _userService.CreateMockUsers();

        return Ok("Users created");
    }
}