using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PatientTrackingSystem.Controllers;

public class RegisterController : Controller
{
    private PatientManager pm = new PatientManager(new EfPatientRepository());
    // GET
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Gender = new List<SelectListItem>
        {
            new SelectListItem { Text = "Erkek", Value = Gender.Erkek.ToString() },
            new SelectListItem { Text = "Kadın", Value = Gender.Kadın.ToString() }
        };

        return View();
    }
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Index(Patient patient)
    {
        try
        {
            patient.PatientStatus = true;
            patient.DoctorID = 1;
            pm.Add(patient);
            return Json(new { success = true, message = "Kayıt başarıyla tamamlandı." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Kayıt sırasında bir hata oluştu: " + ex.Message });
        }
    }
}