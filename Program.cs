using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("RentContext");
builder.Services.AddDbContext<RentContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

builder.Services.AddDefaultIdentity<User>(options => { })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<RentContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<RentContext>();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
