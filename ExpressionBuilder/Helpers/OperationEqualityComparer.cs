using ExpressionBuilder.Interfaces;

namespace ExpressionBuilder.Helpers;

internal class OperationEqualityComparer : IEqualityComparer<IOperation>
{
    public bool Equals(IOperation x, IOperation y) => string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase) == 0
               && x.Active && y.Active;

    public int GetHashCode(IOperation obj) => obj.Name.GetHashCode() ^ obj.Active.GetHashCode();
}