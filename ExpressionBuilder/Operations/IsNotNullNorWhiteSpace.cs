﻿using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing a "not null nor whitespace" check.
/// </summary>
public class IsNotNullNorWhiteSpace : OperationBase
{
    public static int ValueCount { get; } = 0;

    /// <inheritdoc />
    public IsNotNullNorWhiteSpace() : base(nameof(IsNotNullNorWhiteSpace), ValueCount, TypeGroup.Text) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        Expression exprNull = Expression.Constant(null);
        Expression exprEmpty = Expression.Constant(string.Empty);

        return Expression.AndAlso(
            Expression.NotEqual(member, exprNull),
            Expression.NotEqual(member.Trim(), exprEmpty));
    }
}