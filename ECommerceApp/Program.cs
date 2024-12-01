using ECommerceApp.Data;
using ECommerceApp.Helpers;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddLogging();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
});

builder.Services.AddTransient<EmailService>();


//controllers with views, using ReferenceHandler.Preserve to handle circular references

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

var app = builder.Build();



// Middleware configuration
if (app.Environment.IsDevelopment())
{
    // In development, show detailed error pages
    app.UseDeveloperExceptionPage();
}
else
{
    // In production, redirect to a generic error page
    app.UseExceptionHandler("/Home/Error");
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    await SeedRolesAndAdminUser(roleManager, userManager, logger);
}

app.Run();

async Task SeedRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<Program> logger)
{
    var roles = new[] { "SiteOwner", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
            logger.LogInformation("Role {Role} created.", role);
        }
    }

    var adminEmail = "kaifk8402@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = "Itskaif07",
            Email = adminEmail,
            Name = "Md Kaif",
            PhoneNumber = "7257068423",
            Address = "B.M Das Road, Purandarpur gali, Devi Asthan",
            City = "Patna",
            State = "Bihar",
            PinCode = "800004"
        };
        var createAdminUser = await userManager.CreateAsync(adminUser, "ErenJaeger@10");
        if (createAdminUser.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "SiteOwner");
            logger.LogInformation("Admin user created and assigned to role SiteOwner.");
        }
        else
        {
            foreach (var error in createAdminUser.Errors)
            {
                logger.LogError("Error creating user: {Error}", error.Description);
            }
        }
    }
}
