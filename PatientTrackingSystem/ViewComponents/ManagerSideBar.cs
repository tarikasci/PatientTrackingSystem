using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace PatientTrackingSystem.ViewComponents;

public class ManagerSideBar : ViewComponent
{
    private ManagerManager _managerManager = new ManagerManager(new EfManagerRepository());
    public IViewComponentResult Invoke()
    {
        var userMail = User.Identity.Name;
        Context c = new Context();
        var managerId = c.Managers.Where(x => x.MailAddress == userMail).Select(y => y.ManagerID).FirstOrDefault();
        Manager manager = _managerManager.GetById(managerId);
        return View(manager);
    }
}