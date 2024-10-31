using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing an "greater than or equal" comparison.
/// </summary>
public class GreaterThanOrEqualTo : OperationBase
{
    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public GreaterThanOrEqualTo() : base(nameof(GreaterThanOrEqualTo), ValueCount, TypeGroup.Number | TypeGroup.Date) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) => Expression.GreaterThanOrEqual(member, constant1.ConvertUtcIfRequested());
}