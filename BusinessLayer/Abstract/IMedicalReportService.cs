using EntityLayer.Concrete;

namespace BusinessLayer.Abstract;

public interface IMedicalReportService : IGenericService<MedicalReport>
{
    List<MedicalReport> GetListWithDoctor();
    List<MedicalReport> GetListWithPatient();
}