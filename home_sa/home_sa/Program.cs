using home_sa.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using home_sa;
using home_sa.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    options.Lockout.MaxFailedAccessAttempts = 3;

    options.SignIn.RequireConfirmedEmail = true;
});

var app = builder.Build();


var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using (var scope = scopeFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

    bool adminExists = roleManager.RoleExistsAsync("Admin").Result;

    if (!adminExists)
    {
        roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
    }

    var adminUser = userManager.FindByEmailAsync("admin@mysite.com").Result;

    if (adminUser == null) // No admin user yet
    {
        var createUserIdentityResult = userManager.CreateAsync(
            new ApplicationUser()
            {
                UserName = "admin@mysite.com",
                Email = "admin@mysite.com"
            },
            "SecretPassword123!"
        ).Result;

        if (!createUserIdentityResult.Succeeded)
        {
            // user creation has failed!
            // try again or handle this issue
        }

        adminUser = userManager.FindByEmailAsync("admin@mysite.com").Result;
    }

    if (!userManager.IsInRoleAsync(adminUser, "Admin").Result)
    {
        var addRoleIdentityResult = userManager.AddToRoleAsync(adminUser, "Admin").Result;
    }

}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
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
