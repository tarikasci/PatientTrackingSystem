using Microsoft.Extensions.Configuration;

namespace DataAccessLayer;

public class Configuration
{
    public static string ConnectionString
    {
        get
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),
                "../PatientTrackingSystem"));
            configurationManager.AddJsonFile("appsettings.json");
            return configurationManager.GetConnectionString("SQL");
        }
    }
}