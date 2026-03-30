using System.Configuration;
using Domain.Constants;

namespace Infrastrucure.Database
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; private set; }

        private DatabaseSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public static DatabaseSettings Load()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[ConfiguratorConstants.EmployeeDb]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ConfigurationErrorsException($"Connection string {ConfiguratorConstants.EmployeeDb} is missing from App.config.");

            return new DatabaseSettings(connectionString);
        }
    }
}
