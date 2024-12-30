using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing a check for a non-empty string.
/// </summary>
public class IsNotEmpty : OperationBase
{
    public static int ValueCount { get; } = 0;

    /// <inheritdoc />
    public IsNotEmpty() : base(nameof(IsNotEmpty), ValueCount, TypeGroup.Text) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) => Expression.NotEqual(member.Trim(), Expression.Constant(string.Empty))
            .AddNullCheck(member);
}