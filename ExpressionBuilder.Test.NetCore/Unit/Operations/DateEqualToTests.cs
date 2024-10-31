using ExpressionBuilder.Configuration;
using ExpressionBuilder.Operations;
using ExpressionBuilder.Test.Models;
using ExpressionBuilder.Test.Unit.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionBuilder.Test.Unit.Operations;

[TestFixture]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1862:Use the 'StringComparison' method overloads to perform case-insensitive string comparisons", Justification = "<Pending>")]
public class DateEqualToTests
{
    private TestData TestData { get; set; }

    public DateEqualToTests()
    {
        TestData = new TestData();
    }

    [TestCase("SalaryDate", "2024-10-31", true, TestName = "'DateEqualTo UTC 31' operation - Get expression (DateTime? value)")]
    [TestCase("SalaryDate", "2024-10-30", true, TestName = "'DateEqualTo UTC 30' operation - Get expression (DateTime? value)")]
    [TestCase("SalaryDate", "2024-10-29", false, TestName = "'DateEqualTo 29' operation - Get expression (DateTime? value)")]
    [TestCase("SalaryDate", "2024-10-28", false, TestName = "'DateEqualTo 28' operation - Get expression (DateTime? value)")]
    public void GetExpressionDateTimeValueTest(string propertyName, object value, bool useUtcConversionInDateTypes)
    {
        Settings.UseUtcConversionInDateTypes = useUtcConversionInDateTypes;
        var dateValue = DateTime.Parse(value.ToString());
        var startDate = Settings.UseUtcConversionInDateTypes ? dateValue.ToUniversalTime() : dateValue;
        var endDate = startDate.AddDays(1).AddTicks(-1);
        var operation = new DateEqualTo();
        var param = Expression.Parameter(typeof(Person), "x");
        var member = Expression.Property(param, propertyName);
        var constant1 = Expression.Constant(dateValue);

        BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, null);

        //Testing the operation structure

        expression.Left.Should().BeNullChecking(propertyName);
        expression.NodeType.Should().Be(ExpressionType.AndAlso);

        Assert.That(expression.Right, Is.AssignableTo<Expression>());

        var shouldSubjectLeft = (BinaryExpression)((BinaryExpression)expression.Right).Left;
        Assert.That((shouldSubjectLeft.Left as MemberExpression).Member.Name, Is.EqualTo("Value"));
        Assert.That(shouldSubjectLeft.NodeType, Is.EqualTo(ExpressionType.GreaterThanOrEqual));
        Assert.That(shouldSubjectLeft.Right, Is.AssignableFrom<ConstantExpression>());
        Assert.That((shouldSubjectLeft.Right as ConstantExpression).Value, Is.GreaterThanOrEqualTo(startDate));

        var shouldSubjectRight = (BinaryExpression)((BinaryExpression)expression.Right).Right;
        Assert.That((shouldSubjectRight.Left as MemberExpression).Member.Name, Is.EqualTo("Value"));
        Assert.That(shouldSubjectRight.NodeType, Is.EqualTo(ExpressionType.LessThanOrEqual));
        Assert.That(shouldSubjectRight.Right, Is.AssignableFrom<ConstantExpression>());
        Assert.That((shouldSubjectRight.Right as ConstantExpression).Value, Is.LessThanOrEqualTo(endDate));

        //Testing the operation execution
        var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
        var people = TestData.People.Where(lambda.Compile());
        var solutionMethod = (Func<Person, bool>)GetType().GetMethod(propertyName).Invoke(this, [startDate, endDate]);
        var solution = TestData.People.Where(solutionMethod);
        Assert.That(people, Is.EquivalentTo(solution));
    }

    public static Func<Person, bool> SalaryDate(DateTime startDate, DateTime endDate) => x => x.SalaryDate != null && (x.SalaryDate.Value >= startDate && x.SalaryDate.Value <= endDate);

    [TestCase("SalaryDateOffset", "2024-10-31", true, TestName = "'DateEqualTo Offset UTC 31' operation - Get expression (DateTimeOffset? value)")]
    [TestCase("SalaryDateOffset", "2024-10-30", true, TestName = "'DateEqualTo Offset UTC 30' operation - Get expression (DateTimeOffset? value)")]
    [TestCase("SalaryDateOffset", "2024-10-29", false, TestName = "'DateEqualTo Offset 29' operation - Get expression (DateTimeOffset? value)")]
    [TestCase("SalaryDateOffset", "2024-10-28", false, TestName = "'DateEqualTo Offset 28' operation - Get expression (DateTimeOffset? value)")]
    public void GetExpressionDateTimeOffsetValueTest(string propertyName, object value, bool useUtcConversionInDateTypes)
    {
        Settings.UseUtcConversionInDateTypes = useUtcConversionInDateTypes;
        var dateValue = DateTimeOffset.Parse(value.ToString());
        var startDate = Settings.UseUtcConversionInDateTypes ? dateValue.ToUniversalTime() : dateValue;
        var endDate = dateValue.AddDays(1).AddTicks(-1);
        var operation = new DateEqualTo();
        var param = Expression.Parameter(typeof(Person), "x");
        var member = Expression.Property(param, propertyName);
        var constant1 = Expression.Constant(dateValue);

        BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, null);

        //Testing the operation structure

        expression.Left.Should().BeNullChecking(propertyName);
        expression.NodeType.Should().Be(ExpressionType.AndAlso);

        Assert.That(expression.Right, Is.AssignableTo<Expression>());

        var shouldSubjectLeft = (BinaryExpression)((BinaryExpression)expression.Right).Left;
        Assert.That((shouldSubjectLeft.Left as MemberExpression).Member.Name, Is.EqualTo("Value"));
        Assert.That(shouldSubjectLeft.NodeType, Is.EqualTo(ExpressionType.GreaterThanOrEqual));
        Assert.That(shouldSubjectLeft.Right, Is.AssignableFrom<ConstantExpression>());
        Assert.That((shouldSubjectLeft.Right as ConstantExpression).Value, Is.LessThanOrEqualTo(dateValue));

        var shouldSubjectRight = (BinaryExpression)((BinaryExpression)expression.Right).Right;
        Assert.That((shouldSubjectRight.Left as MemberExpression).Member.Name, Is.EqualTo("Value"));
        Assert.That(shouldSubjectRight.NodeType, Is.EqualTo(ExpressionType.LessThanOrEqual));
        Assert.That(shouldSubjectRight.Right, Is.AssignableFrom<ConstantExpression>());
        Assert.That((shouldSubjectRight.Right as ConstantExpression).Value, Is.GreaterThanOrEqualTo(dateValue));

        //Testing the operation execution
        var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
        var people = TestData.People.Where(lambda.Compile());
        var solutionMethod = (Func<Person, bool>)GetType().GetMethod(propertyName).Invoke(this, [startDate, endDate]);
        var solution = TestData.People.Where(solutionMethod);
        Assert.That(people, Is.EquivalentTo(solution));
    }

    public static Func<Person, bool> SalaryDateOffset(DateTimeOffset startDate, DateTimeOffset endDate) => x => x.SalaryDateOffset != null && (x.SalaryDateOffset.Value >= startDate && x.SalaryDateOffset.Value <= endDate);
}