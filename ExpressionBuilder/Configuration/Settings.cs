using ExpressionBuilder.Common;
using ExpressionBuilder.Helpers;
using Microsoft.Extensions.Configuration;

namespace ExpressionBuilder.Configuration;

public static class Settings
{
    public static void LoadSettingsFromConfigurationFile(IConfigurationManager configurationManager)
    {
        foreach (var supportedType in configurationManager.GetSection("supportedTypes").GetChildren())
        {
            var typeGroup = supportedType.GetValue<TypeGroup>("typeGroup");

            var type = Type.GetType(supportedType.GetValue<string>("Type"), false, true);

            if (type != null)
                OperationHelper.TypeGroups[typeGroup].Add(type);
        }
    }

    public static void LoadSettings(List<SupportedType> supportedTypes)
    {
        foreach (var supportedType in supportedTypes)
        {
            if (supportedType.Type != null)
                OperationHelper.TypeGroups[supportedType.TypeGroup].Add(supportedType.Type);
        }
    }
}