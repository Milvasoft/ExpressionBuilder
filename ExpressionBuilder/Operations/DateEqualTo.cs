using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Operation representing an date only equality comparison.
/// </summary>
public class DateEqualTo : OperationBase
{
    public static int ValueCount { get; } = 1;

    /// <inheritdoc />
    public DateEqualTo() : base(nameof(DateEqualTo), ValueCount, TypeGroup.Date) { }

    /// <inheritdoc />
    public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
    {
        if (Nullable.GetUnderlyingType(member.Type) != null)
        {
            var memberValue = Expression.Property(member, "Value");

            var dateMemberValue = Expression.Property(memberValue, "Date");

            return Expression.Equal(dateMemberValue, constant1)
                             .AddNullCheck(member);
        }

        var dateMember = Expression.Property(member, "Date");

        return Expression.Equal(dateMember, constant1);
    }
}