using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace PatientTrackingSystem.ViewComponents;

public class LoginHeader : ViewComponent
{
    private PatientManager _patientManager = new PatientManager(new EfPatientRepository());
    public IViewComponentResult Invoke()
    {
        var userMail = User.Identity.Name;
        Context c = new Context();
        var patientId = c.Patients.Where(x => x.MailAddress == userMail).Select(y => y.PatientID).FirstOrDefault();
        Patient patient = _patientManager.GetById(patientId);
        return View(patient);
    }
}