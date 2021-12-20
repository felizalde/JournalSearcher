using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Website.Utils.Account;

public class JWTTokenConfiguration
{

    private readonly byte[] _key;

    public JWTTokenConfiguration(string key)
    {
        _key = Encoding.ASCII.GetBytes(key);
    }


    /// <summary>
    /// Gets the token default security parameters
    /// </summary>
    /// <returns></returns>
    public TokenValidationParameters GetJWTTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false, //TODO: Improve this adding a refresh token mechanism. :D
            //ClockSkew = TimeSpan.Zero // This is set to 5 minutes (so a token will last 5 minutes more than its actual expiration datetime)
        };
    }


    /// <summary>
    /// Validates a user token
    /// </summary>
    /// <param name="token"></param>
    /// <returns>True is the token is valid</returns>
    public bool IsValidToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var user = handler.ValidateToken(token, this.GetJWTTokenValidationParameters(), out SecurityToken validatedToken);
            return user != null;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
