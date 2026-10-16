using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PetCore.Observability;

public sealed class ExternalServiceHealthCheck(IConfiguration configuration, IHttpClientFactory httpClientFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var serviceUrl = configuration["ExternalServices:BaseUrl"];

        if (string.IsNullOrWhiteSpace(serviceUrl))
            return HealthCheckResult.Healthy("Nenhum serviço externo configurado.");

        try
        {
            using var response = await httpClientFactory.CreateClient("external-health")
                .GetAsync(serviceUrl, cancellationToken);

            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("Serviço externo disponível.")
                : HealthCheckResult.Unhealthy("Serviço externo retornou resposta inválida.");
        }
        catch (HttpRequestException exception)
        {
            return HealthCheckResult.Unhealthy("Serviço externo indisponível.", exception);
        }
    }
}
