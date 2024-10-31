using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing a range comparison.
/// </summary>
public class Between : OperationBase
{
    public static int ValueCount { get; } = 2;

    /// <inheritdoc />
    public Between() : base(nameof(Between), ValueCount, TypeGroup.Number | TypeGroup.Date) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        var left = Expression.GreaterThanOrEqual(member, constant1.ConvertUtcIfRequested());
        var right = Expression.LessThanOrEqual(member, constant2.ConvertUtcIfRequested());

        return Expression.AndAlso(left, right);
    }
}