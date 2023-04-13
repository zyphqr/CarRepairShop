using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("EmployeeDataContextConnection") ?? throw new InvalidOperationException("Connection string 'EmployeeDataContextConnection' not found.");

builder.Services.AddDbContext<ShopDataContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Mechanic>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ShopDataContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CarsService>();
builder.Services.AddScoped<PartsService>();
builder.Services.AddScoped<RepairCardsService>();



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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
