namespace MondoAspNetMvcSample.ViewModels.Monzo
{
    using Mondo;

    using MondoAspNetMvcSample.App_Classes;

    public class MonzoViewModelBase : ViewModelBase
    {
        public MonzoViewModelBase()
        {
            this.SiteSection = SiteSection.Monzo;
        }
    }

    public sealed class IndexViewModel : MonzoViewModelBase
    {
        public Account Account { get; set; }

        public Balance Balance { get; set; }
    }

    public sealed class ActivateViewModel : MonzoViewModelBase
    {
    }

    public sealed class OAuthCallbackViewModel : MonzoViewModelBase
    {
        public bool Error { get; set; }
    }
}
