﻿using ExpressionBuilder.Test.Models;
using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Test.Unit.Helpers;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "<Pending>")]
public class TestData
{
    public List<Person> People { get; private set; }

    public List<Company> Companies { get; private set; }

    public TestData()
    {
        var owner = new Person { Name = "John Smith", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1965, 8, 28), Country = "USA" } };

        var company = new Company { Name = "Back to the future", Industry = "Time Traveling Agency", Owner = owner };

        var manager = new Person { Name = "Bob Storm", Birth = new Person.BirthData { Date = new DateTime(1970, 11, 9), Country = "USA" }, Employer = company };
        var manager2 = new Person { Name = "Ben Clark", Birth = new Person.BirthData { Date = new DateTime(1972, 3, 12), Country = "AUS" } };
        company.Managers = [manager, manager2];

        Companies =
        [
            company,
            new Company { Name = "Acme Inc." },
            new Company { Name = "Underground Ltd.", Managers = [manager2]},
            new Company { Name = "Beyond Co.", Managers = [manager]},
        ];

        People =
        [
            new Person { Name = "John Doe", Gender = PersonGender.Male, Salary=4565, Birth = new Person.BirthData { Date = new DateTime(1979, 2, 28) }, Employer = company, Manager = manager2, EmployeeReferenceNumber = 123 },
            new Person { Name = "Jane Doe", Gender = PersonGender.Female, Salary=4973, Birth = new Person.BirthData { Date = new DateTime(1985, 9, 5), Country = " " } },
            new Person { Name = "Wade Wilson", Gender = PersonGender.Male, Salary=3579, Birth = new Person.BirthData { Date = new DateTime(1973, 10, 9), Country = "USA" }, Manager = manager, EmployeeReferenceNumber = 432 },
            new Person { Name = "Jessica Jones", Gender = PersonGender.Female, Salary=5000, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "usa" }, Manager = manager, EmployeeReferenceNumber = null },
            new Person { Name = "Jane Jones", Gender = PersonGender.Female, Salary=3500, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "AUS" } },
            new Person { Name = "Fulano Silva", Gender = PersonGender.Male, Salary=3322, Birth = new Person.BirthData { Date = new DateTime(1983, 5, 10), Country = "BRA" }, Employer = company, Manager = manager2, EmployeeReferenceNumber = 986543434323 },
            new Person { Name = "John Hancock", Gender = PersonGender.Male, Employer = company },
            new Person { Name = "Jack Luffy UTC 30-21", SalaryDate = new DateTime(2024, 10, 30, 21, 10, 10, DateTimeKind.Utc), Gender = PersonGender.Male, Employer = company },
            new Person { Name = "Jack Luffy UTC Offset 30-21", SalaryDateOffset = new DateTimeOffset(2024, 10, 30, 21, 10, 10, TimeSpan.Zero), Gender = PersonGender.Male, Employer = company },
            new Person { Name = "Jack Luffy Local 28-21", SalaryDate = new DateTime(2024, 10, 28, 21, 10, 10, DateTimeKind.Local), Gender = PersonGender.Male, Employer = company },
            new Person { Name = "Jack Luffy Local Offset 28-21", SalaryDateOffset = new DateTimeOffset(2024, 10, 28, 21, 10, 10, new TimeSpan(3,0,0)), Gender = PersonGender.Male, Employer = company },
        ];
        var id = 1;
        foreach (var person in People)
        {
            person.Id = id++;

            if (id <= 5)
            {
                var email = person.Name.ToLower().Replace(" ", ".") + "@email.com";
                person.Contacts.Add(new Contact { Type = ContactType.Email, Value = email, Comments = person.Name + "'s email" });
            }
        }
    }
}