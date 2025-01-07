using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Crossover;

public class LogicGpCrossover : ICrossover
{
    public IIndividualList Process(IIndividualList individuals)
    {
        var parent = individuals[0];
        var offspring = (IIndividual)individuals[1].Clone();
        var monomial = (IMonomial<float>)((LogicGpGenotype)parent.Genotype)
            .GetRandomMonomial().Clone();
        ((LogicGpGenotype)offspring.Genotype).InsertMonomial(monomial);
        return new Population { offspring };
    }
}