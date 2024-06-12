using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Dierentuin_App.Data;
using Dierentuin_App.Seeding;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Dierentuin_AppContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Dierentuin_AppContext") ?? throw new InvalidOperationException("Connection string 'Dierentuin_AppContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<Dierentuin_App.Services.DayNightService>();

// Removes the required attribute for non-nullable reference types.
builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Dierentuin_AppContext>();

    // Ensure database is created
    context.Database.Migrate();

    // Seed the database
    var seeder = new DataSeeder(context);
    seeder.Seed();
}

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

// Make the application listen on port 80
app.Run("http://*:8080");
