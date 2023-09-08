using Microsoft.EntityFrameworkCore;
using Registration.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

IConfiguration configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json")
    .Build();
builder.Services.AddDbContext<RegistrationDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DBConnectionString")));

builder.Services.AddControllers();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
