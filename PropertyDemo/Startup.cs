using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PropertyDemo.Data;
using PropertyDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyDemo.Data.Model;
using PropertyDemo.Service;
using PropertyDemo.Configuration;
using PropertyDemo.Data.DatabaseService.PropertyService;
using PropertyDemo.Data.DatabaseService.TransactionService;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;

namespace PropertyDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IAppConfiguration AppConfiguration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppConfiguration = new AppConfiguration(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("PropertyDemo.Migrations")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(AppConfiguration);
            services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<ITransactionService, TransactionService>();

            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddHealthChecks()
                    .AddDbContextCheck<DataContext>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
                });
                endpoints.MapRazorPages();
            });
        }
    }
}
