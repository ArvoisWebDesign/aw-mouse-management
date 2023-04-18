using AspNetCore.SEOHelper;
using aw_mouse_management.Contexts;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Get the azure db connection string
var keyVaultUrl = new Uri(builder.Configuration.GetSection("KeyVaultURL").Value!);
var azureCredential = new DefaultAzureCredential();
builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);
var cs = builder.Configuration.GetSection("connectionstring").Value;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MouseContext>(options => options.UseSqlServer(cs));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Mouse/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseXMLSitemap(builder.Environment.ContentRootPath);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mouse}/{action=Index}/{id?}");

app.Run();
