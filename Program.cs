using ST10442835_PROG6212_CMCS.Services;
using ST10442835_PROG6212_CMCS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Register services with proper configuration
builder.Services.AddSingleton<IClaimService, JsonClaimService>();
builder.Services.AddSingleton<IFileStorageService, LocalFileStorageService>();

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "CMCS.Session";
});

// Add HttpContextAccessor for session access in services
builder.Services.AddHttpContextAccessor();

// Add configuration
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // Development-specific middleware
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication (simple session-based)
app.UseAuthorization();

// Session middleware must be after UseRouting and before UseEndpoints
app.UseSession();

// Custom error handling middleware
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Home/Error";
        await next();
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Additional routes for better navigation
app.MapControllerRoute(
    name: "dashboard",
    pattern: "dashboard/{action=Index}",
    defaults: new { controller = "Dashboard" });

app.MapControllerRoute(
    name: "claims",
    pattern: "claims/{action=Index}",
    defaults: new { controller = "Claims" });

app.MapControllerRoute(
    name: "login",
    pattern: "login/{action=Index}",
    defaults: new { controller = "Login" });

app.Run();