namespace HealthcareBookingSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string PatientId { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public bool IsBooked { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Sex { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;

        public Doctor Doctor { get; set; } = null!;
    }
}