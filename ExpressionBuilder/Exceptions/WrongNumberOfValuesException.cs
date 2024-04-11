using ExpressionBuilder.Interfaces;

namespace ExpressionBuilder.Exceptions;

/// <summary>
/// Represents an attempt to use an operation providing the wrong number of values.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WrongNumberOfValuesException" /> class.
/// </remarks>
/// <param name="operation">Operation used.</param>
[Serializable]
public class WrongNumberOfValuesException(IOperation operation) : Exception
{
    /// <summary>
    /// Gets the <see cref="Operation" /> attempted to be used.
    /// </summary>
    public IOperation Operation { get; private set; } = operation;

    /// <summary>
    /// Gets a message that describes the current exception.
    /// </summary>
    public override string Message => $"The operation '{Operation.Name}' admits exactly '{Operation.NumberOfValues}' values (not more neither less than this).";
}