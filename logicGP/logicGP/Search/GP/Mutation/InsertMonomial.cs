using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Mutation;

public class InsertMonomial : IMutation
{
    public IIndividualList Process(IIndividualList individuals)
    {
        var mutant = (IIndividual)individuals[0].Clone();
        ((LogicGpGenotype)mutant.Genotype).InsertRandomMonomial();
        return new Population { mutant };
    }
}