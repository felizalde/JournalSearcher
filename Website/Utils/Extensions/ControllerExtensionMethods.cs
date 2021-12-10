using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Website.Utils;


/// <summary>
/// Extension methods to handle common controller objects 
/// </summary>
public static class ControllerExtensionMethods
{
    public static string CurrentLoggedUsername(this ControllerBase c)
    {
        return c.User.Identity.Name;
    }


    public static Guid CurrentLoggedUserID(this ControllerBase ctlr)
    {
        return Guid.Parse(ctlr.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }

    public static IEnumerable<string> CurrentLoggedUserRoles(this ControllerBase ctlr)
    {
        var roles = ctlr.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value);
        return roles;
    }

}