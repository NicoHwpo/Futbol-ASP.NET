using Futbol.Models;
using Futbol.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<FutbolStoreDatabaseSettings>(
        builder.Configuration.GetSection(nameof(FutbolStoreDatabaseSettings)));

builder.Services.AddSingleton<IFutbolStoreDatabaseSettings>(provider =>
        provider.GetRequiredService<IOptions<FutbolStoreDatabaseSettings>>().Value);

builder.Services.AddSingleton<IEquipoService, EquipoService>();

builder.Services.AddControllers();
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

app.Run();
