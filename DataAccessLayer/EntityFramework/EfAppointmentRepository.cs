using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFramework;

public class EfAppointmentRepository : GenericRepository<Appointment>, IAppointmentDal
{
    public List<Appointment> GetListWithDoctor()
    {
        using (var c = new Context())
        {
            return c.Appointments.Include(x=>x.Doctor).ToList();
        }
    }

    public List<Appointment> GetListWithPatient()
    {
        using (var c = new Context())
        {
            return c.Appointments.Include(x=>x.Patient).ToList();
        }
    }
}