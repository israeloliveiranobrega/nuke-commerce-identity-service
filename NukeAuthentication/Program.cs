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

#region Cookies configuration
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

if (allowedOrigins == null || allowedOrigins.Length == 0)//sem sites cadastrados estora erro
{
    throw new InvalidOperationException("A configuração de sites permitidos não foi encontrada ou está vazia. Verifique as Variáveis de Ambiente.");
}

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowFrontend", Policy =>
    {
        Policy.WithOrigins(allowedOrigins)// só esses sites enviam cookies para aqui
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();// permite passar cookies
    });
});
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
    options.TokenValidationParameters = new TokenValidationParameters() // valida o token para ser usado comparando as informações do token
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

    options.Events = new JwtBearerEvents //o ASP.NET precisa disso para ler http only, sem isso dá 401/403 em todas as rotas
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("access_token")) //lé o cookie enviado e só aceita com esse nome
            {
                context.Token = context.Request.Cookies["access_token"];
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorizationBuilder()

    .AddPolicy("RequireUser", policy => policy.RequireRole(
        nameof(AccessLevel.User),
        nameof(AccessLevel.VerifiedUser),
        nameof(AccessLevel.Support),
        nameof(AccessLevel.Admin),
        nameof(AccessLevel.Master))) // usuario é criado com esse

    .AddPolicy("RequireVerifiedUser", policy => policy.RequireRole(
        nameof(AccessLevel.VerifiedUser),
        nameof(AccessLevel.Support),
        nameof(AccessLevel.Admin),
        nameof(AccessLevel.Master)))

    .AddPolicy("RequireSupport", policy => policy.RequireRole(
        nameof(AccessLevel.Support),
        nameof(AccessLevel.Admin),
        nameof(AccessLevel.Master)))

    .AddPolicy("RequireAdmin", policy => policy.RequireRole(
        nameof(AccessLevel.Admin),
        nameof(AccessLevel.Master)))

    .AddPolicy("RequireMaster", policy => policy.RequireRole(nameof(AccessLevel.Master))); //Apenas devs
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

app.UseCors("AllowFrontend"); // permite o preflight options funcionar sem barrar(pois n tem credenciais)

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion