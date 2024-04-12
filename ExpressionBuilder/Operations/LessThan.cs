using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing an "less than" comparison.
/// </summary>
public class LessThan : OperationBase
{
    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public LessThan() : base(nameof(LessThan), ValueCount, TypeGroup.Number | TypeGroup.Date) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) => Expression.LessThan(member, constant1);
}