using ExpressionBuilder.Common;
using System.Collections;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing the inverse of a list "Contains" method call.
/// </summary>
public class NotIn : OperationBase
{
    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public NotIn() : base(nameof(NotIn), ValueCount, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text, true, true) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        if (constant1.Value is not IList || !constant1.Value.GetType().IsGenericType)
            throw new ArgumentException("The 'NotIn' operation only supports lists as parameters.");

        var type = constant1.Value.GetType();
        var inInfo = type.GetMethod("Contains", [type.GetGenericArguments()[0]]);
        var contains = Expression.Call(constant1, inInfo, member);
        return Expression.Not(contains);
    }
}