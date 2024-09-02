
using TodoAPI.Entities;

namespace TodoAPI.Models.Repositories;

public interface IAuthenticationRepository
{
    public Task<UserAuthenticationEntity?> GetUserByEmail(string email);
}