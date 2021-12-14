
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Website.Contracts;
using Website.Data;
using Website.Models;
using Website.Models.Account;
using Website.Utils.Account;

namespace WebAPI.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly AppSettings _configuration;

    public UserService(
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IOptions<AppSettings> configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration.Value;
    }

    public async Task<User> GetUserByUsername(string username) => await _userManager.FindByNameAsync(username);

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null)
        {
            var validPassword = await _userManager.CheckPasswordAsync(user, password);

            if (validPassword)
            {
                // Get the user Roles once it is all validated
                var userRoles = await _userManager.GetRolesAsync(user);
                // Create the token including user Roles
                var token = GenerateJWT(user, userRoles);

                return token;
            }
        }

        return null;
    }


    private string GenerateJWT(User user, IList<string> userRoles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.JWTSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        // Add the user Roles as Claims (this is the way JWT handles the Roles)
        foreach (var role in userRoles)
        {
            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        // Add the full name claim            
        tokenDescriptor.Subject.AddClaim(new Claim(UserClaims.FullName, $"{user.FirstName} {user.LastName}"));

        var token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenStr = tokenHandler.WriteToken(token);

        return tokenStr;
    }

    public async Task CreateMockUsers()
    {
        bool adminRoleExist = await _roleManager.RoleExistsAsync(ApplicationRoles.ADMIN);
        bool userRoleExist = await _roleManager.RoleExistsAsync(ApplicationRoles.USER);

        if (!adminRoleExist) await _roleManager.CreateAsync(new IdentityRole<Guid>() { Name = ApplicationRoles.ADMIN });
        if (!userRoleExist) await _roleManager.CreateAsync(new IdentityRole<Guid>() { Name = ApplicationRoles.USER });

        await CreateUser("Federico", string.Empty, "Elizalde", "efealde@gmail.com", "Riquelme1.", ApplicationRoles.ADMIN);
        await CreateUser("Nicolas", string.Empty, "Leiva", "nicoleiva08@gmail.com", "Riquelme1.", ApplicationRoles.ADMIN);
        await CreateUser("Diego", string.Empty, "Alonso", "diegoalonso1709@gmail.com", "Riquelme1.", ApplicationRoles.ADMIN);

        await CreateUser("Guest", string.Empty, "1", "guest1@jr10.com", "Riquelme1.", ApplicationRoles.USER);
        await CreateUser("Guest", string.Empty, "2", "guest2@jr10.com", "Riquelme1.", ApplicationRoles.USER);

    }

    public async Task<User> CreateUser(string firstname, string middlename, string lastname, string email, string password, string role)
    {
        var user = await _userManager.FindByNameAsync(email);
        if (user == null)
        {
            //TODO: Validate empty and null values.
            _ = await _userManager.CreateAsync(new User
            {
                FirstName = firstname,
                MiddleName = middlename,
                LastName = lastname,
                Email = email,
                UserName = email
            }, password);

            user = await _userManager.FindByNameAsync(email);
            _ = await _userManager.AddToRoleAsync(user, role);

            return user;

        }
        else
        {
            throw new Exception("User already exist.");
        }
    }
}
