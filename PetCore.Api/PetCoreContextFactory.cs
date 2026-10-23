using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PetCore.Infrastructure.Persistence;

/// <summary>
/// Creates the PostgreSQL context for EF Core tooling without starting the API
/// or requiring its MongoDB configuration.
/// </summary>
public sealed class PetCoreContextFactory : IDesignTimeDbContextFactory<PetCoreContext>
{
    public PetCoreContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        // EF needs the Npgsql provider to build the model for migrations. A real
        // connection is only required by commands that access the database.
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = "Host=localhost;Port=5432;Database=petcore_design_time;Username=postgres;Password=postgres";
        }

        var options = new DbContextOptionsBuilder<PetCoreContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new PetCoreContext(options);
    }
}
