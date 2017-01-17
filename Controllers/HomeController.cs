namespace MondoAspNetMvcSample.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using MondoAspNetMvcSample.App_Classes;
    using MondoAspNetMvcSample.ViewModels.Home;

    public sealed class HomeController : BaseController
    {
        public ActionResult Index()
        {
            // HACK: Get rid of this!
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
#endif

            var viewModel = new IndexViewModel();

            return this.View("Index", viewModel);
        }
    }
}
