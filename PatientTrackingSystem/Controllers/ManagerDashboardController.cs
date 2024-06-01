using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PatientTrackingSystem.Controllers;

public class ManagerDashboardController : Controller
{
    private ManagerManager _managerManager = new ManagerManager(new EfManagerRepository());
    private PatientManager _patientManager = new PatientManager(new EfPatientRepository());
    private DoctorManager _doctorManager = new DoctorManager(new EfDoctorRepository());
    private AppointmentManager _appointmentManager = new AppointmentManager(new EfAppointmentRepository());
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
        var managerId = c.Managers.Where(x => x.MailAddress == userMail).Select(y => y.ManagerID).FirstOrDefault();
        Manager manager = _managerManager.GetById(managerId);
        return View(manager);
    }
    
    [HttpPost]
    public IActionResult Index(Manager m)
    {
        m.ManagerStatus = true;
        _managerManager.Update(m);
        return RedirectToAction("Index", "ManagerDashboard");
    }

    public IActionResult GetPatients()
    {
        var values = _patientManager.GetList();
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
            patient.DoctorID = 1;
            _patientManager.Add(patient);
            return Json(new { success = true, message = "Hasta başarıyla eklendi." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Hasta eklenirken bir hata oluştu: " + ex.Message });
        }
    }
    
    public IActionResult PatientDelete(int id)
    {
        var patient = _patientManager.GetById(id);
        patient.PatientStatus = false;
        _patientManager.Update(patient);
        return RedirectToAction("GetPatients");
    }
    
    [HttpGet]
    public IActionResult PatientUpdate(int id)
    {
        var patient = _patientManager.GetById(id);
        return View(patient);
    }
    
    [HttpPost]
    public IActionResult PatientUpdate(Patient p)
    {
        p.PatientStatus = true;
        p.DoctorID = 1;
        _patientManager.Update(p);
        return RedirectToAction("GetPatients", "ManagerDashboard");
    }

    public IActionResult GetDoctors()
    {
        var values = _doctorManager.GetList();
        return View(values);
    }

    public IActionResult DoctorDelete(int id)
    {
        var doctor = _doctorManager.GetById(id);
        
        var hasActiveAppointments = _appointmentManager.GetList()
            .Any(a => a.DoctorID == id && a.AppointmentStatus == true);

        if (hasActiveAppointments)
        {
            return Json(new { success = false, message = "Doktorun aktif randevuları olduğu için silinemiyor." });
        }

        doctor.DoctorStatus = false;
        _doctorManager.Update(doctor);
        return Json(new { success = true, message = "Doktor başarıyla silindi." });
    }
    
    [HttpGet]
    public IActionResult DoctorUpdate(int id)
    {
        var doctor = _doctorManager.GetById(id);
        return View(doctor);
    }
    
    [HttpPost]
    public IActionResult DoctorUpdate(Doctor d)
    {
        d.DoctorStatus = true;
        _doctorManager.Update(d);
        return RedirectToAction("GetDoctors", "ManagerDashboard");
    }
    
    [HttpGet]
    public IActionResult AddDoctor()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddDoctor(Doctor doctor)
    {
        try
        {
            doctor.DoctorStatus = true;
            _doctorManager.Add(doctor);
            return Json(new { success = true, message = "Doktor başarıyla eklendi." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Doktor eklenirken bir hata oluştu: " + ex.Message });
        }
    }
}