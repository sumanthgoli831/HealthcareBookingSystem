namespace HealthcareBookingSystem.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public TimeSpan AvailableStartTime { get; set; } = TimeSpan.FromHours(9); // 9 AM
        public TimeSpan AvailableEndTime { get; set; } = TimeSpan.FromHours(17); // 5 PM
    }
}