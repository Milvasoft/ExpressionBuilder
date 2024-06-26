﻿using ExpressionBuilder.Common;
using ExpressionBuilder.Generics;
using ExpressionBuilder.Operations;
using ExpressionBuilder.Test.Models;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ExpressionBuilder.Test.Unit;

[TestFixture]
public class FilterXmlSerializerTests
{
    private Filter<Person> _filter;
    private string _filterXml;

    [OneTimeSetUp]
    public void Setup()
    {
        _filter = new Filter<Person>();
        _filter.By("Id", Operation.GreaterThanOrEqualTo, 2).Or.By("Gender", Operation.EqualTo, PersonGender.Male);

        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
        sb.Append("<FilterOfPerson Type=\"ExpressionBuilder.Test.Models.Person, ExpressionBuilder.Test.NetCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\">");
        sb.Append("  <Statements>");
        sb.Append("  <StatementsGroup>");
        sb.Append("    <FilterStatementOfInt32 Type=\"System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">");
        sb.Append("      <PropertyId>Id</PropertyId>");
        sb.Append("      <Operation>GreaterThanOrEqualTo</Operation>");
        sb.Append("      <Value>2</Value>");
        sb.Append("      <Connector>1</Connector>");
        sb.Append("    </FilterStatementOfInt32>");
        sb.Append("    <FilterStatementOfPersonGender Type=\"ExpressionBuilder.Test.Models.PersonGender, ExpressionBuilder.Test.NetCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\">");
        sb.Append("      <PropertyId>Gender</PropertyId>");
        sb.Append("      <Operation>EqualTo</Operation>");
        sb.Append("      <Value>Male</Value>");
        sb.Append("      <Connector>0</Connector>");
        sb.Append("    </FilterStatementOfPersonGender>");
        sb.Append("  </StatementsGroup>");
        sb.Append("  </Statements>");
        sb.Append("</FilterOfPerson>");
        _filterXml = sb.ToString();
    }

    [TestCase(TestName = "Serialize FilterStatement with numeric value")]
    public void SerializeFilterStatementWithNumericValue()
    {
        var serializer = new XmlSerializer(typeof(FilterStatement<int>));
        StringBuilder sb;
        using (var writer = new StringWriter())
        {
            var statement = new FilterStatement<int>
            (
                "Id",
                Operation.GreaterThanOrEqualTo,
                2,
                0,
                Connector.Or
            );
            serializer.Serialize(writer, statement);
            writer.Flush();
            sb = writer.GetStringBuilder();
        }

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(sb.ToString());

        //Checking the statements list
        var root = xmlDoc.DocumentElement;
        Assert.That(root.Attributes["Type"].Value, Does.StartWith("System.Int32"));
        Assert.That(root.SelectSingleNode("PropertyId").InnerText, Is.EqualTo("Id"));
        Assert.That(root.SelectSingleNode("Operation").InnerText, Is.EqualTo("GreaterThanOrEqualTo"));
        Assert.That(root.SelectSingleNode("Value").InnerText, Is.EqualTo("2"));
        Assert.That(root.SelectSingleNode("Connector").InnerText, Is.EqualTo("1"));
    }

    [TestCase(TestName = "Serialize FilterStatement with enum value")]
    public void SerializeFilterStatementWithEnumValue()
    {
        var serializer = new XmlSerializer(typeof(FilterStatement<PersonGender>));
        StringBuilder sb;
        using (var writer = new StringWriter())
        {
            var statement = new FilterStatement<PersonGender>
            (
                "Gender",
                Operation.EqualTo,
                PersonGender.Male,
                default,
                Connector.And
            );
            serializer.Serialize(writer, statement);
            writer.Flush();
            sb = writer.GetStringBuilder();
        }

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(sb.ToString());

        //Checking the statements list
        var root = xmlDoc.DocumentElement;
        Assert.That(root.Attributes["Type"].Value, Does.StartWith("ExpressionBuilder.Test.Models.PersonGender"));
        Assert.That(root.SelectSingleNode("PropertyId").InnerText, Is.EqualTo("Gender"));
        Assert.That(root.SelectSingleNode("Operation").InnerText, Is.EqualTo("EqualTo"));
        Assert.That(root.SelectSingleNode("Value").InnerText, Is.EqualTo("Male"));
        Assert.That(root.SelectSingleNode("Connector").InnerText, Is.EqualTo("0"));
    }

    [TestCase(TestName = "Serialize FilterStatement with DateTime value")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S6580:Use a format provider when parsing date and time", Justification = "<Pending>")]
    public void SerializeFilterStatementWithDateTimeValue()
    {
        var serializer = new XmlSerializer(typeof(FilterStatement<DateTime>));
        StringBuilder sb;
        using (var writer = new StringWriter())
        {
            var statement = new FilterStatement<DateTime>
            (
                "Birth.Date",
                Operation.GreaterThan,
                new DateTime(1980, 1, 1),
                default,
                Connector.And
            );
            serializer.Serialize(writer, statement);
            writer.Flush();
            sb = writer.GetStringBuilder();
        }

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(sb.ToString());

        //Checking the statements list
        var root = xmlDoc.DocumentElement;
        Assert.That(root.Attributes["Type"].Value, Does.StartWith("System.DateTime"));
        Assert.That(root.SelectSingleNode("PropertyId").InnerText, Is.EqualTo("Birth.Date"));
        Assert.That(root.SelectSingleNode("Operation").InnerText, Is.EqualTo("GreaterThan"));
        var value = DateTime.Parse(root.SelectSingleNode("Value").InnerText);
        Assert.That(value, Is.EqualTo(new DateTime(1980, 1, 1)).Within(0).Days);
        Assert.That(root.SelectSingleNode("Connector").InnerText, Is.EqualTo("0"));
    }

    [TestCase(TestName = "Serialize Filter into XML")]
    public void SerializeFilterIntoXml()
    {
        var serializer = new XmlSerializer(typeof(Filter<Person>));
        StringBuilder sb;
        using (var writer = new StringWriter())
        {
            serializer.Serialize(writer, _filter);
            writer.Flush();
            sb = writer.GetStringBuilder();
        }

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(sb.ToString());

        //Checking the filter XML element
        var root = xmlDoc.DocumentElement;
        Assert.That(root.Name, Is.EqualTo("FilterOfPerson"));
        Assert.That(root.GetAttribute("Type"), Does.StartWith("ExpressionBuilder.Test.Models.Person"));
        Assert.That(root.ChildNodes.Count, Is.EqualTo(1));
        Assert.That(root.FirstChild.Name, Is.EqualTo("Statements"));
        Assert.That(root.FirstChild.ChildNodes.Count, Is.EqualTo(1));
        Assert.That(root.FirstChild.FirstChild.ChildNodes.Count, Is.EqualTo(2));
    }

    [TestCase(TestName = "Deserialize XML into Filter object")]
    public void DeserializeXmlIntoFilterObject()
    {
        Filter<Person> filter = null;
        var serializer = new XmlSerializer(typeof(Filter<Person>));
        using (var reader = new StringReader(_filterXml))
        {
            filter = serializer.Deserialize(reader) as Filter<Person>;
        }

        Assert.That(filter, Is.Not.Null);
        Assert.That(filter.Statements.Count(), Is.EqualTo(1));
        Assert.That(filter.Statements.SelectMany(s => s).Count(), Is.EqualTo(2));
    }

    [TestCase(TestName = "Deserialize XML into FilterStatement object with numeric value")]
    public void DeserializeXmlIntoFilterStatementObjectWithNumericValue()
    {
        var sb = new StringBuilder();
        sb.Append("    <FilterStatementOfInt32 Type=\"System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">");
        sb.Append("      <PropertyId>Id</PropertyId>");
        sb.Append("      <Operation>GreaterThanOrEqualTo</Operation>");
        sb.Append("      <Value>2</Value>");
        sb.Append("      <Connector>1</Connector>");
        sb.Append("    </FilterStatementOfInt32>");

        FilterStatement<int> statement = null;
        var serializer = new XmlSerializer(typeof(FilterStatement<int>));
        using (var reader = new StringReader(sb.ToString()))
        {
            statement = serializer.Deserialize(reader) as FilterStatement<int>;
        }

        Assert.That(statement, Is.Not.Null);
        Assert.That(statement.PropertyId, Is.EqualTo("Id"));
        Assert.That(statement.Operation, Is.EqualTo(Operation.GreaterThanOrEqualTo));
        Assert.That(statement.Value, Is.EqualTo(2));
        Assert.That(statement.Connector, Is.EqualTo(Connector.Or));
    }

    [TestCase(TestName = "Deserialize XML into FilterStatement object with enum value")]
    public void DeserializeXmlIntoFilterStatementObjectWithEnumValue()
    {
        var sb = new StringBuilder();
        sb.Append("    <FilterStatementOfPersonGender Type=\"ExpressionBuilder.Test.Models.PersonGender, ExpressionBuilder.Test, Version=1.0.6330.24179, Culture=neutral, PublicKeyToken=null\">");
        sb.Append("      <PropertyId>Gender</PropertyId>");
        sb.Append("      <Operation>EqualTo</Operation>");
        sb.Append("      <Value>Male</Value>");
        sb.Append("      <Connector>0</Connector>");
        sb.Append("    </FilterStatementOfPersonGender>");

        FilterStatement<PersonGender> statement = null;
        var serializer = new XmlSerializer(typeof(FilterStatement<PersonGender>));
        using (var reader = new StringReader(sb.ToString()))
        {
            statement = serializer.Deserialize(reader) as FilterStatement<PersonGender>;
        }

        Assert.That(statement, Is.Not.Null);
        Assert.That(statement.PropertyId, Is.EqualTo("Gender"));
        Assert.That(statement.Operation, Is.EqualTo(Operation.EqualTo));
        Assert.That(statement.Value, Is.EqualTo(PersonGender.Male));
        Assert.That(statement.Connector, Is.EqualTo(Connector.And));
    }

    [Test]
    public void GetSchemaMethodOfFilterClassShouldReturnNull()
    {
        var filter = new Filter<Person>();
        Assert.That(filter.GetSchema(), Is.Null);
    }

    [Test]
    public void GetSchemaMethodOfFilterStatementClassShouldReturnNull()
    {
        var statement = new FilterStatement<PersonGender>
        (
            "Gender",
            Operation.EqualTo,
            PersonGender.Male,
            default,
            Connector.And
        );
        Assert.That(statement.GetSchema(), Is.Null);
    }
}