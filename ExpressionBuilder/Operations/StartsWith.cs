﻿using ExpressionBuilder.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing a string "StartsWith" method call.
/// </summary>
public class StartsWith : OperationBase
{
    private readonly MethodInfo _startsWithMethod = typeof(string).GetMethod("StartsWith", [typeof(string)]);

    /// <inheritdoc />
    public StartsWith()
        : base("StartsWith", 1, TypeGroup.Text) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        Expression constant = constant1.TrimToLower();

        return Expression.Call(member.TrimToLower(), _startsWithMethod, constant)
            .AddNullCheck(member);
    }
}