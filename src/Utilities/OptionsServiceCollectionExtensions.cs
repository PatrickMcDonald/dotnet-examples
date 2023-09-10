using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsServiceCollectionExtensions
{
    public static void AddOptionsWithValidateOnStart<TOptions>(this IServiceCollection services, string configSectionPath)
        where TOptions : class
    {
        services.AddOptions<TOptions>()
            .BindConfiguration(configSectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped(provider => provider.GetRequiredService<IOptionsMonitor<TOptions>>().CurrentValue);
    }
}
