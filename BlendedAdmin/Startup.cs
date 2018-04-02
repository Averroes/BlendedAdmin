using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlendedAdmin.DomainModel.Items;
using BlendedAdmin.DomainModel;
using BlendedAdmin.Data;
using Microsoft.EntityFrameworkCore;
using BlendedAdmin.Services;
using BlendedAdmin.DomainModel.Variables;
using Newtonsoft.Json.Serialization;
using BlendedAdmin.DomainModel.Environments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using BlendedAdmin.Js;
using BlendedAdmin.DomainModel.Users;
using Microsoft.AspNetCore.Identity;
using BlendedAdmin.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using BlendedJS;
using Microsoft.Extensions.Logging;
using BlendedAdmin.Infrastructure.Logging;

namespace BlendedAdmin
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
            services
                .AddMvc(x => x.Filters.Add(typeof(EnvironmentFilter)))
                .AddJsonOptions(x => x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddDbContext<ApplicationDbContext>(options => {
                var database = Configuration.GetConnectionString("Database");
                var provider = Configuration.GetConnectionString("Provider");
                if (provider.SafeEquals("Sqlite"))
                    options.UseSqlite(database);
                if (provider.SafeEquals("SqlServer"))
                    options.UseSqlServer(database);
                if (provider.SafeEquals("MySQL"))
                    options.UseMySql(database);
                if (provider.SafeEquals("PostgreSQL") || provider.SafeEquals("Postgres"))
                    options.UseNpgsql(database);
            });
            services.AddScoped<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddScoped<IDomainContext, DomainContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IVariableRepository, VariableRepository>();
            services.AddScoped<IEnvironmentRepository, EnvironmentRepository>();
            services.AddTransient<IEnvironmentService, EnvironmentService>();
            services.AddTransient<ISiteMenuService, SiteMenuService>();
            services.AddTransient<IUrlService, UrlService>();
            services.AddTransient<IVariablesService, VariablesService>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IJsService, JsService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/users/login";
                options.LogoutPath = "/users/logoff";
                options.AccessDeniedPath = "/accessdenied";
            });
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options => {
            //        options.LoginPath = "/{environment}/login";
            //        options.LogoutPath = "/{environment}/logoff";
            //        options.AccessDeniedPath = "/{environment}accessdenied";
            //    });
            services.AddOptions();
            services.Configure<BlendedSettings>(Configuration.GetSection("BlendedSettings"));
            services.Configure<MailSettings>(Configuration.GetSection("Mail"));
            services.Configure<FileLoggerOptions>(Configuration.GetSection("Logging:File"));
            services.Configure<ElasticLoggerOptions>(Configuration.GetSection("Logging:Elastic"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvide, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{environment=Default}/{controller=Home}/{action=Index}/{id?}");
            });

            using (ApplicationDbContext dbContext = serviceProvide.GetService<ApplicationDbContext>())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
