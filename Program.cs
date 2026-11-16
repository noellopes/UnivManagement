using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

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
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    using (var servicesScope = app.Services.CreateScope())
    {
        var db = servicesScope.ServiceProvider.GetRequiredService<UniversityDbContext>();

        // Garante que a base de dados está criada e aplica o Seed Data
        db.Database.EnsureCreated();

        if (!db.Estudantes.Any())
        {
            db.Estudantes.AddRange(
                new Estudante { Nome = "João Pereira", Email = "joao.pereira@universidade.pt", Curso = "Engenharia Informática", SituacaoProfissional = "Estudante" },
                new Estudante { Nome = "Sofia Martins", Email = "sofia.martins@universidade.pt", Curso = "Engenharia Informática", SituacaoProfissional = "Estudante" }
            );
            db.SaveChanges();
        }
    }
}
else
{
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
