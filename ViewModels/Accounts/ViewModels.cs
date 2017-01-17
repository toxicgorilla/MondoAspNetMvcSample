namespace MondoAspNetMvcSample.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;

    using MondoAspNetMvcSample.App_Classes;

    public class AccountViewModelBase : ViewModelBase
    {
        public AccountViewModelBase()
        {
            this.SiteSection = SiteSection.Accounts;
        }
    }

    public sealed class IndexViewModel : AccountViewModelBase
    {
        #region Constructor

        public IndexViewModel(bool hasAccessToken)
        {
            this.HasAccessToken = hasAccessToken;
            this.AccountSummaries = new List<AccountSummary>();
        }

        #endregion

        #region Properties

        public bool HasAccessToken { get; private set; }

        public List<AccountSummary> AccountSummaries { get; }

        #endregion

        #region Methods

        public void AddAccountSummary(AccountSummary accountSummary)
        {
            this.AccountSummaries.Add(accountSummary);
        }

        #endregion

        #region Classes

        public class AccountSummary
        {
            public AccountSummary(string id, DateTime createdDate, string description, string currencyCode, decimal balance)
            {
                this.Id = id;
                this.CreatedDate = createdDate;
                this.Description = description;
                this.CurrencyCode = currencyCode;
                this.Balance = balance;
            }

            public string Id { get; private set; }

            public DateTime CreatedDate { get; private set; }

            public string Description { get; private set; }

            public string CurrencyCode { get; private set; }

            public decimal Balance { get; private set; }
        }

        #endregion
    }

    public sealed class DetailViewModel : AccountViewModelBase
    {
        public DetailViewModel(string id, string currency, long balance)
        {
            this.Id = id;
            this.Currency = currency;
            this.Balance = balance;
            this.Transactions = new List<TransactionViewModel>();
        }

        public string Id { get; private set; }

        public string Currency { get; private set; }

        public long Balance { get; private set; }

        public IList<TransactionViewModel> Transactions { get; }

        public void AddTransaction(TransactionViewModel transactionViewModel)
        {
            this.Transactions.Add(transactionViewModel);
        }

        #region Classes

        public sealed class TransactionViewModel
        {
            public TransactionViewModel(string id, DateTime created, string currency, long amount, long accountBalance, string notes)
            {
                this.Id = id;
                this.Created = created;
                this.Currency = currency;
                this.Amount = amount;
                this.AccountBalance = accountBalance;
                this.Notes = notes;
            }

            public string Id { get; private set; }

            public DateTime Created { get; private set; }

            public string Currency { get; private set; }

            public long Amount { get; private set; }

            public long AccountBalance { get; private set; }

            public string Notes { get; private set; }
        }

        #endregion
    }
}
