using RoyalResidence.DTO;
using RoyalResidenceWeb.Services;
using RoyalResidenceWeb.Services.IServices;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<ResidenceCreateDTO, ResidenceDTO>().ReverseMap();
    o.CreateMap<ResidenceUpdateDTO, ResidenceDTO>().ReverseMap();
});

builder.Services.AddHttpClient("RoyalResidenceAPI", client =>
{
    var residenceAPIUrl = builder.Configuration.GetValue<string>("ServiceUrls:ResidenceAPI");
    client.BaseAddress = new Uri(residenceAPIUrl);
    client.DefaultRequestHeaders.Add("accept", "application/json");
});
builder.Services.AddScoped<IResidenceService, ResidenceService>();
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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
