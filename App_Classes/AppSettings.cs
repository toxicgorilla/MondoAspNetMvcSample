namespace MondoAspNetMvcSample.App_Classes
{
    using System.Configuration;

    public static class AppSettings
    {
        public static string MondoClientId => ConfigurationManager.AppSettings["MondoClientId"];

        public static string MondoClientSecret => ConfigurationManager.AppSettings["MondoClientSecret"];
    }
}
