using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddSession();

builder.Services.AddMvc();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Login/Index";
});

var is64Bit = IntPtr.Size == 8;
var platform = is64Bit ? "x64" : "x86";

var leptonicaPath = Path.Combine(Directory.GetCurrentDirectory(), "runtimes", platform, "native", "libleptonica.6.dylib");
var tesseractPath = Path.Combine(Directory.GetCurrentDirectory(), "runtimes", platform, "native", "libtesseract.5.dylib");

if (!File.Exists(leptonicaPath) || !File.Exists(tesseractPath))
{
    throw new DllNotFoundException($"Failed to find required libraries: {leptonicaPath} or {tesseractPath}");
}

NativeLibrary.SetDllImportResolver(typeof(Program).Assembly, (libraryName, assembly, searchPath) =>
{
    if (libraryName == "libleptonica-1.82.0.dylib")
    {
        return NativeLibrary.Load(leptonicaPath);
    }
    else if (libraryName == "libtesseract-5.3.0.dylib")
    {
        return NativeLibrary.Load(tesseractPath);
    }
    return IntPtr.Zero;
});


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

app.UseSession();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();