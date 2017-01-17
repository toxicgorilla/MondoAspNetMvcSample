namespace MondoAspNetMvcSample.Controllers
{
    using System;
    using System.Net;
    using System.Security;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Mondo;

    using MondoAspNetMvcSample.App_Classes;
    using MondoAspNetMvcSample.App_Classes.ExtensionMethods;
    using MondoAspNetMvcSample.UserService;
    using MondoAspNetMvcSample.ViewModels.Monzo;

    public class MonzoController : BaseController
    {
        private readonly UserService userService = new UserService();

        private readonly IMondoAuthorizationClient mondoAuthorizationClient;

        public MonzoController()
        {
            this.mondoAuthorizationClient = new MondoAuthorizationClient(AppSettings.MondoClientId, AppSettings.MondoClientSecret, AppSettings.MondoApiUrl);
        }

        public ActionResult Activate()
        {
            var accessToken = ClaimsPrincipal.Current.AccessToken();
            if (accessToken != null)
            {
                this.RedirectToAction("Index", "Home");
            }

            var viewModel = new ActivateViewModel();

            return this.View("Activate", viewModel);
        }

        public ActionResult GoToMonzo()
        {
            var state = CryptoHelper.GenerateRandomString(64);

            this.Session["state"] = state;

            // send user to Mondo's login page
            return this.Redirect(this.mondoAuthorizationClient.GetAuthorizeUrl(state, this.Url.Action("OAuthCallback", "Monzo", null, this.Request.Url.Scheme)));
        }
        
        [HttpGet]
        public async Task<ActionResult> OAuthCallback(string code, string state)
        {
            // Verify anti-CSRF state token matches what was sent
            var expectedState = this.Session["state"] as string;
            if (!string.Equals(expectedState, state))
            {
                throw new SecurityException("State mismatch");
            }

            this.Session.Remove("state");

            // HACK: Get rid of this!
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
#endif

            // Exchange authorization code for access token
            try
            {
                var accessToken =
                    await this.mondoAuthorizationClient.GetAccessTokenAsync(code, this.Url.Action("OAuthCallback", "Monzo", null, this.Request.Url.Scheme));

                var currentUser = this.userService.GetUser(ClaimsPrincipal.Current.UserId());
                currentUser.AccessToken = accessToken.Value;
                this.userService.UpdateUser(currentUser);

                this.AddClaim(CustomClaimTypes.AccessToken, accessToken.Value);

                return this.RedirectToAction("Index", "Accounts");
            }
            catch (Exception ex)
            {
                // TODO: Logging
                var viewModel = new OAuthCallbackViewModel { Error = true };

                return this.View("OAuthCallback", viewModel);
            }
        }
    }
}
