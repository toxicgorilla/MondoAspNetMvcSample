namespace MondoAspNetMvcSample.Controllers
{
    using System.Configuration;
    using System.Security;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Mondo;

    using MondoAspNetMvcSample.App_Classes;
    using MondoAspNetMvcSample.Models;

    public sealed class HomeController : Controller
    {
        private readonly IMondoAuthorizationClient mondoAuthorizationClient;

        public HomeController()
        {
            var clientId = ConfigurationManager.AppSettings["MondoClientId"];
            var clientSecret = ConfigurationManager.AppSettings["MondoClientSecret"];

            this.mondoAuthorizationClient = new MondoAuthorizationClient(clientId, clientSecret, "https://production-api.gmon.io");
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            var state = CryptoHelper.GenerateRandomString(64);

            this.Session["state"] = state;

            // send user to Mondo's login page
            return this.Redirect(this.mondoAuthorizationClient.GetAuthorizeUrl(state, this.Url.Action("OAuthCallback", "Home", null, this.Request.Url.Scheme)));
        }

        [HttpGet]
        public async Task<ActionResult> OAuthCallback(string code, string state)
        {
            // verify anti-CSRF state token matches what was sent
            var expectedState = this.Session["state"] as string;

            if (!string.Equals(expectedState, state))
            {
                throw new SecurityException("State mismatch");
            }

            this.Session.Remove("state");

            // exchange authorization code for access token
            var accessToken =
                await this.mondoAuthorizationClient.GetAccessTokenAsync(code, this.Url.Action("OAuthCallback", "Home", null, this.Request.Url.Scheme));

            // fetch transactions etc
            using (var client = new MondoClient(accessToken.Value, "https://production-api.gmon.io"))
            {
                var accounts = await client.GetAccountsAsync();
                var balance = await client.GetBalanceAsync(accounts[0].Id);
                var transactions = await client.GetTransactionsAsync(accounts[0].Id, expand: "merchant");

                return this.View(new AccountSummaryModel { Account = accounts[0], Balance = balance, Transactions = transactions });
            }
        }
    }
}
