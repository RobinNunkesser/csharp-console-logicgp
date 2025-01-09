using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;

public class FinalModelSelection : ISelection
{
    public IIndividualList Process(IIndividualList individuals)
    {
        var allCandidates = individuals.ToList();
        var groups = allCandidates
            .GroupBy(i => ((LogicGpGenotype)i.Genotype).LiteralSignature())
            .Where(group => group.Count() > 1)
            .OrderByDescending(group => group.FirstOrDefault().Size);
        var largestSize = groups.FirstOrDefault()!.FirstOrDefault().Size;

        var bestModels = new List<IIndividual>[largestSize];
        foreach (var group in groups)
        {
            var groupSize = group.FirstOrDefault().Size;
            if (bestModels[groupSize - 1] == null)
            {
                bestModels[groupSize - 1] = group.ToList();
            }
            else
            {
                var accumulatedFitness = group.Average(element =>
                    element.LatestKnownFitness[0]
                );
                var bestFitness = bestModels[groupSize - 1].Average(element =>
                    element.LatestKnownFitness[0]
                );
                if (accumulatedFitness > bestFitness)
                    bestModels[groupSize - 1] = group.ToList();
            }
        }

        var chosenGroup = bestModels.FirstOrDefault(group => group != null);
        var bestAccuracy = 0.0;

        for (var i = largestSize - 1; i >= 0; i--)
        {
            if (bestModels[i] == null || bestModels[i].Count == 0) continue;
            var accumulatedFitness = bestModels[i].Average(element =>
                element.LatestKnownFitness[0]
            );
            if (accumulatedFitness < bestAccuracy) continue;
            var bestSmallerFitness = 0.0;
            for (var k = i - 1; k > 0; k--)
            {
                if (bestModels[k] == null || bestModels[k].Count == 0)
                    continue;
                if (bestModels[k].Average(element =>
                        element.LatestKnownFitness[0]
                    ) > bestSmallerFitness)
                    bestSmallerFitness = bestModels[k].Average(element =>
                        element.LatestKnownFitness[0]
                    );
            }

            var gain = accumulatedFitness / bestSmallerFitness;
            if (gain < 1.01) continue;
            chosenGroup = bestModels[i];
            bestAccuracy = accumulatedFitness;
        }

        return new Population { chosenGroup.First() };
    }

    public int Size { get; set; }
}