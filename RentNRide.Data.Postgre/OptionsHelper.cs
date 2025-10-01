using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RentNRide.Data.Postgre;

public static class OptionsHelper
{
    public static DbContextOptionsBuilder UsePostgresFromEnv(this DbContextOptionsBuilder options, IConfiguration configuration)
    {
        var host = configuration["POSTGRES_HOST"];
        var port = configuration["POSTGRES_PORT"];
        var db = configuration["POSTGRES_DB"];
        var user = configuration["POSTGRES_USER"];
        var pass = configuration["POSTGRES_PASSWORD"];

        var connString = $"Host={host};Port={port};Database={db};Username={user};Password={pass}";
        return options.UseNpgsql(connString);
    }
}
