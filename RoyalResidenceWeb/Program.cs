using Microsoft.AspNetCore.Authentication.Cookies;
using RoyalResidence.DTO;
using RoyalResidenceWeb.Services;
using RoyalResidenceWeb.Services.IServices;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<ResidenceCreateDTO, ResidenceDTO>().ReverseMap();
    o.CreateMap<ResidenceUpdateDTO, ResidenceDTO>().ReverseMap();
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/accessdenied";
    });
builder.Services.AddHttpClient("RoyalResidenceAPI", client =>
{
    var residenceAPIUrl = builder.Configuration.GetValue<string>("ServiceUrls:ResidenceAPI");
    client.BaseAddress = new Uri(residenceAPIUrl);
    client.DefaultRequestHeaders.Add("accept", "application/json");
});
builder.Services.AddScoped<IResidenceService, ResidenceService>();
builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
