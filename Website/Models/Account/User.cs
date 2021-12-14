using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Website.Models.Account;

public class User : IdentityUser<Guid>
{
    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string MiddleName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    public bool IsActive { get; set; }

}

public static class UserClaims
{
    public const string FullName = "fullname";
}