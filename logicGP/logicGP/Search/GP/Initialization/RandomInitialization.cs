using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Ports.Algorithms.AI.Search.GP;
using Italbytz.Ports.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Ports.Algorithms.AI.Search.GP.Initialization;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Initialization;

public class RandomInitialization(IGeneticProgram gp) : IInitialization
{
    public int Size { get; set; }

    public IIndividualList Process(IIndividualList individuals)
    {
        var result = new Population();
        var searchSpace = gp.SearchSpace;
        for (var i = 0; i < Size; i++)
            result
                .Add(new Individual(searchSpace.GetRandomGenotype(),
                    null));
        return result;
    }
}