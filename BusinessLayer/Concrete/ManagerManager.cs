using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete;

public class ManagerManager : IManagerService
{
    private IManagerDal _managerDal;

    public ManagerManager(IManagerDal managerDal)
    {
        _managerDal = managerDal;
    }

    public void Add(Manager t)
    {
        _managerDal.Insert(t);
    }

    public void Delete(Manager t)
    {
        _managerDal.Delete(t);
    }

    public void Update(Manager t)
    {
        _managerDal.Update(t);
    }

    public List<Manager> GetList()
    {
        return _managerDal.GetListAll();
    }

    public Manager GetById(int id)
    {
        return _managerDal.GetById(id);
    }
}