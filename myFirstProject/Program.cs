using myFirstProject.Data;
using myFirstProject.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Read config
var useJson = builder.Configuration.GetValue<bool>("UseJson"); // Add this in appsettings.json

// Register DbContext
builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

// Register repositories
if (useJson)
{
    var jsonPath = Path.Combine(builder.Environment.ContentRootPath, "customers.json");
    builder.Services.AddSingleton<ICustomerRepository>(new JsonCustomerRepository(jsonPath));
}
else
{
    builder.Services.AddScoped<ICustomerRepository, SqlCustomerRepository>();
}

// Add HttpClient factory for API calls
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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


app.Run();
