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
        if (!IsDateType(constant1) || constant1.Value == null)
            throw new InvalidDataException("DateEqualTo can be used with only date types and cannot be null");

        var (startDateExpression, endDateExpression) = GetStartAndEndDates(constant1);

        var memberExpression = member;

        if (Nullable.GetUnderlyingType(member.Type) != null)
        {
            memberExpression = Expression.Property(member, "Value");
        }

        var left = Expression.GreaterThanOrEqual(memberExpression, startDateExpression);
        var right = Expression.LessThanOrEqual(memberExpression, endDateExpression);

        return Nullable.GetUnderlyingType(member.Type) != null ? Expression.AndAlso(left, right).AddNullCheck(member) : Expression.AndAlso(left, right);
    }

    private static (ConstantExpression, ConstantExpression) GetStartAndEndDates(ConstantExpression constant)
    {
        ConstantExpression startDateExpression = constant;
        ConstantExpression endDateExpression = constant;

        if (constant.Type == typeof(DateTime) || constant.Type == typeof(DateTime?))
        {
            var valueAsDateTime = (DateTime)constant.Value;
            DateTime startDate = new(valueAsDateTime.Year, valueAsDateTime.Month, valueAsDateTime.Day, 0, 0, 0, kind: DateTimeKind.Utc);
            DateTime endDate = startDate.AddDays(1).AddTicks(-1);

            startDateExpression = Expression.Constant(startDate);
            endDateExpression = Expression.Constant(endDate);
        }
        else if (constant.Type == typeof(DateTimeOffset) || constant.Type == typeof(DateTimeOffset?))
        {
            var valueAsDateTimeOffset = (DateTimeOffset)constant.Value;
            DateTimeOffset startDate = new(valueAsDateTimeOffset.Year, valueAsDateTimeOffset.Month, valueAsDateTimeOffset.Day, 0, 0, 0, TimeSpan.Zero); // UTC
            DateTimeOffset endDate = startDate.AddDays(1).AddTicks(-1);

            startDateExpression = Expression.Constant(startDate);
            endDateExpression = Expression.Constant(endDate);
        }

        return (startDateExpression, endDateExpression);
    }

    private static bool IsDateType(ConstantExpression constant) => constant.Type == typeof(DateTime)
                                                                || constant.Type == typeof(DateTime?)
                                                                || constant.Type == typeof(DateTimeOffset)
                                                                || constant.Type == typeof(DateTimeOffset?);
}