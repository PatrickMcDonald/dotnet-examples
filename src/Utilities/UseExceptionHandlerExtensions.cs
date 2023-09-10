using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Utilities;

namespace Microsoft.Extensions.DependencyInjection;

public static class UseExceptionHandlerExtensions
{
    private const string TraceId = "traceid";

    public static void UseExceptionHandler(this IApplicationBuilder app, IHostEnvironment environment, bool suppressDetailed = false)
    {
        var detailed = environment.IsDevelopment() && !suppressDetailed;

        app.UseExceptionHandler(app => app.Run(async httpContext =>
        {
            var problemDetails = httpContext.ProblemDetails(detailed);

            await Results.Problem(problemDetails).ExecuteAsync(httpContext);
        }));
    }

    private static ProblemDetails ProblemDetails(this HttpContext httpContext, bool detailed)
    {
        var problemDetails = new ProblemDetails();

        if (httpContext.ActivityId() is { } traceId)
        {
            problemDetails.Extensions[TraceId] = traceId;
        }

        if (detailed && httpContext.Features.GetRequiredFeature<IExceptionHandlerFeature>().Error is var error)
        {
            problemDetails.Detail = error.StackTrace;
            problemDetails.Title = error.Message;
        }

        return problemDetails;
    }
}
