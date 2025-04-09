using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HealthcareBookingSystem.Data;
using HealthcareBookingSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

// Database migration and seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.Doctors.Any())
    {
        dbContext.Doctors.AddRange(
            new Doctor { Name = "Dr. Smith", Specialty = "Cardiology", AvailableStartTime = TimeSpan.FromHours(9), AvailableEndTime = TimeSpan.FromHours(17) },
            new Doctor { Name = "Dr. Jones", Specialty = "Pediatrics", AvailableStartTime = TimeSpan.FromHours(9), AvailableEndTime = TimeSpan.FromHours(17) }
        );
        dbContext.SaveChanges();
    }
    else
    {
        // Update existing doctors (optional - included from the first snippet)
        foreach (var doctor in dbContext.Doctors)
        {
            if (doctor.AvailableStartTime == TimeSpan.Zero && doctor.AvailableEndTime == TimeSpan.Zero)
            {
                doctor.AvailableStartTime = TimeSpan.FromHours(9);
                doctor.AvailableEndTime = TimeSpan.FromHours(17);
            }
        }
        dbContext.SaveChanges();
    }
}

app.Run();