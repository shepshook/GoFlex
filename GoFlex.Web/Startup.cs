using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
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

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
