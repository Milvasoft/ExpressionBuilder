using ExpressionBuilder.Resources;
using ExpressionBuilder.Test.NetCore.Models;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionBuilder.Test.NetCore.Integration;

[TestFixture]
public class PropertyLoaderTest
{
    private readonly List<string> _propertyIds =
    [
        "Id", "Name", "Gender", "Salary","SalaryDate", "Birth.Date", "Birth.DateOffset", "Birth.Age", "Birth.Country", "Contacts[Type]", "Contacts[Value]", "Contacts[Comments]", "Employer.Name", "Employer.Industry", "EmployeeReferenceNumber"
    ];

    [TestCase(TestName = "Checking the loading of classes' properties and fields")]
    public void LoadingPropertiesAndFields()
    {
        PropertyCollection properties = new(typeof(Person));
        var id = properties.ToList().Single(p => p.Id == "Id");
        var name = properties.ToList().Single(p => p.Id == "Name");
        id.MemberType.Should().Be(typeof(int));
        name.MemberType.Should().Be(typeof(string));
    }

    [TestCase(TestName = "Checking if all properties and fields were loaded")]
    public void LoadingAllPropertiesAndFields()
    {
        var properties = new PropertyCollection(typeof(Person)).ToList();
        properties.Count.Should().Be(_propertyIds.Count);
        properties.Select(p => p.Id).Should().BeEquivalentTo(_propertyIds);
    }
}