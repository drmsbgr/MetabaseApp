using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MetabaseApp.Infrastructure;

public class TokenProvider(IConfiguration configuration)
{
    private readonly IConfiguration configuration = configuration;

    public string Create(string username)
    {
        string secretKey = configuration["Jwt:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var payload = new Dictionary<string, object>
        {
            { "resource", new { dashboard = 2 } },
            { "params", new { } },
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity([
            ]),
            Claims = new Dictionary<string, object>
            {
                { "resource", new Dictionary<string, int>{ {"dashboard",2}} },
                { "params",  new Dictionary<string, int>()},
            },
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
        };

        var handler = new JwtSecurityTokenHandler();

        var securityToken = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(securityToken);
    }

    public bool ValidateJwtToken(string token, out SecurityToken validatedToken)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            return true;
        }
        catch (Exception)
        {
            validatedToken = null!;
            return false;
        }
    }
}