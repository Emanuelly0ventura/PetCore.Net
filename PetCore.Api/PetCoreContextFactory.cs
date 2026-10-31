using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PetCore.Infrastructure.Persistence;
using PetCore.Configuration;

/// <summary>Creates a PostgreSQL context for EF Core tooling.</summary>
public sealed class PetCoreContextFactory : IDesignTimeDbContextFactory<PetCoreContext>
{
    public PetCoreContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("A conexão PostgreSQL não foi configurada. Defina ConnectionStrings__DefaultConnection.");
        var options = new DbContextOptionsBuilder<PetCoreContext>().UseNpgsql(PostgreSqlConnectionString.Normalize(connectionString)).Options;
        return new PetCoreContext(options);
    }
}
