using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete;

public class DoctorManager : IDoctorService
{
    private IDoctorDal _doctorDal;

    public DoctorManager(IDoctorDal doctorDal)
    {
        _doctorDal = doctorDal;
    }

    public void Add(Doctor t)
    {
        _doctorDal.Insert(t);
    }

    public void Delete(Doctor t)
    {
        _doctorDal.Delete(t);
    }

    public void Update(Doctor t)
    {
        _doctorDal.Update(t);
    }

    public List<Doctor> GetList()
    {
        return _doctorDal.GetListAll();
    }

    public Doctor GetById(int id)
    {
        return _doctorDal.GetById(id);
    }
}