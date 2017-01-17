namespace MondoAspNetMvcSample.Controllers
{
    using System.Web.Mvc;

    using MondoAspNetMvcSample.App_Classes;
    using MondoAspNetMvcSample.ViewModels.Home;

    public sealed class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel();

            return this.View("Index", viewModel);
        }
    }
}
