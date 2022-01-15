using GestorPrestamos.Data.Extensions;
using GestorPrestamos.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.RegisterMyCustomServices();
builder.Services.ConfigureExcelRepository(
    options =>
    {//TODO: Ponerlo en AppSettings, Crear un modelo y bindearlo
        //options.FilePath = @"G:\Temporales\GestorPrestamosTest.xlsx";
        options.FilePath = @"C:\Users\arthu\OneDrive\GestorPrestamosTest.xlsx";
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
