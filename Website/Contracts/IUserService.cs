using Website.Models.Account;

namespace Website.Contracts;

public interface IUserService
{
    Task<User> GetUserByUsername(string username);
    Task<string> AuthenticateAsync(string username, string password);
    Task<User> CreateUser(string firstname, string middlename, string lastname, string email, string password, string role);
    Task CreateMockUsers();
}


