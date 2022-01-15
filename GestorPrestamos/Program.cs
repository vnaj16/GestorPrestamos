using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GestorPrestamos.Data.Extensions;
using GestorPrestamos.Extensions;
using GestorPrestamos.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.RegisterMyCustomServices();
ConfigurationManager configuration = builder.Configuration;
builder.Services.Configure<ExcelConfiguration>(configuration.GetSection("ExcelConfiguration"));
builder.Services.AddScoped(sp => sp.GetService<IOptionsSnapshot<ExcelConfiguration>>().Value);

builder.Services.ConfigureExcelRepository(
    options =>
    {
        options.FilePath = configuration.GetSection("ExcelConfiguration:FilePath").Value;
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

app.Run();
