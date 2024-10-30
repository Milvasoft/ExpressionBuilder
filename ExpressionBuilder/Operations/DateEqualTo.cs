﻿using ExpressionBuilder.Common;
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

        if (Nullable.GetUnderlyingType(member.Type) != null)
        {
            var memberValue = Expression.Property(member, "Value");

            var (startDateExpression, endDateExpression) = GetStartAndEndDates(constant1);

            var left = Expression.GreaterThanOrEqual(memberValue, startDateExpression);
            var right = Expression.LessThanOrEqual(memberValue, endDateExpression);

            return Expression.AndAlso(left, right).AddNullCheck(member);
        }

        var dateMember = Expression.Property(member, "Date");

        return Expression.Equal(dateMember, constant1);
    }

    private static (ConstantExpression, ConstantExpression) GetStartAndEndDates(ConstantExpression constant)
    {
        ConstantExpression startDateExpression = constant;
        ConstantExpression endDateExpression = constant;

        if (constant.Type == typeof(DateTime) || constant.Type == typeof(DateTime?))
        {
            DateTime startDate = ((DateTime)constant.Value).Date; // Tarihin başlangıcı (saat 00:00:00)
            DateTime endDate = startDate.AddDays(1).AddTicks(-1); // Tarihin sonu (saat 23:59:59.9999999)

            startDateExpression = Expression.Constant(startDate);
            endDateExpression = Expression.Constant(endDate);
        }
        else if (constant.Type == typeof(DateTimeOffset) || constant.Type == typeof(DateTimeOffset?))
        {
            DateTimeOffset startDate = ((DateTimeOffset)constant.Value).Date; // Tarihin başlangıcı (saat 00:00:00)
            DateTimeOffset endDate = startDate.AddDays(1).AddTicks(-1); // Tarihin sonu (saat 23:59:59.9999999)

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