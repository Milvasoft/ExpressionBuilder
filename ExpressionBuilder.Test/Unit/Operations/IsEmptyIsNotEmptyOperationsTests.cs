using ExpressionBuilder.Interfaces;
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
public class IsEmptyIsNotEmptyOperationsTests
{
    private TestData TestData { get; set; }

    public IsEmptyIsNotEmptyOperationsTests()
    {
        TestData = new TestData();
    }

    [TestCase(ExpressionType.Equal, "IsEmpty", TestName = "'IsEmpty' operation - Get expression")]
    [TestCase(ExpressionType.NotEqual, "IsNotEmpty", TestName = "'IsNotEmpty' operation - Get expression")]
    public void GetExpressionTest(ExpressionType comparisonType, string methodName)
    {
        var propertyName = "Country";
        var value = string.Empty;
        var type = typeof(IFilter).Assembly.Types()
            .Single(t => t.FullName == "ExpressionBuilder.Operations." + methodName);
        var operation = (IOperation)Activator.CreateInstance(type);
        var param = Expression.Parameter(typeof(Person), "x");
        var parent = Expression.Property(param, "Birth");
        var member = Expression.Property(parent, "Country");
        var constant1 = Expression.Constant(4000D);
        var constant2 = Expression.Constant(5000D);

        BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, constant2);

        //Testing the operation structure
        expression.Left.Should().BeNullChecking(propertyName);
        expression.NodeType.Should().Be(ExpressionType.AndAlso);

        expression.Right.Should().BeAStringExpressionCheckingIf(propertyName, comparisonType, value, false);

        //Testing the operation execution
        var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
        var people = TestData.People.Where(lambda.Compile());
        var solutionMethod = (Func<Person, bool>)GetType().GetMethod(methodName).Invoke(this, []);
        var solution = TestData.People.Where(solutionMethod);
        Assert.That(people, Is.EquivalentTo(solution));
    }

    public static Func<Person, bool> IsEmpty() => x => x.Birth != null && (x.Birth.Country != null && x.Birth.Country.Trim().ToLower() == string.Empty);

    public static Func<Person, bool> IsNotEmpty() => x => x.Birth != null && (x.Birth.Country != null && x.Birth.Country.Trim().ToLower() != string.Empty);
}