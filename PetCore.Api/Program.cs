using PetCore.Exceptions;
using PetCore.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicyName = "PublicApi";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["*"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        if (allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
        }
    });
});

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "PetCore.Api"));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSwagger();

builder.Services.AddServices();

builder.Services.AddRepositories();

builder.Services.AddDBContext(builder.Configuration);
builder.Services.AddPetCoreObservability(builder.Configuration);

Console.WriteLine("Inicializando os serviços da API...");

var app = builder.Build();
Console.WriteLine("Serviços inicializados. Iniciando servidor HTTP...");

// Configure the HTTP request pipeline.
if (app.Configuration.GetValue("Swagger:Enabled", true))
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PetCore API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicyName);

app.UseCorrelationId();
app.UseSerilogRequestLogging();
app.UseApiMetrics();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");

app.Run();

/// <summary>
/// 
/// </summary>
public partial class Program;
