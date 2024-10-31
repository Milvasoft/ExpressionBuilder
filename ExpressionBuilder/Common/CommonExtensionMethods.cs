using ExpressionBuilder.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Common;

public static class CommonExtensionMethods
{
    private static readonly MethodInfo _trimMethod = typeof(string).GetMethod("Trim", []);
    private static readonly MethodInfo _toLowerMethod = typeof(string).GetMethod("ToLower", []);

    /// <summary>
    /// Gets a member expression for an specific property
    /// </summary>
    /// <param name="param"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static MemberExpression GetMemberExpression(this ParameterExpression param, string propertyName) => GetMemberExpression((Expression)param, propertyName);

    private static MemberExpression GetMemberExpression(Expression param, string propertyName)
    {
        if (!propertyName.Contains('.'))
        {
            return Expression.PropertyOrField(param, propertyName);
        }

        var index = propertyName.IndexOf('.');
        var subParam = Expression.PropertyOrField(param, propertyName[..index]);
        return GetMemberExpression(subParam, propertyName[(index + 1)..]);
    }

    /// <summary>
    /// Applies the string Trim and ToLower methods to an ExpressionMember.
    /// </summary>
    /// <param name="member">Member to which to methods will be applied.</param>
    /// <returns></returns>
    public static Expression TrimToLower(this MemberExpression member)
    {
        var trimMemberCall = Expression.Call(member, _trimMethod);
        return Expression.Call(trimMemberCall, _toLowerMethod);
    }

    /// <summary>
    /// Applies the string Trim and ToLower methods to an ExpressionMember.
    /// </summary>
    /// <param name="constant">Constant to which to methods will be applied.</param>
    /// <returns></returns>
    public static Expression TrimToLower(this ConstantExpression constant)
    {
        var trimMemberCall = Expression.Call(constant, _trimMethod);
        return Expression.Call(trimMemberCall, _toLowerMethod);
    }

    /// <summary>
    /// Adds a "null check" to the expression (before the original one).
    /// </summary>
    /// <param name="expression">Expression to which the null check will be pre-pended.</param>
    /// <param name="member">Member that will be checked.</param>
    /// <returns></returns>
    public static Expression AddNullCheck(this Expression expression, MemberExpression member)
    {
        Expression memberIsNotNull = Expression.NotEqual(member, Expression.Constant(null));
        return Expression.AndAlso(memberIsNotNull, expression);
    }

    /// <summary>
    /// Checks if an object is a generic list.
    /// </summary>
    /// <param name="o">Object to be tested.</param>
    /// <returns>TRUE if the object is a generic list.</returns>
    public static bool IsGenericList(this object o)
    {
        var oType = o.GetType();
        return oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(List<>);
    }

    /// <summary>
    /// If constant type is date tyep it will converts constant value to utc if requested in the <see cref="Settings"/>.
    /// </summary>
    /// <param name="constant"></param>
    /// <returns></returns>
    public static ConstantExpression ConvertUtcIfRequested(this ConstantExpression constant)
    {
        if (constant.Type == typeof(DateTime) || constant.Type == typeof(DateTime?))
        {
            var valueAsDateTime = (DateTime)constant.Value;

            DateTime dateValue = new(valueAsDateTime.Year, valueAsDateTime.Month, valueAsDateTime.Day, 0, 0, 0, DateTimeKind.Local);

            if (Settings.UseUtcConversionInDateTypes)
                dateValue = dateValue.ToUniversalTime();

            constant = Expression.Constant(dateValue);
        }
        else if (constant.Type == typeof(DateTimeOffset) || constant.Type == typeof(DateTimeOffset?))
        {
            var valueAsDateTimeOffset = (DateTimeOffset)constant.Value;

            DateTimeOffset dateValue = new(valueAsDateTimeOffset.Year, valueAsDateTimeOffset.Month, valueAsDateTimeOffset.Day, 0, 0, 0, valueAsDateTimeOffset.Offset);

            if (Settings.UseUtcConversionInDateTypes)
                dateValue = dateValue.ToOffset(TimeSpan.Zero);

            constant = Expression.Constant(dateValue);
        }

        return constant;
    }
}