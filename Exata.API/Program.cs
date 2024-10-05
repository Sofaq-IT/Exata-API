using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exata.Domain.Interfaces;
using Exata.Helpers;
using Exata.Helpers.Interfaces;
using Exata.Repository.Context;
using Exata.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connSql = builder.Configuration.GetConnectionString("Default");
string sChave = builder.Configuration["Parametros:Chave"];
string sVetor = builder.Configuration["Parametros:Vetor"];

builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(connSql));

builder.Services.AddControllers();
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//    {
//        options.SuppressModelStateInvalidFilter = true;
//    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICampo, CampoRepository>();
builder.Services.AddScoped<ICripto>(sp => new Cripto(sChave, sVetor));
builder.Services.AddScoped<IContato, ContatoRepository>();
builder.Services.AddScoped<IContatoTipo, ContatoTipoRepository>();
builder.Services.AddScoped<IPerfil, PerfilRepository>();
builder.Services.AddScoped<IPerfilSecao, PerfilSecaoRepository>();
builder.Services.AddScoped<ISecao, SecaoRepository>();
builder.Services.AddScoped<ITelefones, TelefonesRepository>();
builder.Services.AddScoped<ITipoContato, TipoContatoRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApiContext>();
await dbContext.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
