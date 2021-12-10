using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Website.Data;
using Website.Models;
using Website.Models.Account;

namespace Website.Utils.Account;

public class ApplicationClaimsIndentityFactory : UserClaimsPrincipalFactory<User>
{

    private readonly UserManager<User> _userManager;
    private readonly IOptions<AppSettings> _journalsConfiguration;


    public ApplicationClaimsIndentityFactory(
        UserManager<User> userManager,
        IOptions<IdentityOptions> optionsAccessor,
        IOptions<AppSettings> journalsConfiguration) : base(userManager, optionsAccessor)
    {
        _userManager = userManager;
        _journalsConfiguration = journalsConfiguration;
    }

    public async override Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var principal = await base.CreateAsync(user);

        var claimsPrincipal = (ClaimsIdentity)principal.Identity;

        // Add the Full Name as identity claims so it's accessible everywhere
        claimsPrincipal.AddClaim(new Claim(UserClaims.FullName, $"{user.FirstName} {user.LastName}"));

        // Add the user roles as claims
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            claimsPrincipal.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return principal;
    }

}