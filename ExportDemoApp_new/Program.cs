using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan service controller + views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing
app.UseRouting();

app.UseAuthorization();

// Default route (arahkan ke controller Export)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Export}/{action=Index}/{id?}");

// Konfigurasi Rotativa untuk PDF
RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");




app.Run();