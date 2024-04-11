namespace ExpressionBuilder.Exceptions;

/// <summary>
/// Represents an attempt to instantiate an operation that was not loaded.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="OperationNotFoundException" /> class.
/// </remarks>
/// <param name="operationName">Name of the operation that was intended to be instantiated.</param>
[Serializable]
public class OperationNotFoundException(string operationName) : Exception
{
    /// <summary>
    /// Name of the operation that was intended to be instantiated.
    /// </summary>
    public string OperationName { get; } = operationName;

    /// <inheritdoc />
    public override string Message
    {
        get
        {
            return string.Format("Sorry, the operation '{0}' was not found.", OperationName);
        }
    }
}