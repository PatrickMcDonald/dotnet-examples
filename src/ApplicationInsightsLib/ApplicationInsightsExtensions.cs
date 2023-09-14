using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Logging;

public static class ApplicationInsightsExtensions
{
    public static ILoggingBuilder AddApplicationInsights(this ILoggingBuilder logging, IConfiguration configuration, Action<ISupportProperties>? configureProperties = null)
    {
        logging.AddApplicationInsights(
            configureTelemetryConfiguration: config =>
            {
                config.ConnectionString = configuration.GetConnectionString("ApplicationInsights");

                if (configureProperties is not null)
                {
                    config.TelemetryInitializers.Add(new CustomTelemetry(configureProperties));
                }
            },
            configureApplicationInsightsLoggerOptions: _ => { }
        );

        return logging;
    }

    private class CustomTelemetry : ITelemetryInitializer
    {
        private readonly Action<ISupportProperties> _configureProperties;

        public CustomTelemetry(Action<ISupportProperties> configureProperties)
        {
            _configureProperties = configureProperties;
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is ISupportProperties supportProperties)
            {
                _configureProperties(supportProperties);
            }
        }
    }
}
