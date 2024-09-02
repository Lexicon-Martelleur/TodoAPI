
namespace TodoAPI.Models.Repositories;

public interface IAuthenticationRepository
{
    Task<string?> AuthenticateByPassword();
}