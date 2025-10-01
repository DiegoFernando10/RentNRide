using DbUp;
using System.Reflection;

namespace RentNRide.Deployment.Database;

internal class Program
{
    static int Main(string[] args)
    {
        Console.WriteLine("Starting DB upgrade (Postgres) - RentNRide");

        Dictionary<string, string>? parameters = null;
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var dbname = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

        parameters = new Dictionary<string, string>
        {
            ["host"] = host,
            ["port"] = port,
            ["dbname"] = dbname,
            ["username"] = username,
            ["password"] = password
        };

        Console.WriteLine("Loaded DB connection from individual environment variables.");

        var hostParam = parameters["host"];
        var portParam = parameters["port"];
        var dbNameParam = parameters["dbname"];
        var userParam = parameters["username"];
        var passParam = parameters["password"];

        var connString = $"Host={hostParam};Port={portParam};Database={dbNameParam};Username={userParam};Password={passParam};";

        Console.WriteLine($"Using Postgres connection: Host={hostParam};Database={dbNameParam};Port={portParam};");
        int retries = 10;
        while (retries > 0)
        {
            try
            {
                var upgrader = DeployChanges.To
                    .PostgresqlDatabase(connString)
                    .WithScriptsEmbeddedInAssembly(
                        Assembly.GetExecutingAssembly(),
                        s => s.StartsWith("RentNRide.Deployment.Database.Scripts"))
                    .WithTransactionPerScript()
                    .LogToConsole()
                    .WithExecutionTimeout(TimeSpan.FromSeconds(180))
                    .Build();

                var result = upgrader.PerformUpgrade();
                if (!result.Successful)
                {
                    Console.WriteLine(result.Error);
                    return -1;
                }
                Console.WriteLine("Database upgrade completed successfully...");
                return 0;
            }
            catch
            {
                Console.WriteLine("DB not ready yet, retrying in 5s...");
                Thread.Sleep(5000);
                retries--;
            }
        }
        Console.WriteLine("Failed to connect to DB after multiple retries.");
        return -1;
    }
}
