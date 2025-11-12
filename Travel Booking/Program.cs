using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Travel_Booking.Data;

var builder = WebApplication.CreateBuilder(args);

// ------------------------
// 1. Add services to the container
// ------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add developer exception page for database errors
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity with Roles support
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>() // enable roles
.AddEntityFrameworkStores<ApplicationDbContext>();

// Add MVC controllers with views
builder.Services.AddControllersWithViews();

// Add Razor Pages (needed for Identity UI)
builder.Services.AddRazorPages();

// ------------------------
// 2. Build the app
// ------------------------
var app = builder.Build();

// ------------------------
// 3. Configure the HTTP request pipeline
// ------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Enable HTTPS and static files
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

// ------------------------
// 4. Map routes
// ------------------------

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Identity UI (Login, Register, etc.)
app.MapRazorPages();

app.Run();
