using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SimpleJwt.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptionsWithValidateOnStart<JwtSettings>(JwtSettings.ConfigSection);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/jwt", (JwtSettings settings, ILogger<Program> logger, IConfiguration configuration) =>
{
    var secret = settings.Secret;

    var claims = new Claim[]
    {
        new("sub", "user-name"),
    };

    var tokenDescriptor = new SecurityTokenDescriptor()
    {
        Subject = new ClaimsIdentity(claims),
        Issuer = settings.Issuer,
        Audience = settings.Audience,
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret)),
            SecurityAlgorithms.HmacSha512Signature),
    };

    var jwtHandler = new JwtSecurityTokenHandler();
    var securityToken = jwtHandler.CreateJwtSecurityToken(tokenDescriptor);
    var jwt = jwtHandler.WriteToken(securityToken);

    return jwt;
});

app.Run();

return;
