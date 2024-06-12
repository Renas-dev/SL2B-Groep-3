using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Dierentuin_App.Data;
using Dierentuin_App.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Dierentuin_AppContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Dierentuin_AppContext") ?? throw new InvalidOperationException("Connection string 'Dierentuin_AppContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<Dierentuin_App.Services.DayNightService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "stalls",
    pattern: "{controller=Stalls}/{action=Index}/{id?}");

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

// Make the application listen on port 80
app.Run("http://*:8080");
