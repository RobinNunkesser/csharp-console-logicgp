using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;

public class ParetoFrontSelection : ISelection
{
    public IIndividualList Process(IIndividualList individuals)
    {
        var individualList = individuals.ToList();
        var maxGeneration =
            individualList.Max(individual => individual.Generation);
        var i = 0;
        while (i < individualList.Count)
        {
            var individual = individualList[i];
            if (individual.Generation < maxGeneration) break;
            var j = i + 1;
            while (j < individualList.Count)
            {
                var otherIndividual = individualList[j];
                if (individual.IsDominating(otherIndividual))
                {
                    individualList.RemoveAt(j);
                }
                else if (otherIndividual.IsDominating(individual))
                {
                    individualList.RemoveAt(i);
                    i--;
                    break;
                }
                else
                {
                    j++;
                }
            }

            i++;
        }

        var population = new Population();
        foreach (var individual in individualList) population.Add(individual);
        return population;
    }

    public int Size { get; set; }
}