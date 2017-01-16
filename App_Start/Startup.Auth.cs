namespace MondoAspNetMvcSample
{
    using System;

    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;

    using Owin;

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {
                    AuthenticationType = "MondoAspNetMvcSample",
                    SlidingExpiration = true,
                    ExpireTimeSpan = TimeSpan.FromMinutes(15),
                    LoginPath = new PathString("/login"),
                    ReturnUrlParameter = "returnUrl"
                });
        }
    }
}