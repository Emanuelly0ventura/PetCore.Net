using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using PetCore.Infrastructure.Persistence;
using PetCore.Observability;

namespace PetCore.Extensions;

public static class ObservabilityExtensions
{
    public static IServiceCollection AddPetCoreObservability(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("external-health", client => client.Timeout = TimeSpan.FromSeconds(5));

        services.AddHealthChecks()
            .AddDbContextCheck<PetCoreContext>("postgresql", tags: ["ready", "database"])
            .AddCheck<ExternalServiceHealthCheck>("external-service", tags: ["ready", "external"]);

        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddSource(ApiMetrics.MeterName))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddMeter(ApiMetrics.MeterName)
                .AddPrometheusExporter());

        return services;
    }

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app) =>
        app.UseMiddleware<CorrelationIdMiddleware>();

    public static IApplicationBuilder UseApiMetrics(this IApplicationBuilder app) =>
        app.UseMiddleware<ApiMetricsMiddleware>();
}
