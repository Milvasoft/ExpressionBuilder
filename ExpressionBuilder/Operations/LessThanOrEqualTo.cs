using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing an "less than or equal" comparison.
/// </summary>
public class LessThanOrEqualTo : OperationBase
{
    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public LessThanOrEqualTo() : base(nameof(LessThanOrEqualTo), ValueCount, TypeGroup.Number | TypeGroup.Date) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) => Expression.LessThanOrEqual(member, constant1.ConvertUtcIfRequested());
}