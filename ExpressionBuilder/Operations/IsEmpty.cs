using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing a check for an empty string.
/// </summary>
public class IsEmpty : OperationBase
{
    public static int ValueCount { get; } = 0;

    /// <inheritdoc />
    public IsEmpty() : base(nameof(IsEmpty), ValueCount, TypeGroup.Text) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) => Expression.Equal(member.TrimToLower(), Expression.Constant(string.Empty))
            .AddNullCheck(member);
}