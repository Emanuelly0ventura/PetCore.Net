using System.Diagnostics;

namespace PetCore.Observability;

public sealed class ApiMetricsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            if (context.Response.StatusCode >= StatusCodes.Status500InternalServerError)
            {
                ApiMetrics.RequestErrors.Add(1,
                    new KeyValuePair<string, object?>("http.request.method", context.Request.Method),
                    new KeyValuePair<string, object?>("url.path", context.Request.Path));
            }

            ApiMetrics.RequestDuration.Record(stopwatch.Elapsed.TotalMilliseconds,
                    new KeyValuePair<string, object?>("http.request.method", context.Request.Method),
                    new KeyValuePair<string, object?>("http.response.status_code", context.Response.StatusCode));
        }
    }
}
