using System;
using System.Linq;
using System.Reflection;
using DbUp;
using Infrastrucure.Database;

namespace InterviewConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseInitialization();
        }
        
        private static void DatabaseInitialization()
        {
            var settings = DatabaseSettings.Load();

            EnsureDatabase.For.SqlDatabase(settings.ConnectionString);

            var upgrader = DeployChanges.To
                .SqlDatabase(settings.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .LogToConsole()
                .Build();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            foreach (var name in resourceNames)
            {
                Console.WriteLine($"Found resource: {name}");
            }

            var scripts = upgrader.GetScriptsToExecute();
            if (scripts.Any())
            {
                Console.WriteLine($"Applying migrations:");
                foreach (var s in scripts)
                {
                    Console.WriteLine($"  → {s.Name}");
                }

                var result = upgrader.PerformUpgrade();

                if (result.Successful)
                {
                    Console.WriteLine("Migration completed successfully.");
                }
                else
                {
                    Console.WriteLine("Migration FAILED: " + result.Error);
                }
            }
            else
            {
                Console.WriteLine("No pending migrations. Database is up to date.");
            }
        }
    }
}
