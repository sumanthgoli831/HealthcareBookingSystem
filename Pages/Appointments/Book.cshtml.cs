using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthcareBookingSystem.Data;
using HealthcareBookingSystem.Models;
using System.Net.Mail;

namespace HealthcareBookingSystem.Pages.Appointments
{
    public class BookModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public DateTime AppointmentDate { get; set; }
        [BindProperty]
        public string? PatientName { get; set; }
        [BindProperty]
        public int Age { get; set; }
        [BindProperty]
        public string? Sex { get; set; }
        [BindProperty]
        public string? ContactNumber { get; set; }
        [BindProperty]
        public string? Address { get; set; }
        [BindProperty]
        public string? Reason { get; set; }
        public Doctor? Doctor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Doctor = await _context.Doctors.FindAsync(id);
            if (Doctor == null) return NotFound();
            Console.WriteLine($"Doctor {Doctor.Name} availability: {Doctor.AvailableStartTime} to {Doctor.AvailableEndTime}");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Doctor = await _context.Doctors.FindAsync(id);
            if (Doctor == null) return NotFound();

            if (AppointmentDate < DateTime.Now)
            {
                ModelState.AddModelError("AppointmentDate", "Cannot book in the past.");
                return Page();
            }

            var appointmentTime = AppointmentDate.TimeOfDay;
            Console.WriteLine($"Checking availability: {appointmentTime} vs {Doctor.AvailableStartTime} - {Doctor.AvailableEndTime}");
            if (appointmentTime < Doctor.AvailableStartTime || appointmentTime > Doctor.AvailableEndTime)
            {
                ModelState.AddModelError("AppointmentDate", $"Doctor is available only from {Doctor.AvailableStartTime:hh\\:mm} to {Doctor.AvailableEndTime:hh\\:mm}.");
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Redirect("/Identity/Account/Login");

            var appointment = new Appointment
            {
                DoctorId = id,
                PatientId = user.Id,
                AppointmentDate = AppointmentDate,
                IsBooked = true,
                PatientName = PatientName ?? string.Empty, // Null-coalescing operator
                Age = Age,
                Sex = Sex ?? string.Empty,             // Null-coalescing operator
                ContactNumber = ContactNumber ?? string.Empty, // Null-coalescing operator
                Address = Address ?? string.Empty,         // Null-coalescing operator
                Reason = Reason ?? string.Empty          // Null-coalescing operator
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            await SendConfirmationEmail(user.Email, Doctor.Name, AppointmentDate, PatientName);

            TempData["Message"] = "Appointment booked successfully!";
            return RedirectToPage("Index");
        }

        private async Task SendConfirmationEmail(string? email, string doctorName, DateTime date, string? patientName)
        {
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email address is null or empty. Cannot send confirmation email.");
                return;
            }

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("your-email@gmail.com", "your-app-password"),
                    EnableSsl = true,
                };
                var message = new MailMessage
                {
                    From = new MailAddress("your-email@gmail.com"),
                    Subject = "Appointment Confirmation",
                    Body = $"Dear {patientName},\n\nYour appointment with {doctorName} on {date.ToString("g")} has been confirmed.\n\nThank you,\nHealthcare Booking System",
                    IsBodyHtml = false,
                };
                message.To.Add(email);
                await smtpClient.SendMailAsync(message);
                Console.WriteLine("Email sent successfully to " + email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    }
}