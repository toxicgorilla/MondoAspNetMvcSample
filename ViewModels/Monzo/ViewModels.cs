namespace MondoAspNetMvcSample.ViewModels.Monzo
{
    using System.Collections.Generic;

    using Mondo;

    public sealed class IndexViewModel
    {
        public Account Account { get; set; }

        public Balance Balance { get; set; }
    }

    public sealed class ActivateViewModel
    {
    }

    public sealed class OAuthCallbackViewModel
    {
        public bool Error { get; set; }
    }
}
