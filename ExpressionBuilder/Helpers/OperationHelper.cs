using ExpressionBuilder.Common;
using ExpressionBuilder.Exceptions;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Operations;
using System.Reflection;

namespace ExpressionBuilder.Helpers;

/// <summary>
/// Useful methods regarding <seealso cref="IOperation"></seealso>.
/// </summary>
public class OperationHelper : IOperationHelper
{
    private static HashSet<IOperation> _operations;

    public static Dictionary<TypeGroup, HashSet<Type>> TypeGroups { get; } = new Dictionary<TypeGroup, HashSet<Type>>
    {
        { TypeGroup.Text, new HashSet<Type> { typeof(string), typeof(char) } },
        { TypeGroup.Number, new HashSet<Type> { typeof(int), typeof(uint), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
        { TypeGroup.Boolean, new HashSet<Type> { typeof(bool) } },
        { TypeGroup.Date, new HashSet<Type> { typeof(DateTime), typeof(DateTimeOffset) } },
        { TypeGroup.Nullable, new HashSet<Type> { typeof(Nullable<>), typeof(string) } }
    };

    /// <summary>
    /// List of all operations loaded so far.
    /// </summary>
    public HashSet<IOperation> Operations => _operations;

    static OperationHelper()
    {
        LoadDefaultOperations();
    }

    /// <summary>
    /// Loads the default operations overwriting any previous changes to the <see cref="Operations"></see> list.
    /// </summary>
    public static void LoadDefaultOperations()
    {
        var operationInterface = typeof(IOperation);

        var operationsFound = Assembly.GetAssembly(typeof(DoesNotContain))
                                      .GetTypes()
                                      .Where(a => a.Namespace == "ExpressionBuilder.Operations"
                                                  && operationInterface.IsAssignableFrom(a)
                                                  && a.IsClass
                                                  && !a.IsAbstract)
                                      .Select(t => Activator.CreateInstance(t) as IOperation);

        _operations = new HashSet<IOperation>(operationsFound!, new OperationEqualityComparer());
    }

    /// <summary>
    /// Retrieves a list of <see cref="IOperation"></see> supported by a type.
    /// </summary>
    /// <param name="type">Type for which supported operations should be retrieved.</param>
    /// <returns></returns>
    public HashSet<IOperation> SupportedOperations(Type type)
    {
        var underlyingNullableType = Nullable.GetUnderlyingType(type);
        var typeName = (underlyingNullableType ?? type).Name;

        var supportedOperations = new List<IOperation>();

        if (type.IsArray)
        {
            typeName = type.GetElementType()?.Name;
            supportedOperations.AddRange(_operations.Where(o => o.SupportsLists && o.Active));
        }

        var typeGroup = TypeGroup.Default;

        if (TypeGroups.Any(i => i.Value.Any(v => v.Name == typeName)))
            typeGroup = TypeGroups.FirstOrDefault(i => i.Value.Any(v => v.Name == typeName)).Key;

        supportedOperations.AddRange(_operations.Where(o => o.TypeGroup.HasFlag(typeGroup) && !o.SupportsLists && o.Active));

        if (underlyingNullableType != null)
            supportedOperations.AddRange(_operations.Where(o => o.TypeGroup.HasFlag(TypeGroup.Nullable) && !o.SupportsLists && o.Active));

        return new HashSet<IOperation>(supportedOperations);
    }

    /// <summary>
    /// Instantiates an IOperation given its name.
    /// </summary>
    /// <param name="operationName">Name of the operation to be instantiated.</param>
    /// <returns></returns>
    public IOperation GetOperationByName(string operationName)
    {
        var operation = _operations.SingleOrDefault(o => o.Name == operationName && o.Active);

        return operation ?? throw new OperationNotFoundException(operationName);
    }

    /// <summary>
    /// Loads a list of custom operations into the <see cref="Operations"></see> list.
    /// </summary>
    /// <param name="operations">List of operations to load.</param>
    public void LoadOperations(List<IOperation> operations) => LoadOperations(operations, false);

    /// <summary>
    /// Loads a list of custom operations into the <see cref="Operations"></see> list.
    /// </summary>
    /// <param name="operations">List of operations to load.</param>
    /// <param name="overloadExisting">Specifies that any matching pre-existing operations should be replaced by the ones from the list. (Useful to overwrite the default operations)</param>
    public void LoadOperations(List<IOperation> operations, bool overloadExisting)
    {
        foreach (var operation in operations)
        {
            DeactivateOperation(operation.Name, overloadExisting);
            _operations.Add(operation);
        }
    }

    private static void DeactivateOperation(string operationName, bool overloadExisting)
    {
        if (!overloadExisting)
            return;

        var op = _operations.FirstOrDefault(o => string.Compare(o.Name, operationName, StringComparison.InvariantCultureIgnoreCase) == 0);

        if (op != null)
            op.Active = false;
    }
}