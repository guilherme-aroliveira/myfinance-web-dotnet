using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet_infra;
using myfinance_web_dotnet_infra.Interfaces;
using myfinance_web_dotnet_infra.Repositories;
using myfinance_web_dotnet_service;
using myfinance_web_dotnet_service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Variáveis para a string de conexão
var server = builder.Configuration["DbServer"] ?? "myserversqlserver.database.windows.net";
var user = builder.Configuration["DbUser"] ?? "user";
var password = builder.Configuration["Password"] ?? "vb4wt3FS4w&H^u";
var database = builder.Configuration["Database"] ?? "myfinance";

// String de conexão
var connectionString = $"Server={server};Database={database};User Id={user};Password={password}";

builder.Services.AddDbContext<MyFinanceDbContext>(options => options.UseSqlServer(connectionString));

// Serviços injetados nas Controllers
builder.Services.AddScoped<IPlanoContaService, PlanoContaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

// Repositories
builder.Services.AddScoped<IPlanoContaRepository, PlanoContaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
