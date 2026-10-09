using Microsoft.EntityFrameworkCore;
using RoyalResidence_API.Controllers.Data;
using RoyalResidence_API.Models;
using RoyalResidence_API.Models.DTO;
using RoyalResidence_API.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<ResidenceCreateDTO, Residence>().ReverseMap();
    o.CreateMap<ResidenceUpdateDTO, Residence>().ReverseMap();
    o.CreateMap<Residence, ResidenceDTO>().ReverseMap();
    o.CreateMap<ResidenceUpdateDTO, ResidenceDTO>().ReverseMap();
    o.CreateMap<User, UserDTO>().ReverseMap();
});

builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();
await SeedDataAsync(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();
}