namespace MondoAspNetMvcSample.Controllers
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Security;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Mondo;

    using MondoAspNetMvcSample.App_Classes;
    using MondoAspNetMvcSample.App_Classes.ExtensionMethods;
    using MondoAspNetMvcSample.ViewModels;
    using MondoAspNetMvcSample.ViewModels.Home;

    public sealed class HomeController : Controller
    {
        private readonly IMondoAuthorizationClient mondoAuthorizationClient;

        public HomeController()
        {
            this.mondoAuthorizationClient = new MondoAuthorizationClient(
                AppSettings.MondoClientId,
                AppSettings.MondoClientSecret,
                "https://production-api.gmon.io");
        }

        [HttpGet]
        public ActionResult Index()
        {
            var accessToken = ClaimsPrincipal.Current.AccessToken();

            var viewModel = new IndexViewModel { HasAccessToken = accessToken != null };

            return this.View(viewModel);
        }

        [HttpGet]
        public ActionResult Login()
        {
            var state = CryptoHelper.GenerateRandomString(64);

            this.Session["state"] = state;

            // send user to Mondo's login page
            return this.Redirect(this.mondoAuthorizationClient.GetAuthorizeUrl(state, this.Url.Action("OAuthCallback", "Home", null, this.Request.Url.Scheme)));
        }
    }
}
