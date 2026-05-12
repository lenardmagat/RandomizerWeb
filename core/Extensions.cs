using System.Security.Claims;
namespace PracticeWeb.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(int.TryParse(userIdClaim, out int Useruid)) return Useruid;
            return null;
        }
    }
}