﻿using ExpressionBuilder.Operations;
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
public class EqualToTests
{
    private TestData TestData { get; set; }

    public EqualToTests()
    {
        TestData = new TestData();
    }

    [TestCase("Name", " John doe ", TestName = "'EqualTo' operation - Get expression (string value)")]
    [TestCase("Salary", 3500D, TestName = "'EqualTo' operation - Get expression (Failure: string property with integer value)")]
    public void GetExpressionStringValueTest(string propertyName, object value)
    {
        var operation = new EqualTo();
        var param = Expression.Parameter(typeof(Person), "x");
        var member = Expression.Property(param, propertyName);
        var constant1 = Expression.Constant(value);

        BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, null);

        //Testing the operation structure
        if (value is string)
        {
            expression.Left.Should().BeNullChecking(propertyName);
            expression.NodeType.Should().Be(ExpressionType.AndAlso);
            expression.Right.Should().BeAStringExpressionCheckingIf(propertyName, ExpressionType.Equal, value);
        }
        else
        {
            expression.Should().BeAnExpressionCheckingIf(propertyName, ExpressionType.Equal, value);
        }

        //Testing the operation execution
        var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
        var people = TestData.People.Where(lambda.Compile());
        var solutionMethod = (Func<Person, bool>)GetType().GetMethod(propertyName).Invoke(this, [value]);
        var solution = TestData.People.Where(solutionMethod);
        Assert.That(people, Is.EquivalentTo(solution));
    }

    public static Func<Person, bool> Name(string value) => x => x.Name.Trim() == value.ToString().Trim();

    public static Func<Person, bool> Salary(double value) => x => x.Salary == value;
}