using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//Abajo Culture-Info (Para validación de rangos de fechas?)
//var cultureInfo = new CultureInfo("es-ES");
//CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
//CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BdfflContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

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
