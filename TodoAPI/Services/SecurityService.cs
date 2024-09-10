using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoAPI.Config;
using TodoAPI.Constants;
using TodoAPI.Entities;
using TodoAPI.Lib;

namespace TodoAPI.Services;

public class SecurityService : ISecurityService
{
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<UserAuthenticationEntity> _passwordHasher;

    public SecurityService(
        IConfiguration configuration,
        IPasswordHasher<UserAuthenticationEntity> passwordHasher)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public string HashPassword(
        UserAuthenticationEntity userAuthentication,
        string password)
    {
        return _passwordHasher.HashPassword(userAuthentication, password);
    }

    public bool VerifyPassword(
        UserAuthenticationEntity userAuthentication,
        string hashedPassword,
        string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            userAuthentication,
            hashedPassword,
            providedPassword);
        return result == PasswordVerificationResult.Success;
    }

    public string? GenerateToken(UserAuthenticationEntity userAuthentication)
    {
        var securityKey = new SymmetricSecurityKey(SecurityConfig.GetTokenSecret(_configuration));

        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [
            new(SecurityConfig.ClaimSubject, userAuthentication.Id.ToString()),
            new(SecurityConfig.ClaimEmail, userAuthentication.Email),
            new(SecurityConfig.ClaimRole, AuthorizationRules.UserRole)
        ];

        var token = new JwtSecurityToken(
            SecurityConfig.GetTokenIssuer(_configuration),
            SecurityConfig.GetTokenAudience(_configuration),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(SecurityConfig.TokenValidTimeDuration),
            signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
