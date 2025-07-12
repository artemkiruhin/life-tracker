using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LifeTrack.Core.Interfaces;
using LifeTrack.Core.Interfaces.Repositories;
using LifeTrack.Core.Interfaces.Services.Entity;
using LifeTrack.Core.Interfaces.Services.Mappers;
using LifeTrack.Core.Interfaces.Services.Security;
using LifeTrack.Core.Models.Contracts;
using LifeTrack.Infractructure;
using LifeTrack.Infractructure.Repositories;
using LifeTrack.Services.Entity;
using LifeTrack.Services.Mapper;
using LifeTrack.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration =  builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"] ??
                                                                               throw new ApplicationException(
                                                                                   "SecurityKey is missing.")))
        };
        opt.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var jwt = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(jwt)) context.Token = jwt;
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                try
                {
                    var claims = context.Principal.Claims;
                    var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (Guid.TryParse(userIdClaim?.Value, out Guid userId))
                    {
                        var identity = context.Principal.Identity as ClaimsIdentity;
                        if (identity != null)
                        {
                            if (!identity.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("--------ERROR--------");
                        Console.WriteLine("Failed auth\n\n\n");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    context.Fail("Invalid token");
                }

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000");
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowCredentials();
    });
});

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(configuration.GetConnectionString("Database")).UseLazyLoadingProxies();
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IKanbanCategoryRepository, KanbanCategoryRepository>();
builder.Services.AddScoped<IKanbanTaskRepository, KanbanTaskRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IKanbanCategoryService, KanbanCategoryService>();
builder.Services.AddScoped<IKanbanTaskService, KanbanTaskService>();
builder.Services.AddScoped<IHashingService, Sha256Hasher>();
builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();
builder.Services.AddScoped<IKanbanCategoryMapper, KanbanCategoryMapper>();
builder.Services.AddScoped<IKanbanTaskMapper, KanbanTaskMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IUserSecurityService, UserSecurityService>();
builder.Services.AddScoped<IJwtService>(opt =>
{
    var settings = new JwtSettings(
        Audience: configuration["JWT:Audience"] ?? throw new ApplicationException("Missing JWT:Audience"),
        Issuer: configuration["JWT:Issuer"] ?? throw new ApplicationException("Missing JWT:Issuer"),
        SecurityKey: configuration["JWT:Secret"] ?? throw new ApplicationException("Missing JWT:Secret"),
        ExpiresInHours: int.Parse(configuration["JWT:ExpiresInHours"] ?? throw new ApplicationException("Missing JWT:ExpiresInHours"))
    );
    return new JwtService(settings);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();