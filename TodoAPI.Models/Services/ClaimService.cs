using System.Security.Claims;
using TodoAPI.Models.DTO;

namespace TodoAPI.Models.Services;

public class ClaimService : IClaimService
{
    public int? GetValidUserIdFromClaims(ClaimsPrincipal user, TodoDTO todo)
    {
        var claimedSubject = user.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (claimedSubject == null ||
            !int.TryParse(claimedSubject, out int claimedUserId) ||
            claimedUserId != todo.UserId)
        {
            return null;
        }

        return claimedUserId;
    }

    public int? GetValidUserIdFromClaims(ClaimsPrincipal user)
    {
        var claimedSubject = user.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (claimedSubject == null ||
            !int.TryParse(claimedSubject, out int claimedUserId))
        {
            return null;
        }

        return claimedUserId;
    }
}
