using System;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Infrastructure;
using GoFlex.Web.Services;
using GoFlex.Web.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;

namespace GoFlex.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = new PathString("/Error/401");
                    options.LoginPath = new PathString("/Login");
                    options.ReturnUrlParameter = "returnUrl";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEventService, Services.EventService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //todo: move the key to configs
            StripeConfiguration.ApiKey =
                "sk_test_51HdZIQIbg49bp0FGFkLkYfxwt8CaK57JwNTNApnzZ9AZ4UcumYXqJjGFn64VATNdpopdSYpFcm9tPFZSCGY9IbBn008jn8p4Cp";

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
