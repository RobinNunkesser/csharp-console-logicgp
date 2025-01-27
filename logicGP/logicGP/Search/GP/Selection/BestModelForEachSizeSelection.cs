using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Ports.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Ports.Algorithms.AI.Search.GP.Selection;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;

public class BestModelForEachSizeSelection : ISelection
{
    public IIndividualList Process(IIndividualList individuals)
    {
        var population = new Population();
        var individualList = individuals.ToList();

        var groupedIndividuals =
            individualList.GroupBy(individual => individual.Size);
        foreach (var group in groupedIndividuals)
        {
            var bestIndividual = group.First();
            var bestFitness = 0;
            foreach (var individual in group)
            {
                var fitness =
                    (individual.LatestKnownFitness ?? Array.Empty<double>())
                    .Aggregate(0, (current, fitval) => (int)(current + fitval));
                if (fitness <= bestFitness) continue;
                bestFitness = fitness;
                bestIndividual = individual;
            }

            population.Add(bestIndividual);
        }

        return population;
    }

    public int Size { get; set; }
}