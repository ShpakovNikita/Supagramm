using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using IgiLab.Models;
using IgiLab.Constants;
using IgiLab.Constants.Enums;
using IgiLab.Hubs;
using IgiLab.Middlewares;
using NLog;
using AppManagers;
using EntityCore.Constants;
using AppManagers.Database;

namespace IgiLab
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

            // getting the connection string from json config
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            // adding Context to our application
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(connection));

            // cookies auth initialization
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddAuthorization(opts => {
                opts.AddPolicy(ClaimPolicies.ADMIN_REQUIRED, policy => {
                    policy.RequireClaim(ClaimTypes.Role, Roles.ADMIN);
                });
            });

            services.AddSignalR();

            services.AddTransient<DbContext, AppManagers.Database.Context>();
            services.AddTransient<IManagersContainer, ManagersContainer>();

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddTransient<MessageHub>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Pathes.BASE_DIR),
                RequestPath = Pathes.WAB_STATICFILES_DIR,
                EnableDirectoryBrowsing = true
            });

            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<MessageHub>(MessageHub.HUB_URL);
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(null, "{controller}/{action}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
