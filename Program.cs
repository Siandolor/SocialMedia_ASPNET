// ==========================================================
//  PROGRAM.CS
//  Entry point for the SocialMedia (Tweeble) ASP.NET Core MVC app.
//
//  Purpose:
//  • Configures services, middleware, and app startup.
//  • Registers Entity Framework Core (SQLite) and ASP.NET Identity.
//  • Defines routing and HTTP request pipeline.
//
//  Framework: .NET 6+ (Minimal Hosting Model)
// ==========================================================

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Models;

// ----------------------------------------------------------
//  APPLICATION BUILDER INITIALIZATION
//  Creates the WebApplication host with default configuration.
// ----------------------------------------------------------
var builder = WebApplication.CreateBuilder(args);


// ==========================================================
//  DATABASE CONFIGURATION
//  Register Entity Framework Core with a SQLite database.
//  Database file: SocialMedia.db (local persistence)
// ==========================================================
const string connectionString = "Data Source=SocialMedia.db";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));


// ==========================================================
//  IDENTITY CONFIGURATION
//  Registers ASP.NET Core Identity with ApplicationUser model.
//
//  Features:
//  • Password complexity enforcement
//  • Unique email requirement
//  • Local sign-in without confirmation
// ==========================================================
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();


// ==========================================================
//  COOKIE CONFIGURATION
//  Defines authentication cookie paths, expiration policy,
//  and security-related HTTP-only restrictions.
// ==========================================================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});


// ==========================================================
//  MVC + CONTROLLERS
//  Registers controllers and Razor views with MVC support.
// ==========================================================
builder.Services.AddControllersWithViews();


// ----------------------------------------------------------
//  APPLICATION BUILD
//  Compiles the service container and middleware pipeline.
// ----------------------------------------------------------
var app = builder.Build();


// ==========================================================
//  DATABASE MIGRATION AT STARTUP
//  Applies any pending EF Core migrations automatically.
//  Ensures DB schema is up-to-date before serving requests.
// ==========================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}


// ==========================================================
//  ERROR HANDLING & SECURITY
//  Enables HSTS and custom error page for production.
// ==========================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// ==========================================================
//  MIDDLEWARE PIPELINE
//  Defines HTTP processing order: HTTPS → Static Files → Routing
//  → Authentication → Authorization.
// ==========================================================
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// ==========================================================
//  ROUTING CONFIGURATION
//  Sets default route: Home/Index/{id?}
// ==========================================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ==========================================================
//  APPLICATION START
//  Launches the web server.
// ==========================================================
app.Run();
