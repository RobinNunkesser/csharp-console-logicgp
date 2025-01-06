using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;

public class UniformSelection : ISelection
{
    public int Size { get; set; }

    public IIndividualList[] Process(IIndividualList[] individuals)
    {
        var result = new IIndividualList[1];
        result[0] = new Population();
        var population = individuals[0];
        for (var i = 0; i < Size; i++)
            result[0].AddIndividual(population.GetRandomIndividual());
        return result;
    }
}