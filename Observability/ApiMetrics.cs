using System.Diagnostics.Metrics;

namespace ChallengePetApi.Observability;

public static class ApiMetrics
{
    public const string MeterName = "SuperNovaVet.Api";

    private static readonly Meter Meter = new(MeterName);

    public static readonly Counter<long> Requests =
        Meter.CreateCounter<long>("api_requests_total");

    public static readonly Counter<long> Errors =
        Meter.CreateCounter<long>("api_errors_total");

    public static readonly Histogram<double> RequestDuration =
        Meter.CreateHistogram<double>(
            "api_request_duration_ms",
            unit: "ms"
        );
}