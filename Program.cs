using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("UniversityConnection") ?? throw new InvalidOperationException("Connection string 'UniversityConnection' not found.");
builder.Services.AddDbContext<UniversityDbContext>(options =>
    options.UseSqlServer(connectionString));

connectionString = builder.Configuration.GetConnectionString("UsersConnection") ?? throw new InvalidOperationException("Connection string 'UsersConnection' not found.");
builder.Services.AddDbContext<ApplicationUsersDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationUsersDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();

    using (var servicesScope = app.Services.CreateScope()) {
        var db = servicesScope.ServiceProvider.GetService<UniversityDbContext>();
        SeedData.Populate(db);
    }
} else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
