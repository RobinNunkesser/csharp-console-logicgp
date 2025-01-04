using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

public class Population : IIndividualList
{
    private readonly List<IIndividual> _individuals = [];

    public void AddIndividual(IIndividual individual)
    {
        _individuals.Add(individual);
    }

    public IIndividual GetRandomIndividual()
    {
        return _individuals[new Random().Next(_individuals.Count)];
    }

    public override string ToString()
    {
        return string.Join("\n", _individuals);
    }
}