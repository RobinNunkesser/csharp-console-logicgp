using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Mutation;

public class DeleteLiteral : IMutation
{
    public IIndividualList Process(IIndividualList individuals)
    {
        var newPopulation = new Population();
        foreach (var individual in individuals)
        {
            var mutant = (IIndividual)individual.Clone();
            ((LogicGpGenotype)mutant.Genotype).DeleteRandomLiteral();
            if (!((LogicGpGenotype)mutant.Genotype).IsEmpty())
                newPopulation.Add(mutant);
        }

        return newPopulation;
    }
}