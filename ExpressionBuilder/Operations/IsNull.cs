using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing a null check.
/// </summary>
public class IsNull : OperationBase
{
    public static int ValueCount { get; } = 0;

    /// <inheritdoc />
    public IsNull() : base(nameof(IsNull), ValueCount, TypeGroup.Text | TypeGroup.Nullable, expectNullValues: true) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) => Expression.Equal(member, Expression.Constant(null));
}