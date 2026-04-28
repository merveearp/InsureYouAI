using InsureYouAI.Context;
using InsureYouAI.Describer;
using InsureYouAI.Entities;
using InsureYouAI.Extensions;
using InsureYouAI.Hubs;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddHttpClient("openai", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/");
});
builder.Services.AddRepositoriesExt();
builder.Services.AddDbContext<InsureContext>(options =>
{
    options
    .UseLazyLoadingProxies()
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    
});

builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 6;
        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    })
    .AddErrorDescriber<CustomIdentityErrorDescriber>()
    .AddEntityFrameworkStores<InsureContext>()
    .AddDefaultTokenProviders();


builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapHub<ChatHub>("/chathub");
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
