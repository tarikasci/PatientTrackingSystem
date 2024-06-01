using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public bool AppointmentStatus { get; set; }

        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public int PatientID { get; set; }
        public Patient Patient { get; set; }
    }
}