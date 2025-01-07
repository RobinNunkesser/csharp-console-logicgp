using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Mutation;

public class InsertLiteral : IMutation
{
    public IIndividualList Process(IIndividualList individuals)
    {
        var mutant = (IIndividual)individuals[0].Clone();
        ((LogicGpGenotype)mutant.Genotype).InsertRandomLiteral();
        return new Population { mutant };
    }
}