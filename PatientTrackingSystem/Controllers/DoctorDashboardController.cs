using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PatientTrackingSystem.Controllers;

public class DoctorDashboardController : Controller
{
    private DoctorManager _doctorManager = new DoctorManager(new EfDoctorRepository());
    private PatientManager _patientManager = new PatientManager(new EfPatientRepository());
    private AppointmentManager _appointmentManager = new AppointmentManager(new EfAppointmentRepository());
    private MedicalReportManager _medicalReportManager = new MedicalReportManager(new EfMedicalReportRepository());
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
        var doctorId = c.Doctors.Where(x => x.MailAddress == userMail).Select(y => y.DoctorID).FirstOrDefault();
        Doctor doctor = _doctorManager.GetById(doctorId);
        return View(doctor);
    }
    
    [HttpPost]
    public IActionResult Index(Doctor d)
    {
        try
        {
            d.DoctorStatus = true;
            _doctorManager.Update(d);
            return Json(new { success = true, message = "Bilgiler başarıyla güncellendi." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu: " + ex.Message });
        }
    }

    public IActionResult GetPatients()
    {
        var userMail = User.Identity.Name;
        Context c = new Context();
        var doctorId = c.Doctors.Where(x => x.MailAddress == userMail).Select(y => y.DoctorID).FirstOrDefault();
        var values = _patientManager.GetPatientListByDoctor(doctorId);
        return View(values);
    }

    [HttpGet]
    public IActionResult AddPatient()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddPatient(Patient patient)
    {
        try
        {
            patient.PatientStatus = true;
            var userMail = User.Identity.Name;
            Context c = new Context();
            var doctorId = c.Doctors.Where(x => x.MailAddress == userMail).Select(y => y.DoctorID).FirstOrDefault();
            patient.DoctorID = doctorId;
            _patientManager.Add(patient);
            return Json(new { success = true, message = "Hasta başarıyla eklendi." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Hasta eklenirken bir hata oluştu: " + ex.Message });
        }
    }
    
    public IActionResult GetAppointments()
    {
        var userMail = User.Identity.Name;
        Context c = new Context();
        var doctorId = c.Doctors.Where(x => x.MailAddress == userMail).Select(y => y.DoctorID).FirstOrDefault();
        var values = _appointmentManager.GetListWithPatient().Where(x => x.DoctorID == doctorId).ToList();
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
        var patient = _patientManager.GetById(appointment.PatientID);
        ViewBag.PatientName = patient.Name;
        ViewBag.PatientSurname = patient.Surname;
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
            var doctorId = c.Doctors.Where(x => x.MailAddress == userMail).Select(y => y.DoctorID).FirstOrDefault();
            a.AppointmentStatus = true;
            a.DoctorID = doctorId;
            _appointmentManager.Update(a);
            return Json(new { success = true, message = "Randevu başarıyla düzenlendi." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Randevu düzenlenirken bir hata oluştu: " + ex.Message });
        }
    }

    public IActionResult GetMedicalReports()
    {
        var userMail = User.Identity.Name;
        Context c = new Context();
        var doctorId = c.Doctors.Where(x => x.MailAddress == userMail).Select(y => y.DoctorID).FirstOrDefault();
        var values = _medicalReportManager.GetListWithPatient().Where(x => x.DoctorID == doctorId).ToList();
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
    
    [HttpPost]
    public IActionResult PreviewReport(int reportId)
    {
        var report = _medicalReportManager.GetById(reportId);
        if (report != null)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", report.ReportPath.TrimStart('/'));
            var ocrResult = OcrService.ExtractTextFromPdf(filePath);
            return Json(new { success = true, text = ocrResult });
        }
        return Json(new { success = false, message = "Rapor bulunamadı." });
    }
    
    [HttpGet]
    public IActionResult AddMedicalReport()
    {
        var userMail = User.Identity.Name;
        Context c = new Context();
        var doctorId = c.Doctors.Where(x => x.MailAddress == userMail).Select(y => y.DoctorID).FirstOrDefault();
        var patients = _patientManager.GetList().Where(p => p.DoctorID == doctorId && p.PatientStatus == true)
            .Select(p => new SelectListItem
            {
                Value = p.PatientID.ToString(),
                Text = p.Name + " " + p.Surname
            }).ToList();
        ViewBag.Patients = patients;
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddMedicalReport(MedicalReport report, IFormFile ReportPath, int PatientID)
    {
        try
        {
            var userMail = User.Identity.Name;
            Context c = new Context();
            var doctorId = c.Doctors.Where(x => x.MailAddress == userMail).Select(y => y.DoctorID).FirstOrDefault();

            if (ReportPath != null && ReportPath.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/reports");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ReportPath.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ReportPath.CopyToAsync(stream);
                }

                report.ReportPath = "/reports/" + uniqueFileName;
            }

            report.DoctorID = doctorId;
            report.PatientID = PatientID;
            report.CreateDate = DateTime.Now;
            report.ReportStatus = true;
            _medicalReportManager.Add(report);
            return Json(new { success = true, message = "Rapor başarıyla oluşturuldu." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Rapor oluşturulurken bir hata oluştu: " + ex.Message });
        }
    }
}