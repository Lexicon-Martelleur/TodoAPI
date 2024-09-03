using System.Security.Claims;
using TodoAPI.Models.DTO;

namespace TodoAPI.Models.Services;

public interface IClaimService
{
    int? GetValidUserIdFromClaims(ClaimsPrincipal user, TodoDTO todo);

    int? GetValidUserIdFromClaims(ClaimsPrincipal user);
}