using System.Diagnostics.Metrics;

namespace PetCore.Observability;

public static class ApiMetrics
{
    public const string MeterName = "PetCore.Metrics";
    public static readonly Meter Meter = new(MeterName);
    public static readonly Histogram<double> RequestDuration = Meter.CreateHistogram<double>("petcore.http.server.duration", "ms");
    public static readonly Counter<long> RequestErrors = Meter.CreateCounter<long>("petcore.http.server.errors");
}
