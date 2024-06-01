using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class Context : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.ConnectionString);
    }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<MedicalReport> MedicalReports { get; set; }
    public DbSet<Patient> Patients { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MedicalReport>()
            .HasOne(m => m.Patient)
            .WithMany(p => p.MedicalReports)
            .HasForeignKey(m => m.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalReport>()
            .HasOne(m => m.Doctor)
            .WithMany(d => d.MedicalReports)
            .HasForeignKey(m => m.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Doctor)
            .WithMany(d => d.Patients)
            .HasForeignKey(p => p.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}