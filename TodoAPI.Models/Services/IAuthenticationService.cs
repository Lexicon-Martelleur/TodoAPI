using TodoAPI.Models.DTO;

namespace TodoAPI.Models.Services;

public interface IAuthenticationService
{
    Task<TokenDTO?> AuthenticateByPassword(UserAuthenticationDTO authentication);
}