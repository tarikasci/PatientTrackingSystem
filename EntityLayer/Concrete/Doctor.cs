using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public string Specialty { get; set; }
        public string Hospital { get; set; }
        public string DoctorImage { get; set; }
        public bool DoctorStatus { get; set; }
        
        public string MailAddress { get; set; }
        public string Password { get; set; }

        public List<Patient> Patients { get; set; }
        public List<MedicalReport> MedicalReports { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}