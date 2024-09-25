using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuentas/Login"; // Página de inicio de sesión
        options.AccessDeniedPath = "/Cuentas/AccessDenied"; // Página de acceso denegado
    });


// Configurar las opciones de las cookies de Identity para usar las rutas personalizadas
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Cuentas/Login"; // Página de inicio de sesión personalizada
    options.AccessDeniedPath = "/Cuentas/AccessDenied"; // Página de acceso denegado personalizada
});

// Add services to the container.
builder.Services.AddControllersWithViews();

DotNetEnv.Env.Load();

builder.Services.AddDbContext<BdfflContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

    //options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_STRING"))
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
