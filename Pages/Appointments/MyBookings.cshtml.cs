using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthcareBookingSystem.Data;
using HealthcareBookingSystem.Models;

namespace HealthcareBookingSystem.Pages.Appointments
{
    public class MyBookingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyBookingsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Appointment> Appointments { get; set; } = new List<Appointment>();

        public async Task OnGetAsync()
        {
            Console.WriteLine("OnGetAsync started for MyBookings...");
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("No user authenticated, setting empty appointments list");
                Appointments = new List<Appointment>();
                return;
            }

            Console.WriteLine($"Fetching appointments for user: {user.UserName} (ID: {user.Id})");
            Appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == user.Id)
                .ToListAsync();

            Console.WriteLine($"Loaded {Appointments.Count} appointments for user {user.UserName}");
            foreach (var appt in Appointments)
            {
                Console.WriteLine($"Appointment ID: {appt.Id}, Doctor: {appt.Doctor?.Name}, Date: {appt.AppointmentDate}, IsBooked: {appt.IsBooked}");
            }
        }
    }
}