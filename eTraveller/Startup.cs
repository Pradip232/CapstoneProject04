using eTraveller.Data;
using eTraveller.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTraveller
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //Get the connection string from AppSettings.json file
                string ConnectionString = Configuration.GetConnectionString("MyDefaultString");
                options.UseSqlServer(ConnectionString);
            });

            // Register the OWIN Identity Middleware
            services
                  .AddIdentity<IdentityUser, IdentityRole>(options =>
                  {
                      options.SignIn.RequireConfirmedAccount = true;
                      options.Password.RequiredLength = 8;
                  })
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders();

            //Register the Asp.Net Razor pages middleware
            services.AddRazorPages().AddRazorPagesOptions(option => { option.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                option.Conventions.AuthorizeAreaPage("Identity", "/Account/Manage");
            });

            //Application Cookie
            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/Identity/Account/Login";
                option.LogoutPath = "/Identity/Account/Logout";
                option.AccessDeniedPath = "/Identity/Account/AccessDenied";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(5);                        //default session cookie expire time : 5 min
                option.SlidingExpiration = true;
                option.Cookie.HttpOnly = true;
                option.Cookie.Name = "MyAuthCookie";
            });

            //Register the EmailSender Service For Dependancy Injection
            services.AddSingleton<IEmailSender, MyEmailSenderService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            //14/09/2022 :: 01:47:30AM
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                // Register the endpoints for the ROUTES in the AREAS
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area}/{controller=Home}/{action=Index}/{id?}");

                // Register the endpoints for the ROUTES not in any area.
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
