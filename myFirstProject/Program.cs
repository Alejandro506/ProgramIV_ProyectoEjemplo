using Microsoft.EntityFrameworkCore;
using myFirstProject.Data;
using myFirstProject.MyInterfaces;
using myFirstProject.MyRepositories;

var builder = WebApplication.CreateBuilder(args);

// 📦 Leer configuración para elegir origen de datos
var useJson = builder.Configuration.GetValue<bool>("UseJson");

// 🗃️ Registrar DbContext (solo si se usará SQL)
builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

// 🧩 Inyectar implementación del repositorio de libros
if (useJson)
{
    var jsonPath = Path.Combine(builder.Environment.ContentRootPath, "libros.json");
    builder.Services.AddSingleton<ILibroRepository>(new JsonLibroRepository(jsonPath));
}
else
{
    builder.Services.AddScoped<ILibroRepository, SqlLibroRepository>();
}

// 🌐 Agregar cliente HTTP para API si se necesita
builder.Services.AddHttpClient();

// 🧠 Agregar soporte para controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 🔐 Manejo de errores y HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 🌍 Middlewares esenciales
app.UseHttpsRedirection();
app.UseStaticFiles(); // Asegura que pueda servir CSS, JS, etc.
app.UseRouting();
app.UseAuthorization();

// 🧭 Ruta por defecto: Libros/Buscar
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Libros}/{action=Buscar}/{id?}");

app.Run();
