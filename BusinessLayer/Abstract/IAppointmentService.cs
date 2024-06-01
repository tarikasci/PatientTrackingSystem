using EntityLayer.Concrete;

namespace BusinessLayer.Abstract;

public interface IAppointmentService : IGenericService<Appointment>
{
    List<Appointment> GetListWithDoctor();
    List<Appointment> GetListWithPatient();
}