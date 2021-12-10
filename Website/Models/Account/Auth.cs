using System.ComponentModel.DataAnnotations;
namespace Website.Models.Account;

public record AuthRequest([Required] string Username, [Required] string Password);
public record AuthResponse(string Email, string Firstname, string Lastname, string Token);