using Microsoft.AspNetCore.Identity;

namespace HealthcareBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}