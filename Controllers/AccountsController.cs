namespace MondoAspNetMvcSample.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Mondo;

    using MondoAspNetMvcSample.App_Classes;
    using MondoAspNetMvcSample.App_Classes.ExtensionMethods;
    using MondoAspNetMvcSample.ViewModels.Accounts;

    [RoutePrefix("accounts")]
    public class AccountsController : BaseController
    {
        [HttpGet]
        [Route("~/accounts")]
        public async Task<ActionResult> Index()
        {
            var accessToken = ClaimsPrincipal.Current.AccessToken();
            if (accessToken == null)
            {
                var viewModel = new IndexViewModel(false);

                return this.View("Index", viewModel);
            }
            else
            {
                var viewModel = new IndexViewModel(true);
                using (var client = new MondoClient(accessToken, "https://production-api.gmon.io"))
                {
                    var accounts = await client.GetAccountsAsync();
                    foreach (var account in accounts)
                    {
                        var balance = await client.GetBalanceAsync(account.Id);
                        var accountsummary = new IndexViewModel.AccountSummary(account.Id, account.Created, account.Description, balance.Currency, balance.Value);

                        viewModel.AddAccountSummary(accountsummary);
                    }
                }

                return this.View("Index", viewModel);
            }
        }

        [HttpGet]
        [Route("/{id:int}")]
        public ActionResult Detail(int id)
        {
            throw new NotImplementedException();
            ////return View();
        }
    }
}