using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing a "not-null" check.
/// </summary>
public class IsNotNull : OperationBase
{
    public static int ValueCount { get; } = 0;

    /// <inheritdoc />
    public IsNotNull() : base(nameof(IsNotNull), ValueCount, TypeGroup.Text | TypeGroup.Nullable) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) => Expression.NotEqual(member, Expression.Constant(null));
}