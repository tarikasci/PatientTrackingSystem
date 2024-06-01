using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool PatientStatus { get; set; }

        public string MailAddress { get; set; }
        public string Password { get; set; }

        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public List<MedicalReport> MedicalReports { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}