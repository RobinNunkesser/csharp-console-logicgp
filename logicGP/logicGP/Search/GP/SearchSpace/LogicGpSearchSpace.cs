using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using logicGP.Search.GP;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpSearchSpace(IGeneticProgram gp) : ISearchSpace
{
    public int Classes { get; set; } = 2;

    public IGenotype GetRandomGenotype()
    {
        return new LogicGpGenotype(Classes);
    }
}