
using TodoAPI.DBContext;

namespace TodoAPI.Models.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly TodoContext _context;

    public AuthenticationRepository(TodoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<string?> AuthenticateByPassword()
    {
        throw new NotImplementedException();
    }
}
