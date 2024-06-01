using System.Runtime.InteropServices.JavaScript;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PatientTrackingSystem.Controllers;

public class PatientInfoController : Controller
{
    private PatientManager _patientManager = new PatientManager(new EfPatientRepository());
    private MedicalReportManager _medicalReportManager = new MedicalReportManager(new EfMedicalReportRepository());
    private AppointmentManager _appointmentManager = new AppointmentManager(new EfAppointmentRepository());
    private DoctorManager _doctorManager = new DoctorManager(new EfDoctorRepository());
    // GET
    public IActionResult Index()
    {
        ViewBag.Gender = new List<SelectListItem>
        {
            new SelectListItem { Text = "Erkek", Value = Gender.Erkek.ToString() },
            new SelectListItem { Text = "Kadın", Value = Gender.Kadın.ToString() }
        };
        var userMail = User.Identity.Name;
        Context c = new Context();
        var patientId = c.Patients.Where(x => x.MailAddress == userMail).Select(y => y.PatientID).FirstOrDefault();
        Patient patient = _patientManager.GetById(patientId);
        return View(patient);
    }
    
    [HttpPost]
    public IActionResult Index(Patient p)
    {
        p.PatientStatus = true;
        p.DoctorID = 1;
        _patientManager.Update(p);
        return RedirectToAction("Index", "Doctor");
    }

    [HttpGet]
    public IActionResult GetMedicalReports()
    {
        var userMail = User.Identity.Name;
        Context c = new Context();
        var patientId = c.Patients.Where(x => x.MailAddress == userMail).Select(y => y.PatientID).FirstOrDefault();
        var values = _medicalReportManager.GetListWithDoctor().Where(x => x.PatientID == patientId).ToList();
        return View(values);
    }
    
    public IActionResult DownloadReport(int id)
    {
        var report = _medicalReportManager.GetById(id);
        if (report == null)
        {
            return NotFound();
        }

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", report.ReportPath.TrimStart('/'));
        var memory = new MemoryStream();

        using (var stream = new FileStream(path, FileMode.Open))
        {
            stream.CopyTo(memory);
        }

        memory.Position = 0;
        return File(memory, "application/pdf", Path.GetFileName(path));
    }

    [HttpGet]
    public IActionResult GetAppointments()
    {
        var userMail = User.Identity.Name;
        Context c = new Context();
        var patientId = c.Patients.Where(x => x.MailAddress == userMail).Select(y => y.PatientID).FirstOrDefault();
        var values = _appointmentManager.GetListWithDoctor().Where(x => x.PatientID == patientId).ToList();
        return View(values);
    }
    
    public IActionResult AppointmentDelete(int id)
    {
        var appointmentValue = _appointmentManager.GetById(id);
        appointmentValue.AppointmentStatus = false;
        _appointmentManager.Update(appointmentValue);
        return RedirectToAction("GetAppointments");
    }

    [HttpGet]
    public IActionResult AppointmentUpdate(int id)
    {
        var appointment = _appointmentManager.GetById(id);
        var doctor = _doctorManager.GetById(appointment.DoctorID);
        ViewBag.DoctorName = doctor.Name;
        ViewBag.DoctorSurname = doctor.Surname;
        return View(appointment);
    }
    
    [HttpPost]
    public IActionResult AppointmentUpdate(Appointment a)
    {
        if (a.AppointmentDate.CompareTo(DateOnly.FromDateTime(DateTime.Today)) < 0)
        {
            return Json(new { success = false, message = "Geçmiş bir tarihe randevu alamazsınız." });
        }
        try
        {
            var userMail = User.Identity.Name;
            Context c = new Context();
            var patientId = c.Patients.Where(x => x.MailAddress == userMail).Select(y => y.PatientID).FirstOrDefault();
            a.AppointmentStatus = true;
            a.PatientID = patientId;
            _appointmentManager.Update(a);
            return Json(new { success = true, message = "Randevu başarıyla düzenlendi." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Randevu düzenlenirken bir hata oluştu: " + ex.Message });
        }
    }

    [HttpGet]
    public IActionResult MakeAnAppointment(int id)
    {
        Doctor doctor = _doctorManager.GetById(id);
        var appointment = new Appointment()
        {
            DoctorID = id
        };
        ViewBag.DoctorName = doctor.Name;
        ViewBag.DoctorSurname = doctor.Surname;
        return View(appointment);
    }
    
    [HttpPost]
    public IActionResult MakeAnAppointment(Appointment a)
    {
        if (a.AppointmentDate.CompareTo(DateOnly.FromDateTime(DateTime.Today)) < 0)
        {
            return Json(new { success = false, message = "Geçmiş bir tarihe randevu alamazsınız." });
        }
        try
        {
            var userMail = User.Identity.Name;
            Context c = new Context();
            var patientId = c.Patients.Where(x => x.MailAddress == userMail).Select(y => y.PatientID).FirstOrDefault();
            a.AppointmentStatus = true;
            a.PatientID = patientId;
            _appointmentManager.Add(a);
            return Json(new { success = true, message = "Randevu başarıyla oluşturuldu." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Randevu oluşturulurken bir hata oluştu: " + ex.Message });
        }
    }
}