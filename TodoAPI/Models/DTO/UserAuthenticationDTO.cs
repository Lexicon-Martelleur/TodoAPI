
namespace TodoAPI.Models.DTO;

public class UserAuthenticationDTO
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string EMail { get; init; }
}
