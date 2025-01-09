using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Crossover;

public class LogicGpCrossover : ICrossover
{
    public IIndividualList Process(IIndividualList individuals)
    {
        var newPopulation = new Population();
        for (var i = 0; i < individuals.ToList().Count - 1; i += 2)
        {
            var parent = individuals[i];
            var offspring = (IIndividual)individuals[i + 1].Clone();
            var monomial = (IMonomial<float>)((LogicGpGenotype)parent.Genotype)
                .GetRandomMonomial().Clone();
            ((LogicGpGenotype)offspring.Genotype).InsertMonomial(monomial);
            newPopulation.Add(offspring);
        }

        return newPopulation;
    }
}