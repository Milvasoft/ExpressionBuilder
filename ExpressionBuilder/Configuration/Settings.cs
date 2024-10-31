using ExpressionBuilder.Common;
using ExpressionBuilder.Helpers;
using Microsoft.Extensions.Configuration;

namespace ExpressionBuilder.Configuration;

public static class Settings
{
    public const string SectionName = "Milvasoft:ExpressionBuilder";

    /// <summary>
    /// Uses utc conversion in date types searchs. Default is true.
    /// </summary>
    public static bool UseUtcConversionInDateTypes { get; set; } = true;

    public static void LoadSettingsFromConfigurationFile(IConfigurationManager configurationManager)
    {
        UseUtcConversionInDateTypes = configurationManager.GetSection($"{SectionName}:UseUtcConversionInDateTypes").Get<bool>();

        foreach (var supportedType in configurationManager.GetSection($"{SectionName}:SupportedTypes").GetChildren())
        {
            var typeGroup = supportedType.GetValue<TypeGroup>("typeGroup");

            var type = Type.GetType(supportedType.GetValue<string>("Type"), false, true);

            if (type != null)
                OperationHelper.TypeGroups[typeGroup].Add(type);
        }
    }

    public static void LoadSettings(List<SupportedType> supportedTypes, bool useUtcConversionInDateTypes = true)
    {
        UseUtcConversionInDateTypes = useUtcConversionInDateTypes;

        foreach (var supportedType in supportedTypes)
        {
            if (supportedType.Type != null)
                OperationHelper.TypeGroups[supportedType.TypeGroup].Add(supportedType.Type);
        }
    }
}