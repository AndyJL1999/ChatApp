using System.Security.Claims;

namespace ChatApp.API.Extensions
{
    public static class ClaimsPrincipleExtentions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
