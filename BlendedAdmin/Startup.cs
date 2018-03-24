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
                options.UseSqlite("Data Source=Database.db;");
            });
            services.AddScoped<IDomainContext, DomainContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IVariableRepository, VariableRepository>();
            services.AddScoped<IEnvironmentRepository, EnvironmentRepository>();
            services.AddTransient<IEnvironmentService, EnvironmentService>();
            services.AddTransient<ISiteMenuService, SiteMenuService>();
            services.AddTransient<IUrlServicecs, UrlServicecs>();
            services.AddTransient<IVariablesService, VariablesService>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IJsService, JsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvide, IHostingEnvironment env)
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
