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

    [TestCase("SalaryDate", "2024-10-29", TestName = "'DateEqualTo' operation - Get expression (DateTime? value)")]
    public void GetExpressionDateTimeValueTest(string propertyName, object value)
    {
        var dateValue = DateTime.Parse(value.ToString());
        dateValue = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, 0, 0, 0, DateTimeKind.Utc);
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
        var solutionMethod = (Func<Person, bool>)GetType().GetMethod(propertyName).Invoke(this, [dateValue, endDate]);
        var solution = TestData.People.Where(solutionMethod);
        Assert.That(people, Is.EquivalentTo(solution));
    }

    public static Func<Person, bool> SalaryDate(DateTime startDate, DateTime endDate) => x => x.SalaryDate != null && (x.SalaryDate.Value >= startDate && x.SalaryDate.Value <= endDate);

    [TestCase("SalaryDateOffset", "2024-10-29", TestName = "'DateEqualTo Offset' operation - Get expression (DateTimeOffset? value)")]
    public void GetExpressionDateTimeOffsetValueTest(string propertyName, object value)
    {
        var dateValue = DateTimeOffset.Parse(value.ToString());
        dateValue = new DateTimeOffset(dateValue.Year, dateValue.Month, dateValue.Day, 0, 0, 0, TimeSpan.Zero);
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
        var solutionMethod = (Func<Person, bool>)GetType().GetMethod(propertyName).Invoke(this, [dateValue, endDate]);
        var solution = TestData.People.Where(solutionMethod);
        Assert.That(people, Is.EquivalentTo(solution));
    }

    public static Func<Person, bool> SalaryDateOffset(DateTimeOffset startDate, DateTimeOffset endDate) => x => x.SalaryDate != null && (x.SalaryDate.Value >= startDate && x.SalaryDate.Value <= endDate);
}