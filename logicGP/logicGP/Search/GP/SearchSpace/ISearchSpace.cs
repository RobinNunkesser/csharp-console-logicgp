using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public interface ISearchSpace
{
    IGenotype GetRandomGenotype();
    IIndividualList GetAStartingPopulation();
}