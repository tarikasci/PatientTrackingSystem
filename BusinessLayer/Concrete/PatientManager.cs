using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete;

public class PatientManager : IPatientService
{
    private IPatientDal _patientDal;

    public PatientManager(IPatientDal patientDal)
    {
        _patientDal = patientDal;
    }

    public void Add(Patient t)
    {
        _patientDal.Insert(t);
    }

    public void Delete(Patient t)
    {
        _patientDal.Delete(t);
    }

    public void Update(Patient t)
    {
        _patientDal.Update(t);
    }

    public List<Patient> GetList()
    {
        return _patientDal.GetListAll();
    }

    public Patient GetById(int id)
    {
        return _patientDal.GetById(id);
    }

    public List<Patient> GetPatientListByDoctor(int id)
    {
        return _patientDal.GetListAll(x=>x.DoctorID == id);
    }
}