
//using Microsoft.EntityFrameworkCore;
//using TodoAPI.DbContext.Contexts;
//using TodoAPI.Entities;

//namespace TodoAPI.Models.Repositories;

//public class AuthenticationRepository : IAuthenticationRepository
//{
//    private readonly TodoContext _context;

//    public AuthenticationRepository(TodoContext context)
//    {
//        _context = context ?? throw new ArgumentNullException(nameof(context));
//    }

//    public async Task<UserAuthenticationEntity?> GetUserByEmail(string email)
//    {
//        return await _context.UserAuthentications
//            .Where(storedUser => storedUser.Email == email)
//            .FirstOrDefaultAsync();
//    }
//}
