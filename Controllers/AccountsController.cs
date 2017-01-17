namespace MondoAspNetMvcSample.Controllers
{
    using System;
    using System.Linq;
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
        [Route("~/accounts/{id}")]
        public async Task<ActionResult> Detail(string id)
        {
            var accessToken = ClaimsPrincipal.Current.AccessToken();
            if (accessToken == null)
            {
                return this.RedirectToAction("Index", "Accounts");
            }

            using (var client = new MondoClient(accessToken, "https://production-api.gmon.io"))
            {
                var allAccounts = await client.GetAccountsAsync();
                var account = allAccounts.SingleOrDefault(a => a.Id.ToLower() == id.ToLower());
                if (account == null)
                {
                    return new HttpNotFoundResult($"Account with id {id} was not found");
                }

                var balance = await client.GetBalanceAsync(account.Id);
                var viewModel = new DetailViewModel(account.Id, balance.Currency, balance.Value);

                var transactions = await client.GetTransactionsAsync(account.Id, expand: "merchant");
                foreach (var transaction in transactions)
                {
                    var transactionViewModel = new DetailViewModel.TransactionViewModel(
                        transaction.Id,
                        transaction.Created,
                        transaction.Currency,
                        transaction.Amount,
                        transaction.AccountBalance,
                        transaction.Notes);

                    viewModel.AddTransaction(transactionViewModel);
                }

                return this.View("Detail", viewModel);
            }
        }
    }
}
