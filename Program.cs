using System.Diagnostics;
using ChallengePetApi.Authentication;
using ChallengePetApi.Data;
using ChallengePetApi.HealthChecks;
using ChallengePetApi.Observability;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Context;

var builder = WebApplication.CreateBuilder(args);

// SERILOG
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

builder.Host.UseSerilog();

// CONTROLLERS E SWAGGER
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationHandler.SchemeName,
        options => { }
    );

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(
        builder.Configuration.GetConnectionString("OracleConnection")
    )
);

builder.Services.AddHttpClient<ExternalServiceHealthCheck>();

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<AppDbContext>(
        name: "oracle_database",
        tags: new[] { "database" }
    )
    .AddCheck<ExternalServiceHealthCheck>(
        "external_service",
        tags: new[] { "external" }
    );

builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddMeter(ApiMetrics.MeterName)
            .AddConsoleExporter();
    });

var app = builder.Build();

app.Use(async (context, next) =>
{
    var stopwatch = Stopwatch.StartNew();

    var correlationId =
        context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
        ?? Guid.NewGuid().ToString();

    context.Response.Headers["X-Correlation-ID"] = correlationId;

    using (LogContext.PushProperty("CorrelationId", correlationId))
    {
        Log.Information(
            "Requisição iniciada: {Method} {Path}",
            context.Request.Method,
            context.Request.Path
        );

        try
        {
            ApiMetrics.Requests.Add(
                1,
                new KeyValuePair<string, object?>(
                    "method",
                    context.Request.Method
                )
            );

            await next();

            stopwatch.Stop();

            ApiMetrics.RequestDuration.Record(
                stopwatch.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>(
                    "method",
                    context.Request.Method
                ),
                new KeyValuePair<string, object?>(
                    "path",
                    context.Request.Path.Value
                )
            );

            if (context.Response.StatusCode >= 400)
            {
                ApiMetrics.Errors.Add(
                    1,
                    new KeyValuePair<string, object?>(
                        "status_code",
                        context.Response.StatusCode
                    )
                );
            }

            if (context.Response.StatusCode >= 400 &&
                context.Response.StatusCode < 500)
            {
                Log.Warning(
                    "Requisição finalizada com aviso. StatusCode: {StatusCode}",
                    context.Response.StatusCode
                );
            }
            else if (context.Response.StatusCode >= 500)
            {
                Log.Error(
                    "Requisição finalizada com erro. StatusCode: {StatusCode}",
                    context.Response.StatusCode
                );
            }
            else
            {
                Log.Information(
                    "Requisição finalizada com sucesso. StatusCode: {StatusCode}",
                    context.Response.StatusCode
                );
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            ApiMetrics.Errors.Add(1);

            ApiMetrics.RequestDuration.Record(
                stopwatch.Elapsed.TotalMilliseconds
            );

            Log.Error(
                ex,
                "Erro inesperado durante a requisição."
            );

            throw;
        }
    }
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program
{
}