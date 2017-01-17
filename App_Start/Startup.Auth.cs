namespace MondoAspNetMvcSample
{
    using System;

    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;

    using MondoAspNetMvcSample.App_Classes;

    using Owin;

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {
                    AuthenticationType = AppSettings.AuthenticationCookie,
                    SlidingExpiration = true,
                    ExpireTimeSpan = TimeSpan.FromMinutes(15),
                    LoginPath = new PathString("/login"),
                    ReturnUrlParameter = "returnUrl"
                });
        }
    }
}