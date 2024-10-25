using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Exata.API.Filters;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers;
using Exata.Helpers.Interfaces;
using Exata.Repository.Context;
using Exata.Repository.Repositories;
using Exata.Domain.DTO;

var builder = WebApplication.CreateBuilder(args);

string connSql = builder.Configuration.GetConnectionString("Default");
string sChave = builder.Configuration["Parametros:Chave"];
string sVetor = builder.Configuration["Parametros:Vetor"];
string sServer = builder.Configuration["Parametros:BD:Server"];
string sBanco = builder.Configuration["Parametros:BD:DataBase"];
string sUser = builder.Configuration["Parametros:BD:User"];
string sPass = builder.Configuration["Parametros:BD:Pass"];
string sCripto = builder.Configuration["Parametros:BD:Cripto"];
string sSecretKey = builder.Configuration["JWT:SecretKey"]
                   ?? throw new ArgumentException("Chave Secreta Inválida!");
string sValidAudience = builder.Configuration["JWT:ValidAudience"];
string sValidIssuer = builder.Configuration["JWT:ValidIssuer"];
string sCriptoToken = builder.Configuration["JWT:Cripto"];
string sTokenValidityInMinutes = builder.Configuration["JWT:TokenValidityInMinutes"];
string sRefreshTokenValidityInMinutes = builder.Configuration["JWT:RefreshTokenValidityInMinutes"];
string sUsuarioADM = builder.Configuration["Parametros:UsuarioADM"];
string sLogarRequisicoes = builder.Configuration["Parametros:LogarRequisicoes"];
string sLicenca = builder.Configuration["Parametros:Licenca"];

if (sCripto.ToLower().Trim() == "true")
{
    var _cri = new Cripto(sChave, sVetor);
    sServer = _cri.Descriptografar(sServer);
    sBanco = _cri.Descriptografar(sBanco);
    sUser = _cri.Descriptografar(sUser);
    sPass = _cri.Descriptografar(sPass);
}

connSql = connSql
    .Replace("{parServer}", sServer)
    .Replace("{parData}", sBanco)
    .Replace("{parUser}", sUser)
    .Replace("{parPass}", sPass);

if (sCriptoToken.ToLower().Trim() == "true")
{
    var _cri = new Cripto(sChave, sVetor);
    sSecretKey = _cri.Descriptografar(sSecretKey);
}

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApiContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(connSql));

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
    options.Filters.Add(typeof(PermissaoFilter)); 
} )
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("x-Paginacao");
    });
});

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//    {
//        options.SuppressModelStateInvalidFilter = true;
//    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Exata API", Version = "v1" });

    c.IncludeXmlComments($@"{AppDomain.CurrentDomain.BaseDirectory}\Exata.API.xml");

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT ",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});

//builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = sValidAudience,
        ValidIssuer = sValidIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sSecretKey))
    };
});

builder.Services.AddScoped<ICampo, CampoRepository>();
builder.Services.AddScoped<ICliente, ClienteRepository>();
builder.Services.AddScoped<IControllerAction, ControllerActionRepository>();
builder.Services.AddScoped<ICripto>(sp => new Cripto(sChave, sVetor));
builder.Services.AddScoped<IEmail, Email>();
builder.Services.AddScoped<IErrorRequest, ErrorRequest>();
builder.Services.AddScoped<IFuncoes, Funcoes>();
builder.Services.AddScoped<ILogRequisicao, LogRequisicoesRepository>();
builder.Services.AddScoped<IToken, Token>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IVariaveisAmbiente>(sp => new VariaveisAmbiente(
    sSecretKey,
    sValidAudience,
    sValidIssuer,
    sTokenValidityInMinutes,
    sRefreshTokenValidityInMinutes,
    sUsuarioADM,
    sLogarRequisicoes,
    sLicenca));

builder.Services.Configure<SmtpSettingsDTO>(builder.Configuration.GetSection("SmtpConfiguration"));

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApiContext>();
await dbContext.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(s => {
        s.DocumentTitle = app.Environment.EnvironmentName.ToString() + " - API Exata";
    });
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
