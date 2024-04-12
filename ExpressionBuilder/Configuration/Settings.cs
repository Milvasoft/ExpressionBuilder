using ExpressionBuilder.Common;
using Microsoft.Extensions.Configuration;

namespace ExpressionBuilder.Configuration;

public class Settings
{
    public List<SupportedType> SupportedTypes { get; private set; }

    public static void LoadSettings(Settings settings)
    {
        var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                                                .AddJsonFile("appsettings.json",
                                                             optional: true,
                                                             reloadOnChange: true);

        var config = builder.Build();

        settings.SupportedTypes = [];

        foreach (var supportedType in config.GetSection("supportedTypes").GetChildren())
        {
            var typeGroup = supportedType.GetValue<TypeGroup>("typeGroup");
            var type = Type.GetType(supportedType.GetValue<string>("Type"), false, true);
            if (type != null)
            {
                settings.SupportedTypes.Add(new SupportedType { TypeGroup = typeGroup, Type = type });
            }
        }
    }
}