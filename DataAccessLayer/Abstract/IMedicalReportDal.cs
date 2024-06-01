using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract;

public interface IMedicalReportDal : IGenericDal<MedicalReport>
{
    List<MedicalReport> GetListWithDoctor();
    List<MedicalReport> GetListWithPatient();
}