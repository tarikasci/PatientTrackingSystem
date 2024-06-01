using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace PatientTrackingSystem.Controllers;

public class DoctorController : Controller
{
    // GET
    private DoctorManager dm = new DoctorManager(new EfDoctorRepository());
    public IActionResult Index()
    {
        var values = dm.GetList();
        return View(values);
    }
}