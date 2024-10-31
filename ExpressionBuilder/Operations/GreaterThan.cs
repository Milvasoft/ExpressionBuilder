using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing an "greater than" comparison.
/// </summary>
public class GreaterThan : OperationBase
{
    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public GreaterThan() : base(nameof(GreaterThan), ValueCount, TypeGroup.Number | TypeGroup.Date) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) => Expression.GreaterThan(member, constant1.ConvertUtcIfRequested());
}