using System.Security.Claims;

namespace TodoAPI.Models.Services;

public class ClaimService : IClaimService
{
    public int? GetUserIdFromClaims(ClaimsPrincipal user)
    {
        var claimedSubject = user.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (claimedSubject == null || !int.TryParse(claimedSubject, out int claimedUserId))
        {
            return null;
        }

        return claimedUserId;
    }
}
