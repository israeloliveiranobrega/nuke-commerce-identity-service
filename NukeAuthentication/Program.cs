using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NukeAuthentication.Scr.Domain.ValueObjects.Base.Enums;
using NukeAuthentication.Scr.Features.AuthenticationFeatures.JasonWebTokenGenerator;
using NukeAuthentication.Scr.Infraestructure;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Contracts;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations;
using NukeAuthentication.Scr.Infraestructure.Repositorys.AuthenticationRepositorys.Implementations.UserRepo;

var builder = WebApplication.CreateBuilder(args);

#region EntityFramework Configurations
var connectionString = builder.Configuration.GetConnectionString("SafePlaceConnection");

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
#endregion

#region Route Configurations
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
#endregion

#region Dependencies injection
//builder.Services.AddApplication(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddTransient<IUserSessionRepository, UserSessionRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
#endregion

#region JasonWebToken configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var secretKey = builder.Configuration["Jwt:SecretKey"];

if (string.IsNullOrEmpty(secretKey))//jwt nulo estora erro
{
    throw new InvalidOperationException("A configuração 'Jwt:SecretKey' não foi encontrada. Verifique as Variáveis de Ambiente (Jwt__SecretKey).");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorizationBuilder()
    // Nível Usuário (Todo mundo logado entra)
    .AddPolicy("RequireUser", policy => policy.RequireRole(
        nameof(AccessLevel.User),
        nameof(AccessLevel.VerifiedUser),
        nameof(AccessLevel.Support),
        nameof(AccessLevel.Admin),
        nameof(AccessLevel.Master)))

    // Nível Verificado (Verificado e acima entram)
    .AddPolicy("RequireVerifiedUser", policy => policy.RequireRole(
        nameof(AccessLevel.VerifiedUser),
        nameof(AccessLevel.Support),
        nameof(AccessLevel.Admin),
        nameof(AccessLevel.Master)))

    // Nível Suporte (Suporte e acima entram)
    .AddPolicy("RequireSupport", policy => policy.RequireRole(
        nameof(AccessLevel.Support),
        nameof(AccessLevel.Admin),
        nameof(AccessLevel.Master)))

    // Nível Admin (Admin e Master entram)
    .AddPolicy("RequireAdmin", policy => policy.RequireRole(
        nameof(AccessLevel.Admin),
        nameof(AccessLevel.Master)))

    // Nível Master (Só Master)
    .AddPolicy("RequireMaster", policy => policy.RequireRole(
        nameof(AccessLevel.Master)));
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region Swaager Configurarion
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Nuke Authentication API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new()
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "Jwt",
        Scheme = "Bearer",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            new List<string>()
        },
    });
});
#endregion

var app = builder.Build();

#region HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

#region others
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion