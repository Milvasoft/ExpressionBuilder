using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using System.Linq.Expressions;

namespace ExpressionBuilder.Test.CustomOperations;

public class EqualTo : IOperation
{
    public string Name => "EqualTo";

    public TypeGroup TypeGroup => TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text;

    public int NumberOfValues => 1;

    public bool Active { get; set; }

    public bool SupportsLists => false;

    public bool ExpectNullValues => false;

    public EqualTo()
    {
        Active = true;
    }

    public Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2) => Expression.Equal(member, constant1);

    public override string ToString() => Name;
}