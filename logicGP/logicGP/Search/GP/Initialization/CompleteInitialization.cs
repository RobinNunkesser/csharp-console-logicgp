using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using logicGP.Search.GP;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Initialization;

public class CompleteInitialization(IGeneticProgram gp) : IInitialization
{
    public int Size { get; set; }

    public IIndividualList Process(IIndividualList individuals)
    {
        var searchSpace = gp.SearchSpace;
        var population = searchSpace.GetAStartingPopulation();
        return population;
    }
}