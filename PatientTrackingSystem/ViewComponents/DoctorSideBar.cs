using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace PatientTrackingSystem.ViewComponents;

public class DoctorSideBar : ViewComponent
{
    private DoctorManager _doctorManager = new DoctorManager(new EfDoctorRepository());
    public IViewComponentResult Invoke()
    {
        var userMail = User.Identity.Name;
        Context c = new Context();
        var doctorId = c.Doctors.Where(x => x.MailAddress == userMail).Select(y => y.DoctorID).FirstOrDefault();
        Doctor doctor = _doctorManager.GetById(doctorId);
        return View(doctor);
    }
}