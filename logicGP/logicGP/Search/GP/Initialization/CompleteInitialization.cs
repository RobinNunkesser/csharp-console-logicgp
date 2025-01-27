using Italbytz.Ports.Algorithms.AI.Search.GP;
using Italbytz.Ports.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Ports.Algorithms.AI.Search.GP.Initialization;

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