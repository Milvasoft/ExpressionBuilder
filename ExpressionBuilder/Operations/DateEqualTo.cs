using ExpressionBuilder.Common;
using ExpressionBuilder.Configuration;
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
        if (!IsDateType(constant1))
            throw new InvalidDataException("DateEqualTo can be used with only date types");

        if (constant1.Value == null)
            return Expression.Equal(member, constant1);

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

            DateTime startDate = new(valueAsDateTime.Year, valueAsDateTime.Month, valueAsDateTime.Day, 0, 0, 0, DateTimeKind.Local);
            DateTime endDate = startDate.AddDays(1).AddTicks(-1);

            if (Settings.UseUtcConversionInDateTypes)
            {
                startDate = startDate.ToUniversalTime();
                endDate = endDate.ToUniversalTime();
            }

            startDateExpression = Expression.Constant(startDate);
            endDateExpression = Expression.Constant(endDate);
        }
        else if (constant.Type == typeof(DateTimeOffset) || constant.Type == typeof(DateTimeOffset?))
        {
            var valueAsDateTimeOffset = (DateTimeOffset)constant.Value;

            // Kullanıcının saat dilimine göre gün başlangıcı ve sonu
            DateTimeOffset startDate = new(valueAsDateTimeOffset.Year, valueAsDateTimeOffset.Month, valueAsDateTimeOffset.Day, 0, 0, 0, valueAsDateTimeOffset.Offset);
            DateTimeOffset endDate = startDate.AddDays(1).AddTicks(-1);

            if (Settings.UseUtcConversionInDateTypes)
            {
                startDate = startDate.ToOffset(TimeSpan.Zero);
                endDate = endDate.ToOffset(TimeSpan.Zero);
            }
            // UTC'ye dönüştürme

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