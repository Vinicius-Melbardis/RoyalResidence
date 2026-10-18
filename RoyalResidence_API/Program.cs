using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RoyalResidence_API.Controllers.Data;
using RoyalResidence_API.Models;
using RoyalResidence_API.Models.DTO;
using RoyalResidence_API.Services;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtSettings")["Secret"]);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter JWT Bearer token"
            }
        };

        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                {new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
            }
        ];

        return Task.CompletedTask;
    });
});

builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<ResidenceCreateDTO, Residence>().ReverseMap();
    o.CreateMap<ResidenceUpdateDTO, Residence>().ReverseMap();
    o.CreateMap<Residence, ResidenceDTO>().ReverseMap();
    o.CreateMap<ResidenceUpdateDTO, ResidenceDTO>().ReverseMap();
    o.CreateMap<User, UserDTO>().ReverseMap();
    o.CreateMap<ResidenceAmenitiesCreateDTO, ResidenceAmenities>().ReverseMap();
    o.CreateMap<ResidenceAmenitiesUpdateDTO, ResidenceAmenities>().ReverseMap();
    o.CreateMap<ResidenceAmenitiesDTO, ResidenceAmenities>();
    o.CreateMap<ResidenceAmenities, ResidenceAmenitiesDTO>()
    .ForMember(dest => dest.ResidenceName, opt => opt.MapFrom(src => src.Residence!=null? src.Residence.Name : null));
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();
}