﻿using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Test.Models;

public enum PersonGender
{
    Male,
    Female
}

public class Person
{
    public int Id;
    public string Name { get; set; }
    public PersonGender Gender { get; set; }
    public BirthData Birth { get; set; }
    public List<Contact> Contacts { get; private set; }
    public Company Employer { get; set; }
    public Person Manager { get; set; }
    public double Salary { get; set; }
    public DateTime? SalaryDate { get; set; }
    public DateTimeOffset? SalaryDateOffset { get; set; }
    public long? EmployeeReferenceNumber { get; set; }

    public Person()
    {
        Contacts = [];
        Birth = new BirthData();
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = GetType().ToString().GetHashCode();
            hash = (hash * 16777619) ^ Name.GetHashCode();
            hash = (hash * 16777619) ^ Gender.GetHashCode();

            if (Birth.Date != null)
            {
                hash = (hash * 16777619) ^ Birth.Date.GetHashCode();
            }

            return hash;
        }
    }

    public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();

    public override string ToString() => Name;

    public class BirthData
    {
        public DateTime? Date { get; set; }
        public string Country { get; set; }

        public DateTimeOffset? DateOffset
        {
            get
            {
                return Date.HasValue ? new DateTimeOffset?(Date.Value) : new DateTimeOffset?();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "<Pending>")]
        public int Age
        {
            get
            {
                if (!Date.HasValue)
                {
                    return 0;
                }

                var timeSpan = DateTime.Now - Date.Value;
                return (int)(timeSpan.Days / 365.2425);
            }
        }

        public override string ToString() => string.Format("Born at {0} in {1} ({2} yo)", Date.Value.ToShortDateString(), Country, Age);
    }
}

public class Company
{
    public string Name { get; set; }
    public string Industry { get; set; }
    public Person Owner { get; set; }
    public List<Person> Managers { get; set; }

    public Company()
    {
        Managers = [];
    }
}