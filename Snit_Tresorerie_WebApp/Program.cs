using NLog;
using Entities.Mappings;
using Snit_Tresorerie_WebApp.Extensions;
using Entities.Seeds;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment environment = builder.Environment;

LogManager.LoadConfiguration(Path.Combine(environment.WebRootPath, "logfiles", "nlog.config"));

builder.Services.ConfigureCors();
builder.Services.ConfigureSession();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryContext(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureRepositoryWrapper();
builder.Services.ConfigureCookieAuthentication(builder.Configuration);
builder.Services.ConfigureClaimPolicy();
builder.Services.ConfigureNewtonsoftJson();
builder.Services.ConfigureMailService(builder.Configuration);

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllersWithViews();


var app = builder.Build();


try
{

    await app.SeedDefaultRolesAsync();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Dashboard/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSession();

    //Global execption handling
    app.ConfigureCustomExceptionMiddleware();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}");

    app.Run();

    return 0;
}
catch (Exception e)
{
    //Log.Fatal(e, "Host terminated unexpectedly");

    return 1;
}
finally
{
    //Log.CloseAndFlush();
}
