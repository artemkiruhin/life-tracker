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
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration =  builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

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

app.UseAuthorization();

app.MapControllers();

app.Run();