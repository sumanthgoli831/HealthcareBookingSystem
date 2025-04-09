using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // Add this directive
using HealthcareBookingSystem.Data;
using HealthcareBookingSystem.Models;

namespace HealthcareBookingSystem.Pages.Appointments
{
    public class CancelModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CancelModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Appointment? Appointment { get; set; }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Console.WriteLine($"Cancel POST requested for appointment ID: {id}");
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("User not authenticated, redirecting to login...");
                return RedirectToPage("/Identity/Account/Login", new { returnUrl = "/Appointments/MyBookings" });
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
            {
                Console.WriteLine($"Appointment ID {id} not found");
                TempData["Error"] = "Appointment not found.";
                return RedirectToPage("/Appointments/MyBookings");
            }
            if (appointment.PatientId != user.Id)
            {
                Console.WriteLine($"User {user.UserName} not authorized to cancel appointment ID {id}");
                TempData["Error"] = "You are not authorized to cancel this appointment.";
                return RedirectToPage("/Appointments/MyBookings");
            }
            if (!appointment.IsBooked)
            {
                Console.WriteLine($"Appointment ID {id} already cancelled");
                TempData["Message"] = "This appointment is already cancelled.";
                return RedirectToPage("/Appointments/MyBookings");
            }

            Console.WriteLine($"Cancelling appointment ID {id} for user {user.UserName}");
            appointment.IsBooked = false;
            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("Appointment cancelled successfully in database");
                Appointment = appointment; // Pass to confirmation page
                return Page(); // Show confirmation instead of redirect
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving cancellation: {ex.Message}");
                TempData["Error"] = "Failed to cancel appointment due to a server error.";
                return RedirectToPage("/Appointments/MyBookings");
            }
        }
    }
}
