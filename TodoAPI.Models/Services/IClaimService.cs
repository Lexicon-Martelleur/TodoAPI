using System.Security.Claims;

namespace TodoAPI.Models.Services
{
    public interface IClaimService
    {
        int? GetUserIdFromClaims(ClaimsPrincipal user);
    }
}