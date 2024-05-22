using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using loginDemo.Data;
using loginDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
var connectionString2 = builder.Configuration.GetConnectionString("MyConnection") ?? throw new InvalidOperationException("Connection string 'MyConnection' not found.");

builder.Services.AddDbContext<WebAppDataBaseContext>(options =>
    options.UseSqlServer(connectionString2));

builder.Services.AddRazorPages(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();


app.MapGet("/", async context =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        context.Response.Redirect("/Index");
    }
    else
    {
        context.Response.Redirect("/Identity/Account/Login");
    }
});

app.Run();
