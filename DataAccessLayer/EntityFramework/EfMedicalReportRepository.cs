using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFramework;

public class EfMedicalReportRepository : GenericRepository<MedicalReport>, IMedicalReportDal
{
    public List<MedicalReport> GetListWithDoctor()
    {
        using (var c = new Context())
        {
            return c.MedicalReports.Include(x=>x.Doctor).ToList();
        }
    }

    public List<MedicalReport> GetListWithPatient()
    {
        using (var c = new Context())
        {
            return c.MedicalReports.Include(x=>x.Patient).ToList();
        }
    }
}