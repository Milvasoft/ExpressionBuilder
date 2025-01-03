﻿using ExpressionBuilder.Test.Models;
using ExpressionBuilder.Test.Unit.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Test.Unit.Operations;

[TestFixture]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1862:Use the 'StringComparison' method overloads to perform case-insensitive string comparisons", Justification = "<Pending>")]
public class DoesNotContainTests
{
    private TestData TestData { get; set; }

    public DoesNotContainTests()
    {
        TestData = new TestData();
    }

    [TestCase(TestName = "'DoesNotContain' operation - Get expression")]
    public void GetExpressionTest()
    {
        var propertyName = "Name";
        var value = "Doe ";
        var operation = new ExpressionBuilder.Operations.DoesNotContain();
        var param = Expression.Parameter(typeof(Person), "x");
        var member = Expression.Property(param, propertyName);
        var constant1 = Expression.Constant(value);

        var expression = (BinaryExpression)operation.GetExpression(member, constant1, null);

        //Testing the operation structure
        expression.Left.Should().BeNullChecking(propertyName);
        expression.NodeType.Should().Be(ExpressionType.AndAlso);

        var not = (expression.Right as UnaryExpression);
        not.NodeType.Should().Be(ExpressionType.Not);

        var doesNotContain = (not.Operand as MethodCallExpression);
        doesNotContain.Method.Should().BeAssignableTo<MethodInfo>();
        var method = doesNotContain.Method;
        method.Name.Should().Be("Contains");

        var property = doesNotContain.Object.ExtractTrimProperty();
        property.Member.Name.Should().Be(propertyName);

        var constant = doesNotContain.Arguments[0].ExtractTrimConstant();
        constant.Value.Should().Be(value);

        //Testing the operation execution
        var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
        var people = TestData.People.Where(lambda.Compile());
        var solution = TestData.People.Where(x => !x.Name.Trim().Contains("Doe"));
        people.Should().BeEquivalentTo(solution);
    }
}