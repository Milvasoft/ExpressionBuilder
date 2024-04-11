using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations;

/// <summary>
/// Base class for operations.
/// </summary>
/// <remarks>
/// Instantiates a new operation.
/// </remarks>
/// <param name="name">Operations name.</param>
/// <param name="numberOfValues">Number of values supported by the operation.</param>
/// <param name="typeGroups">TypeGroup(s) which the operation supports.</param>
/// <param name="active">Determines if the operation is active.</param>
/// <param name="supportsLists">Determines if the operation supports arrays.</param>
/// <param name="expectNullValues"></param>
public abstract class OperationBase(string name, int numberOfValues, TypeGroup typeGroups, bool active = true, bool supportsLists = false, bool expectNullValues = false) : IOperation, IEquatable<IOperation>
{
    /// <inheritdoc />
    public string Name { get; } = name;

    /// <inheritdoc />
    public TypeGroup TypeGroup { get; } = typeGroups;

    /// <inheritdoc />
    public int NumberOfValues { get; } = numberOfValues;

    /// <inheritdoc />
    public bool Active { get; set; } = active;

    /// <inheritdoc />
    public bool SupportsLists { get; } = supportsLists;

    /// <inheritdoc />
    public bool ExpectNullValues { get; } = expectNullValues;

    /// <inheritdoc />
    public abstract Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2);

    /// <inheritdoc />
    public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;

    /// <inheritdoc />
    public override string ToString() => Name.Trim();

    public bool Equals(IOperation other) => string.Equals(Name, other.Name);

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((OperationBase)obj);
    }
}