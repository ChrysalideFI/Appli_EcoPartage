using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<Users, IdentityRole<int>>(options =>
{
    options.User.RequireUniqueEmail = false;
})
 .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();

    // Create Admin role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
    }

    if (!await roleManager.RoleExistsAsync("UserBlocked"))
    {
        await roleManager.CreateAsync(new IdentityRole<int>("UserBlocked"));
    }

    // Create a default admin user if it doesn't exist
    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
    if (adminUser == null)
    {
        adminUser = new Users { UserName = "admin@example.com", Email = "admin@example.com" };
        await userManager.CreateAsync(adminUser, "AdminPassword123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    // Ensure the admin user is validated
    if (!adminUser.IsValidated)
    {
        adminUser.IsValidated = true;
        await userManager.UpdateAsync(adminUser);
    }

    // Create 5 default user if it doesn't exist
    var defaultUser1 = await userManager.FindByEmailAsync("user1@test.com");
    var defaultUser2 = await userManager.FindByEmailAsync("user2@test.com");
    var defaultUser3 = await userManager.FindByEmailAsync("user3@test.com");
    var defaultUser4 = await userManager.FindByEmailAsync("user4@test.com");
    var defaultUser5 = await userManager.FindByEmailAsync("user5@test.com");
    // Crée un utilisateur non validé par l'admin (donc n'ayant pas accès à tous le site)
    if (defaultUser1 == null)
    {
        defaultUser1 = new Users { UserName = "user1@test.com", Email = "user1@test.com", IsValidated = false };
    await userManager.CreateAsync(defaultUser1, "UserPassword123!");
    }
    if (defaultUser2 == null)
    {
        defaultUser2 = new Users { UserName = "user2@test.com", Email = "user2@test.com", IsValidated = true };
        await userManager.CreateAsync(defaultUser2, "UserPassword123!");
    }
    if (defaultUser3 == null)
    {
        defaultUser3 = new Users { UserName = "user3@test.com", Email = "user3@test.com" };
        await userManager.CreateAsync(defaultUser3, "UserPassword123!");
    }
    if (defaultUser4 == null)
    {
        defaultUser4 = new Users { UserName = "user4@test.com", Email = "user4@test.com" };
        await userManager.CreateAsync(defaultUser4, "UserPassword123!");
    }
    if (defaultUser5 == null)
    {
        defaultUser5 = new Users { UserName = "user5@test.com", Email = "user5@test.com" };
        await userManager.CreateAsync(defaultUser5, "UserPassword123!");
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    app.Run();
}