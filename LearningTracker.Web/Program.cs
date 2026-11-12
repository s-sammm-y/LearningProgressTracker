using LearningTracker.Data;
using LearningTracker.Models.Entities;
using LearningTracker.Repositories.Implementations;
using LearningTracker.Repositories.Interfaces;
using LearningTracker.Services.Implementations;
using LearningTracker.Services.Interfaces;
using LearningTracker.Web.Authentication;
using LearningTracker.Web.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<RequirePasswordChangeFilter>();
});

builder.Services.AddAuthentication("LearningTrackerCookie")
    .AddCookie("LearningTrackerCookie", options =>
    {
        options.Cookie.Name = "LearningTracker.Auth";
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;

        options.EventsType = typeof(SecurityStampValidationEvents);
    });

builder.Services.AddAuthorization();

//DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

//Authorisation Role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CoordinatorOnly", policy =>
        policy.RequireRole("Coordinator"));

    options.AddPolicy("CandidateOnly", policy =>
        policy.RequireRole("Candidate"));
});

//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<ILearnerService, LearnerService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<SecurityStampValidationEvents>();
builder.Services.AddScoped<RequirePasswordChangeFilter>();

//Repository Injection
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILearnerRepository, LearnerRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();

var app = builder.Build();

// Automatically apply pending EF Core migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
