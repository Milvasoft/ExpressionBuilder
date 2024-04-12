using ExpressionBuilder.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing a string "EndsWith" method call.
/// </summary>
public class EndsWith : OperationBase
{
    private readonly MethodInfo _endsWithMethod = typeof(string).GetMethod(nameof(string.EndsWith), [typeof(string)]);

    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public EndsWith() : base(nameof(EndsWith), ValueCount, TypeGroup.Text) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        var constant = constant1.TrimToLower();

        return Expression.Call(member.TrimToLower(), _endsWithMethod, constant)
            .AddNullCheck(member);
    }
}