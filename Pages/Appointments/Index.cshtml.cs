using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthcareBookingSystem.Data;
using HealthcareBookingSystem.Models;

namespace HealthcareBookingSystem.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Doctor> Doctors { get; set; } = new List<Doctor>();

        public async Task OnGetAsync()
        {
            Doctors = await _context.Doctors.ToListAsync();
        }
    }
}