using System.Security.Claims;

namespace BlazorClient.Provider
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsInRole(this ClaimsPrincipal user, string role)
        {
            return user?.Claims?.Any(c => c.Type == ClaimTypes.Role && c.Value == role) ?? false;
        }
    }
}
