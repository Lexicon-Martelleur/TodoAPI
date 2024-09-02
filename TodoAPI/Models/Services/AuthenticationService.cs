using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoAPI.Config;
using TodoAPI.Entities;
using TodoAPI.Models.DTO;
using TodoAPI.Models.Repositories;

namespace TodoAPI.Models.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<UserAuthenticationEntity> _passwordHasher;

    public AuthenticationService (
        IAuthenticationRepository repository,
        IConfiguration configuration,
        IMapper mapper,
        IPasswordHasher<UserAuthenticationEntity> passwordHasher)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task<TokenDTO?> AuthenticateByPassword(
        UserAuthenticationDTO authentication)
    {
        var userAuthentication = _mapper.Map<UserAuthenticationEntity>(authentication);

        // TODO Validate user in DB
        var token = GenerateToken(userAuthentication);
        
        if (token == null) { return null; }
        
        return new TokenDTO() { Token = token };
    }


    private string? GenerateToken(UserAuthenticationEntity userAuthentication)
    {
        var securityKey = new SymmetricSecurityKey(ApplicationConfig.GetTokenSecret(_configuration));

        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [
            new(ApplicationConfig.ClaimSubject, userAuthentication.Id.ToString()),
            new(ApplicationConfig.ClaimEmail, userAuthentication.EMail)
        ];

        var token = new JwtSecurityToken(
            ApplicationConfig.GetTokenIssuer(_configuration),
            ApplicationConfig.GetTokenAudience(_configuration),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(
        UserAuthenticationEntity userAuthentication,
        string password)
    {
        return _passwordHasher.HashPassword(userAuthentication, password);
    }

    private bool VerifyPassword(
        UserAuthenticationEntity userAuthentication,
        string hashedPassword,
        string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(userAuthentication, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
