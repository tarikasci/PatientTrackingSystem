using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract;

public interface IAppointmentDal : IGenericDal<Appointment>
{
    List<Appointment> GetListWithDoctor();
    List<Appointment> GetListWithPatient();
}