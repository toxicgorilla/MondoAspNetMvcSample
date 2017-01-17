namespace MondoAspNetMvcSample.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;

    using Mondo;

    public sealed class IndexViewModel
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

    public sealed class DetailViewModel
    {
        public string Id { get; private set; }

        public IList<Transaction> Transactions { get; set; }
    }
}
