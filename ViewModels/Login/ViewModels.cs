namespace MondoAspNetMvcSample.ViewModels.Login
{
    using System.ComponentModel;

    public class IndexViewModel
    {
        [Description("Username")]
        public string Username { get; set; }

        [Description("Password")]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
    }
}