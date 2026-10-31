using PetCore.Exceptions;
using PetCore.Extensions;
using PetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicyName = "PublicApi";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
 

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        if (allowedOrigins.Contains("*") && builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        else if (allowedOrigins.Length > 0 && !allowedOrigins.Contains("*"))
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

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options => options.Filters.Add<PaginationHateoasFilter>());
var jwtKey = builder.Configuration["Jwt:Key"] ?? (builder.Environment.IsEnvironment("Testing") ? "test-only-signing-key-with-32-bytes-minimum" : null);
if (Encoding.UTF8.GetByteCount(jwtKey ?? string.Empty) < 32)
    throw new InvalidOperationException("Configure Jwt:Key com pelo menos 32 bytes via variável de ambiente Jwt__Key.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true, ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
        ValidateLifetime = true, ClockSkew = TimeSpan.FromSeconds(30)
    };
});
builder.Services.AddAuthorization(options => options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
}).AllowAnonymous();
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
}).AllowAnonymous();
app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");

app.Run();

/// <summary>
/// 
/// </summary>
public partial class Program;
