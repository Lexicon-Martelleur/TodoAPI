
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.DTO;

public class UserAuthenticationDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}
