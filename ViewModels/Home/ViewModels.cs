namespace MondoAspNetMvcSample.ViewModels.Home
{
    using MondoAspNetMvcSample.App_Classes;

    public class HomeViewModelBase : ViewModelBase
    {
        public HomeViewModelBase()
        {
            this.SiteSection = SiteSection.Home;
        }
    }

    public sealed class IndexViewModel : HomeViewModelBase
    {
    }
}
