using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PetCore.Infrastructure.Persistence;

/// <summary>Creates a SQL Server context for EF Core tooling.</summary>
public sealed class PetCoreContextFactory : IDesignTimeDbContextFactory<PetCoreContext>
{
    public PetCoreContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            connectionString = "Server=localhost;Database=PetCore_DesignTime;User Id=sa;Password=DesignTimeOnly!123;TrustServerCertificate=True";
        var options = new DbContextOptionsBuilder<PetCoreContext>().UseSqlServer(connectionString).Options;
        return new PetCoreContext(options);
    }
}
