//using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GestorPrestamos.Data.Extensions;
using GestorPrestamos.Data.Utils;
using GestorPrestamos.Domain.Utils;
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

builder.Services.ConfigureCaching(options =>
{
    //LO MALO DE ESTO ES QUE LA CONIFUGRACIÓN SOLO SE CARGA UNA VEZ, SI MODIFICO LAS OPCIONES, NO SE DAR'A CUENTA, YA QUE ESTO LEE SOLO UNA VEZ EL ARCHIVO, Y ES CUANDO INICIA LA APP
    options.SetLifetimeInSecondsForDeudoresDictionary(Convert.ToInt32(configuration.GetSection("CachingConfiguration:LifetimeInSecondsForDeudoresDictionary").Value));
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
    pattern: "{controller=Home}/{action=LoansToCollectIndex}/{id?}");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

app.Run();
