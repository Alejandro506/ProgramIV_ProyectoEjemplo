using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;      // AppDbContext (scaffold)
using myFirstProject.Repository;       // ICustomerRepository, JsonCustomerRepository, SqlCustomerRepository
using myFirstProject.Services;         // CitasService (SP con Dapper)

var builder = WebApplication.CreateBuilder(args);

// ----- Flags de configuración -----
var useJson = builder.Configuration.GetValue<bool>("UseJson");

// ----- EF Core: DbContext -----
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Falta ConnectionStrings:Default en appsettings.json (o Secret Manager).");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ----- Repositorios -----
if (useJson)
{
    var jsonPath = Path.Combine(builder.Environment.ContentRootPath, "customers.json");
    builder.Services.AddSingleton<ICustomerRepository>(new JsonCustomerRepository(jsonPath));
}
else
{
    builder.Services.AddScoped<ICustomerRepository, SqlCustomerRepository>();
}

// ----- Servicios (SP de Citas con Dapper) -----
builder.Services.AddScoped<CitasService>();

// HTTP Client (si consumes APIs externas)
builder.Services.AddHttpClient();

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ----- Pipeline HTTP -----
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

// Endpoint de verificación rápida (DI + conexión)
app.MapGet("/pingdb", async (AppDbContext db) =>
{
    var any = await db.Clientes.Take(1).ToListAsync();
    return Results.Ok(new { ok = true, rows = any.Count });
});

app.Run();
