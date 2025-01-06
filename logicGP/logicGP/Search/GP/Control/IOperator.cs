using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Control;

public interface IOperator
{
    public IIndividualList Process(IIndividualList individuals);
}