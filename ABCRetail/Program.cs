using ABCRetail.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// We are attempting to register services

builder.Services.AddTransient<AzureTableService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<AzureTableService>>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetValue<string>("AzureStorage:ConnectionString"); // Correctly fetch the connection string
    Console.WriteLine($"Connection String: {connectionString}");
    return new AzureTableService(configuration, logger);
});



// Register BlobStorageService
builder.Services.AddTransient<BlobStorageService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetValue<string>("AzureStorage:ConnectionString");

    // Check if the connection string is null or empty
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Azure Storage Connection String is not configured.");
    }
    
    return new BlobStorageService(connectionString);
});
// Add services to the container.
builder.Services.AddControllersWithViews();

//// Register AzureTableService for Dependency Injection (its breaking why!!!)
//builder.Services.AddScoped<ABCRetail.Services.AzureTableService>();

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
