using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete;

public class MedicalReportManager : IMedicalReportService
{
    private IMedicalReportDal _medicalReportDal;

    public MedicalReportManager(IMedicalReportDal medicalReportDal)
    {
        _medicalReportDal = medicalReportDal;
    }

    public void Add(MedicalReport t)
    {
        _medicalReportDal.Insert(t);
    }

    public void Delete(MedicalReport t)
    {
        _medicalReportDal.Delete(t);
    }

    public void Update(MedicalReport t)
    {
        _medicalReportDal.Update(t);
    }

    public List<MedicalReport> GetList()
    {
        return _medicalReportDal.GetListAll();
    }

    public MedicalReport GetById(int id)
    {
        return _medicalReportDal.GetById(id);
    }

    public List<MedicalReport> GetListWithDoctor()
    {
        return _medicalReportDal.GetListWithDoctor();
    }

    public List<MedicalReport> GetListWithPatient()
    {
        return _medicalReportDal.GetListWithPatient();
    }
}