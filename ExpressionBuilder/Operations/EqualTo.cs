using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing an equality comparison.
/// </summary>
public class EqualTo : OperationBase
{
    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public EqualTo() : base(nameof(EqualTo), ValueCount, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        Expression constant = constant1.ConvertUtcIfRequested();

        if (member.Type != typeof(string))
            return Expression.Equal(member, constant);

        constant = constant1.TrimToLower();

        return Expression.Equal(member.TrimToLower(), constant)
            .AddNullCheck(member);
    }
}