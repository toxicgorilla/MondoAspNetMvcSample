namespace MondoAspNetMvcSample.App_Classes.ExtensionMethods
{
    using System.Linq;
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static int UserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.Single(c => c.Type == CustomClaimTypes.UserId);

            return int.Parse(claim.Value);
        }

        public static string AccessToken(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.SingleOrDefault(c => c.Type == CustomClaimTypes.AccessToken);

            return claim?.Value;
        }
    }
}
