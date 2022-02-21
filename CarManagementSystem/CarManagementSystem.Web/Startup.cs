using CarManagementSystem.Data.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarManagementSystem.Service.Services;
using CarManagementSystem.Service.Helper;


namespace CarManagementSystem.Web
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
            services.AddHttpContextAccessor();
           
            services.AddMvc();
            services.AddDbContext<CarManagementSystemDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("connection")));
           services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<CarManagementSystemDbContext>();//Register Identity services
           services.AddTransient<AccountService>();
           services.AddTransient<CarService>();
            services.AddTransient<ModelService>();
            services.AddTransient<SubModelService>();
            services.AddTransient<ImageService>();
            services.AddScoped<IUserService, UserService>();
           //services.AddDistributedMemoryCache();
           //services.AddSession();
           services.AddControllersWithViews();
           services.AddRazorPages(); 
            services.AddControllers();


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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
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
        }
    }
}
