using Utilities;

var instanceId = Guid.NewGuid().ToString();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddApplicationInsights(
    builder.Configuration,
    trace => trace.Properties.Add("Custom Property", "awesome-value"));

var app = builder.Build();

app.Logger.LogInformation("Application started with InstanceId {InstanceId}", instanceId);

app.UseExceptionHandler(app.Environment);

app.MapGet("/ok", (ILogger<Program> logger, HttpContext context) =>
{
    logger.LogCritical("Critical: {InstanceId}", instanceId);
    logger.LogError("Error: {InstanceId}", instanceId);
    logger.LogWarning("Warning: {InstanceId}", instanceId);
    logger.LogInformation("Information: {InstanceId}", instanceId);
    logger.LogDebug("Debug: {InstanceId}", instanceId);
    logger.LogTrace("Trace: {InstanceId}", instanceId);

    var activityId = context.ActivityId();
    logger.LogWarning("context.ActivityId = {ActivityCurrentId}", activityId);

    return new
    {
        ActivityId = activityId,
        InstanceId = instanceId,
    };
});

app.MapGet("/exception", (ILogger<Program> logger, HttpContext context) =>
{
    logger.LogWarning("context.ActivityId {ActivityCurrentId}", context.ActivityId());
    throw new InvalidOperationException("Sample exception");
});

app.Run();
