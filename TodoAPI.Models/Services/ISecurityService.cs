using TodoAPI.Entities;

namespace TodoAPI.Models.Services;

public interface ISecurityService
{
    string? GenerateToken(UserAuthenticationEntity userAuthentication);
    string HashPassword(UserAuthenticationEntity userAuthentication, string password);
    bool VerifyPassword(UserAuthenticationEntity userAuthentication, string hashedPassword, string providedPassword);
}