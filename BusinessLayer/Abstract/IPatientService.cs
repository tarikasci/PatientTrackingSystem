using EntityLayer.Concrete;

namespace BusinessLayer.Abstract;

public interface IPatientService : IGenericService<Patient>
{
    List<Patient> GetPatientListByDoctor(int id);
}