using QuanLySinhVien.Models;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QuanLySinhVien.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using QuanLySinhVien.Attributes;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") + ";TrustServerCertificate=True";
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
       .AddEntityFrameworkStores<SchoolDbContext>()
       .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
    o.TokenLifespan = TimeSpan.FromHours(1));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StudentPolicy", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));

});

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
        Console.WriteLine("Admin Role Exists: " + adminRoleExists);
        if (!adminRoleExists)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var adminUser = await userManager.FindByEmailAsync("nguyenminhquank14@siu.edu.vn");
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = "Nguyen Minh Quan",
                Email = "nguyenminhquank14@siu.edu.vn",
            };
            await userManager.CreateAsync(adminUser, "ilovefemboy");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.SlidingExpiration = true;
    });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.UseSession();

app.MapControllerRoute(
        name: "AdminLogin",
        pattern: "Admin/Login",
        defaults: new { controller = "Home", action = "AdminLogin" });

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Home" });

app.Run();
