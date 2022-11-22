using Backend.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Backend
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddScoped<IdentityController, IdentityController>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("DataSource=database.sqlite")
            );

            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
            await SeedUsers(serviceProvider);
        }

        private async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var admin = new IdentityUser { UserName = "admin" };
            //var pass = Environment.GetEnvironmentVariable("ASPNETCORE_SAMPLE_ADMIN_PASS");
            var pass = System.IO.File.ReadAllText("_");
            await userManager.CreateAsync(admin, pass);

            var user = new IdentityUser("test");
            await userManager.CreateAsync(user, "test");
        }
    }
}