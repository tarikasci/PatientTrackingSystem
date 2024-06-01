using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class MedicalReport
    {
        [Key]
        public int ReportID { get; set; }
        public DateTime CreateDate { get; set; }
        public string Info { get; set; }
        public string ReportPath { get; set; }
        public bool ReportStatus { get; set; }

        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public int PatientID { get; set; }
        public Patient Patient { get; set; }
    }
}