using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TP1_KarineDunberry.Areas.Identity.Data;
using TP1_KarineDunberry.Models;

namespace TP1_KarineDunberry
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
            services.AddDbContext<ZhaoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddAuthorization(options =>         
            {
                options.AddPolicy("AdministrateurSeulement", policy =>
                       policy.RequireRole("Administrateur"));
            });

    




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            CreateRoles(services).Wait();
        }

        private static void UpdateDatabase (IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService < IServiceScopeFactory >().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ZhaoContext>())
                {
                    context.Database.Migrate();
                }
                using (var context = serviceScope.ServiceProvider.GetService<IdentityDataContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        private static object IServiceScopeFactory()
        {
            throw new NotImplementedException();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Administrateur", "Utilisateur" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            IdentityUser user = await UserManager.FindByEmailAsync("courrielTI@cstjean.qc.ca");

            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = "courrielTI@cstjean.qc.ca",
                    Email = "courrielTI@cstjean.qc.ca",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                };
                await UserManager.CreateAsync(user, "179P@ssw0rd");
            }
            await UserManager.AddToRoleAsync(user, "Administrateur");
        }

    }
}
