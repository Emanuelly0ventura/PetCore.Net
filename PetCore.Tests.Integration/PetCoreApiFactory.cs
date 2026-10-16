using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PetCore.Infrastructure.Persistence;

namespace PetCore.Tests.Integration;

public sealed class PetCoreApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(descriptor =>
                descriptor.ServiceType == typeof(DbContextOptions<PetCoreContext>) ||
                descriptor.ServiceType == typeof(PetCoreContext) ||
                descriptor.ServiceType == typeof(IDbContextOptionsConfiguration<PetCoreContext>)).ToList();

            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            services.AddDbContext<PetCoreContext>(options =>
                options.UseInMemoryDatabase("PetCoreIntegrationTests"));

            services.PostConfigure<HealthCheckServiceOptions>(options =>
            {
                var databaseCheck = options.Registrations
                    .FirstOrDefault(registration => registration.Name == "mysql");

                if (databaseCheck is not null)
                {
                    options.Registrations.Remove(databaseCheck);
                }
            });

            services.AddHealthChecks()
                .AddCheck("test-database", () => HealthCheckResult.Healthy(), tags: ["ready", "database"]);
        });
    }
}
