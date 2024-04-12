using ExpressionBuilder.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressionBuilder;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionBuilder(this IServiceCollection services, IConfigurationManager configurationManager = null)
    {
        if (configurationManager != null)
            Settings.LoadSettingsFromConfigurationFile(configurationManager);

        return services;
    }
}
