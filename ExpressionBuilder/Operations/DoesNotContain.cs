﻿using ExpressionBuilder.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation that checks for the non-existence of a substring within another string.
/// </summary>
public class DoesNotContain : OperationBase
{
    private readonly MethodInfo _stringContainsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);

    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public DoesNotContain() : base(nameof(DoesNotContain), ValueCount, TypeGroup.Text) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        var constant = constant1.Trim();

        return Expression.Not(Expression.Call(member.Trim(), _stringContainsMethod, constant))
            .AddNullCheck(member);
    }
}