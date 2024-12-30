using ExpressionBuilder.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing a string "Contains" method call.
/// </summary>
public class Contains : OperationBase
{
    private readonly MethodInfo _stringContainsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);
    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public Contains() : base(nameof(Contains), ValueCount, TypeGroup.Text) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        Expression constant = constant1.Trim();

        return Expression.Call(member.Trim(), _stringContainsMethod, constant)
            .AddNullCheck(member);
    }
}