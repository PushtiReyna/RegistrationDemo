using Microsoft.EntityFrameworkCore;
using Registration.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add Services For Session.
builder.Services.AddSession();


builder.Services.AddDbContext<RegistrationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DBConnectionString")
    ));

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
    pattern: "{controller=Registration}/{action=Register}/{id?}");

app.Run();
