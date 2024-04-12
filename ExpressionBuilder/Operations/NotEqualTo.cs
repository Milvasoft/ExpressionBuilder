using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing an inequality comparison.
/// </summary>
public class NotEqualTo : OperationBase
{
    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public NotEqualTo() : base(nameof(NotEqualTo), ValueCount, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        Expression constant = constant1;

        if (member.Type != typeof(string))
            return Expression.NotEqual(member, constant);

        constant = constant1.TrimToLower();

        return Expression.NotEqual(member.TrimToLower(), constant)
            .AddNullCheck(member);
    }
}