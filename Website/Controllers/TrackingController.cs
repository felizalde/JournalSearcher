using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Website.Utils.Account;
using Website.Models.Tracking;

namespace Website.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TrackingController : ControllerBase
{

    [HttpPost]
    [Authorize(Roles = ApplicationRoles.ALL)]
    public async Task<IActionResult> Track([FromBody] EventRequest request)
    {

        await Task.Delay(300);
        return Ok("Event Saved");
    }

}