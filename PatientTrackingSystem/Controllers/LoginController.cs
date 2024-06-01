using System.Security.Claims;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PatientTrackingSystem.Controllers;

public class LoginController : Controller
{
    // GET
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Index(Patient p)
    {
        Context c = new Context();
        var dataValue = c.Patients.FirstOrDefault(x => x.MailAddress == p.MailAddress && x.Password == p.Password);
        if (dataValue != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, p.MailAddress)
            };
            var userIdentity = new ClaimsIdentity(claims, "a");
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);
            return RedirectToAction("Index", "Doctor");
        }
        else
        {
            return View();
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult DoctorLogin()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> DoctorLogin(Doctor d)
    {
        Context c = new Context();
        var dataValue2 = c.Doctors.FirstOrDefault(x => x.MailAddress == d.MailAddress && x.Password == d.Password);
        if (dataValue2 != null)
        {
            var claims2 = new List<Claim>
            {
                new Claim(ClaimTypes.Name, d.MailAddress)
            };
            var userIdentity2 = new ClaimsIdentity(claims2, "a");
            ClaimsPrincipal principal2 = new ClaimsPrincipal(userIdentity2);
            await HttpContext.SignInAsync(principal2);
            return RedirectToAction("Index", "DoctorDashboard");  
        }
        else
        {
            return View();
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ManagerLogin()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ManagerLogin(Manager m)
    {
        Context c = new Context();
        var dataValue3 = c.Managers.FirstOrDefault(x => x.MailAddress == m.MailAddress && x.Password == m.Password);
        if (dataValue3 != null)
        {
            var claims3 = new List<Claim>
            {
                new Claim(ClaimTypes.Name, m.MailAddress)
            };
            var userIdentity3 = new ClaimsIdentity(claims3, "a");
            ClaimsPrincipal principal3 = new ClaimsPrincipal(userIdentity3);
            await HttpContext.SignInAsync(principal3);
            return RedirectToAction("Index", "ManagerDashboard");  //Manager ın yeni temasındaki giriş ekranına yönlendir
        }
        else
        {
            return View();
        }
    }
    
}