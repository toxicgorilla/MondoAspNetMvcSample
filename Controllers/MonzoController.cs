namespace MondoAspNetMvcSample.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using Mondo;

    using MondoAspNetMvcSample.App_Classes;
    using MondoAspNetMvcSample.App_Classes.ExtensionMethods;
    using MondoAspNetMvcSample.UserService;
    using MondoAspNetMvcSample.ViewModels.Monzo;

    public class MonzoController : Controller
    {
        private UserService userService = new UserService();

        private readonly IMondoAuthorizationClient mondoAuthorizationClient;

        public MonzoController()
        {
            this.mondoAuthorizationClient = new MondoAuthorizationClient(
                AppSettings.MondoClientId,
                AppSettings.MondoClientSecret,
                "https://production-api.gmon.io");
        }

        public async Task<ActionResult> Index()
        {
            var accessToken = ClaimsPrincipal.Current.AccessToken();
            if (accessToken == null)
            {
                this.RedirectToAction("Activate", "Monzo");
            }

            // Fetch transactions etc
            using (var client = new MondoClient(accessToken, "https://production-api.gmon.io"))
            {
                var accounts = await client.GetAccountsAsync();
                var balance = await client.GetBalanceAsync(accounts[0].Id);
                
                var viewModel = new IndexViewModel { Account = accounts[0], Balance = balance };

                return this.View("Index", viewModel);
            }
        }

        public ActionResult Activate()
        {
            var accessToken = ClaimsPrincipal.Current.AccessToken();
            if (accessToken != null)
            {
                this.RedirectToAction("Index", "Monzo");
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

        public async Task<ActionResult> Transactions()
        {
            ////var transactions = await client.GetTransactionsAsync(accounts[0].Id, expand: "merchant");
            throw new NotImplementedException();
            var viewModel = new TransactionsViewModel();

            return this.View("Transactions", viewModel);
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
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };
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

                return this.RedirectToAction("Index", "Monzo");
            }
            catch (Exception ex)
            {
                // TODO: Logging
                var viewModel = new OAuthCallbackViewModel { Error = true };

                return this.View("OAuthCallback", viewModel);
            }
        }

        private void AddClaim(string claimType, string value)
        {
            // Remove existing claim if it exists
            var claims = ClaimsPrincipal.Current.Claims.Where(c => c.Type != claimType).ToList();
            claims.Add(new Claim(claimType, value));

            var claimsIdentity = new ClaimsIdentity(claims, "MondoAspNetMvcSample");

            var authenticationManager = this.HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignIn(claimsIdentity);
        }
    }
}
