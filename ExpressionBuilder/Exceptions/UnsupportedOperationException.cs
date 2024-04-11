using ExpressionBuilder.Interfaces;

namespace ExpressionBuilder.Exceptions;

/// <summary>
/// Represents an attempt to use an operation not currently supported by a type.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UnsupportedOperationException" /> class.
/// </remarks>
/// <param name="operation">Operation used.</param>
/// <param name="typeName">Name of the type.</param>
[Serializable]
public class UnsupportedOperationException(IOperation operation, string typeName) : Exception
{
    /// <summary>
    /// Gets the <see cref="Operation" /> attempted to be used.
    /// </summary>
    public IOperation Operation { get; private set; } = operation;

    /// <summary>
    /// Gets name of the type.
    /// </summary>
    public string TypeName { get; private set; } = typeName;

    /// <summary>
    /// Gets a message that describes the current exception.
    /// </summary>
    public override string Message => $"The type '{TypeName}' does not have support for the operation '{Operation}'.";
}