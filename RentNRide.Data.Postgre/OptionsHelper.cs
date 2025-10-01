using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RentNRide.Data.PostgreSql;

public static class OptionsHelper
{
    public static DbContextOptionsBuilder UsePostgresFromEnv(this DbContextOptionsBuilder options, IConfiguration configuration)
    {
        var host = configuration["POSTGRESQL_HOST"];
        var port = configuration["POSTGRESQL_PORT"];
        var db = configuration["POSTGRESQL_DB"];
        var user = configuration["POSTGRESQL_USER"];
        var pass = configuration["POSTGRESQL_PASSWORD"];

        var connString = $"Host={host};Port={port};Database={db};Username={user};Password={pass}";
        return options.UseNpgsql(connString);
    }
}
