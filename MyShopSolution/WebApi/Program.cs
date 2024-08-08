using Core;
using Core.Interface;
using Core.Models;
using Data;
using Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, DbProductService>();
builder.Services.AddScoped<ICategoryService, DbCategoryService>();

//builder.Services.AddHttpClient<IProductService, HttpProductService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7057");
//});
//builder.Services.AddHttpClient<ICategoryService, HttpCategoryService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7057");
//});

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.InitializeAsync(context);

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    await SeedRolesAndAdminAsync(roleManager, userManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole<Guid>> roleManager, UserManager<User> userManager)
{
    string adminRole = "Admin";
    string userRole = "User";
    string adminEmail = "admin@myshop.com";
    string adminPassword = "Admin@123";

    // Add the admin role if it doesn't exist
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        var adminRoleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(adminRole));
        if (adminRoleResult.Succeeded)
        {
            Console.WriteLine($"Role '{adminRole}' created.");
        }
        else
        {
            foreach (var error in adminRoleResult.Errors)
            {
                Console.WriteLine($"Error creating role '{adminRole}': {error.Description}");
            }
        }
    }

    // Add the user role if it doesn't exist
    if (!await roleManager.RoleExistsAsync(userRole))
    {
        var userRoleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(userRole));
        if (userRoleResult.Succeeded)
        {
            Console.WriteLine($"Role '{userRole}' created.");
        }
        else
        {
            foreach (var error in userRoleResult.Errors)
            {
                Console.WriteLine($"Error creating role '{userRole}': {error.Description}");
            }
        }
    }

    // Create the admin user if it doesn't exist
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            var roleResult = await userManager.AddToRoleAsync(adminUser, adminRole);
            if (roleResult.Succeeded)
            {
                Console.WriteLine($"Admin user created with email '{adminEmail}' and assigned role '{adminRole}'.");
            }
            else
            {
                foreach (var error in roleResult.Errors)
                {
                    Console.WriteLine($"Error adding role to admin user: {error.Description}");
                }
            }
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Failed to create admin user: {error.Description}");
            }
        }
    }
    else
    {
        Console.WriteLine($"Admin user with email '{adminEmail}' already exists.");
    }
}
