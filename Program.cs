using ST10442835_PROG6212_CMCS.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Register Azure Storage services with IConfiguration injection
builder.Services.AddSingleton<TableStorageService>(sp =>
    new TableStorageService(sp.GetRequiredService<IConfiguration>()));

builder.Services.AddSingleton<BlobService>(sp =>
    new BlobService(sp.GetRequiredService<IConfiguration>()));

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
