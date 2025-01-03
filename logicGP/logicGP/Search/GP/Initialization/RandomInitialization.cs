using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using logicGP.Search.GP;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Initialization;

public class RandomInitialization(IGeneticProgram gp) : IInitialization
{
    public int Size { get; set; }

    public IIndividualList[] Process(IIndividualList[] individuals)
    {
        var result = new IIndividualList[1];
        result[0] = new Population();
        var searchSpace = gp.SearchSpace;
        for (var i = 0; i < Size; i++)
            result[0]
                .AddIndividual(new Individual(searchSpace.GetRandomGenotype(),
                    null));
        return result;
    }
}