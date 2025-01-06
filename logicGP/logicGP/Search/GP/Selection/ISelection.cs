using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;

public interface ISelection : IOperator
{
    public int Size { get; set; }
}