namespace MondoAspNetMvcSample.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;

    using MondoAspNetMvcSample.App_Classes;
    using MondoAspNetMvcSample.UserService;
    using MondoAspNetMvcSample.ViewModels.Login;

    [AllowAnonymous]
    public class LoginController : BaseController
    {
        private readonly UserService userService = new UserService();

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel();

            return this.View("Index", viewModel);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel postedViewModel)
        {
            var user = this.userService.TryLogin(postedViewModel.Username, postedViewModel.Password);
            if (user != null)
            {
                var claims = new List<Claim> { new Claim(CustomClaimTypes.UserId, user.Id.ToString()) };
                if (user.AccessToken != null)
                {
                    claims.Add(new Claim(CustomClaimTypes.AccessToken, user.AccessToken));
                }

                var claimsIdentity = new ClaimsIdentity(claims, AppSettings.AuthenticationCookie);
                var authenticationManager = this.HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignIn(claimsIdentity);

                return this.RedirectToAction("Index", "Home");
            }

            var viewModel = new IndexViewModel { ErrorMessage = "Invalid login" };

            return this.View("Index", viewModel);
        }
    }
}
