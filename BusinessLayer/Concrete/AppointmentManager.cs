using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete;

public class AppointmentManager : IAppointmentService
{
    private IAppointmentDal _appointmentDal;

    public AppointmentManager(IAppointmentDal appointmentDal)
    {
        _appointmentDal = appointmentDal;
    }

    public void Add(Appointment t)
    {
        _appointmentDal.Insert(t);
    }

    public void Delete(Appointment t)
    {
        _appointmentDal.Delete(t);
    }

    public void Update(Appointment t)
    {
        _appointmentDal.Update(t);
    }

    public List<Appointment> GetList()
    {
        return _appointmentDal.GetListAll();
    }

    public Appointment GetById(int id)
    {
        return _appointmentDal.GetById(id);
    }

    public List<Appointment> GetListWithDoctor()
    {
        return _appointmentDal.GetListWithDoctor();
    }

    public List<Appointment> GetListWithPatient()
    {
        return _appointmentDal.GetListWithPatient();
    }
}