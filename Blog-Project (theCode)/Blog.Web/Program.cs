using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Blog.DataAccess.Data;
using Blog.DataAccess.Implementation;
using Blog.Entities.Repositories;
using Blog.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace Blog.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<IdentityUser , IdentityRole>().AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<AppDbContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Visitor}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
