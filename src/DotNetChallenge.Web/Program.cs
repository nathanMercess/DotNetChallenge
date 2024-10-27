using Microsoft.Data.SqlClient;
using System.Data;
using DotNetChallenge.Infra.Implementations.Extensions;
using DotNetChallenge.Services.Implementations.Extensions;
using DotNetChallenge.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<CustomResponseMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=student}/{action=Index}/{id?}");

app.Run();