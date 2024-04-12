using ExpressionBuilder.Common;
using Microsoft.Extensions.Configuration;

namespace ExpressionBuilder.Configuration;

public static class Settings
{
    public static List<SupportedType> SupportedTypes { get; internal set; } = [];

    public static void LoadSettingsFromConfigurationFile(IConfigurationManager configurationManager)
    {
        foreach (var supportedType in configurationManager.GetSection("supportedTypes").GetChildren())
        {
            var typeGroup = supportedType.GetValue<TypeGroup>("typeGroup");

            var type = Type.GetType(supportedType.GetValue<string>("Type"), false, true);

            if (type != null)
            {
                SupportedTypes.Add(new SupportedType { TypeGroup = typeGroup, Type = type });
            }
        }
    }

    public static void LoadSettings(List<SupportedType> supportedTypes) => SupportedTypes.AddRange(supportedTypes);
}