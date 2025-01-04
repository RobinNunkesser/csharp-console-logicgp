using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

public class Population : IIndividualList
{
    private readonly List<IIndividual> _individuals = [];

    public void AddIndividual(IIndividual individual)
    {
        _individuals.Add(individual);
    }

    public override string ToString()
    {
        return string.Join("\n", _individuals);
    }
}