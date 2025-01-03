﻿using FluentAssertions;
using FluentAssertions.Primitives;
using NUnit.Framework;
using System.Linq.Expressions;

namespace ExpressionBuilder.Test.Unit.Helpers;

public static class ExtensionMethods
{
    public static AndConstraint<ObjectAssertions> BeNullChecking(this ObjectAssertions should, string propertyName, bool not = false)
    {
        Assert.That(should.Subject, Is.AssignableTo<Expression>());
        var nullCheck = (BinaryExpression)should.Subject;
        Assert.That(nullCheck.Left, Is.AssignableTo<MemberExpression>());
        Assert.That((nullCheck.Left as MemberExpression).Member.Name, Is.EqualTo(propertyName));
        Assert.That(nullCheck.NodeType, Is.EqualTo(not ? ExpressionType.Equal : ExpressionType.NotEqual));
        Assert.That(nullCheck.Right, Is.AssignableFrom<ConstantExpression>());
        Assert.That((nullCheck.Right as ConstantExpression).Value, Is.EqualTo(null));

        return new AndConstraint<ObjectAssertions>(should);
    }

    public static AndConstraint<ObjectAssertions> BeAnExpressionCheckingIf(this ObjectAssertions should, string propertyName, ExpressionType expressionType, object value)
    {
        Assert.That(should.Subject, Is.AssignableTo<Expression>());
        var expression = (BinaryExpression)should.Subject;
        Assert.That(expression.Left, Is.AssignableTo<MemberExpression>());
        Assert.That((expression.Left as MemberExpression).Member.Name, Is.EqualTo(propertyName));
        Assert.That(expression.NodeType, Is.EqualTo(expressionType));
        Assert.That(expression.Right, Is.AssignableFrom<ConstantExpression>());
        Assert.That((expression.Right as ConstantExpression).Value, Is.EqualTo(value));

        return new AndConstraint<ObjectAssertions>(should);
    }

    public static AndConstraint<ObjectAssertions> BeAStringExpressionCheckingIf(this ObjectAssertions should, string propertyName, ExpressionType expressionType, object value, bool trimToLowerValue = true)
    {
        Assert.That(should.Subject, Is.AssignableTo<Expression>());
        var expression = (BinaryExpression)should.Subject;

        var property = expression.Left.ExtractTrimProperty();
        property.Member.Name.Should().Be(propertyName);

        Assert.That(expression.NodeType, Is.EqualTo(expressionType));

        var constant = trimToLowerValue ? expression.Right.ExtractTrimConstant() : (ConstantExpression)expression.Right;
        constant.Value.Should().Be(value);

        return new AndConstraint<ObjectAssertions>(should);
    }

    public static MemberExpression ExtractTrimProperty(this Expression expression)
    {
        var trim = (MethodCallExpression)expression;
        return (MemberExpression)trim.Object;
    }

    public static ConstantExpression ExtractTrimConstant(this Expression expression)
    {
        var trim = (MethodCallExpression)expression;
        return (ConstantExpression)trim.Object;
    }
}