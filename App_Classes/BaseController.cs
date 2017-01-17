namespace MondoAspNetMvcSample.App_Classes
{
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        protected void AddClaim(string claimType, string value)
        {
            // Remove existing claim if it exists
            var claims = ClaimsPrincipal.Current.Claims.Where(c => c.Type != claimType).ToList();
            claims.Add(new Claim(claimType, value));

            var claimsIdentity = new ClaimsIdentity(claims, AppSettings.AuthenticationCookie);

            var authenticationManager = this.HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignIn(claimsIdentity);
        }
    }
}
