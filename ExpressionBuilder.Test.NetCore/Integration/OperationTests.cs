using NUnit.Framework;
using System.Collections.Generic;

namespace ExpressionBuilder.Test.Integration;

[TestFixture]
public class OperationTests
{
    private readonly List<string> _operationsNames = new List<string> {
        "Ends with", "Equal to", "Greater than", "Greater than or equals", "Is empty", "Is not empty", "Is not null", "Is not null nor whitespace", "Is null", "Is null or whitespace", "Less than", "Less than or equals",
        "Not equal to", "Starts with", "Does not contain", "Between", "Contains", "In", "NotIn"
    };

    private readonly List<string> _operationsNamesptBR = new List<string> {
        "entre", "contem", "termina com", "igual", "maior que", "maior que ou igual", "em", "é vazio", "não é vazio", "não é nulo", "não é nulo nem vazio", "é nulo", "é nulo ou vazio", "menor que","menor que ou igual", "diferente",
        "começa com", "não contem", "não entre"
    };
}