using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Dierentuin_App.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<Dierentuin_AppContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Dierentuin_AppContext")));

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<Dierentuin_App.Services.DayNightService>();

// Removes the required attribute for non-nullable reference types
builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{ // Create a new scope for the service provider
    var context = scope.ServiceProvider.GetRequiredService<Dierentuin_AppContext>();
    context.SeedDatabase();
}

// Configure the HTTP request pipeline
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

// Make the application listen on port 8080
app.Run("http://*:8080");
