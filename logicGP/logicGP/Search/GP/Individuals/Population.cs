using System.Collections;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

public class Population : IIndividualList
{
    private readonly List<IIndividual> _individuals = [];

    public IIndividual this[int index] => _individuals[index];

    public void Add(IIndividual individual)
    {
        _individuals.Add(individual);
    }

    public IIndividual GetRandomIndividual()
    {
        return _individuals[new Random().Next(_individuals.Count)];
    }

    public List<IIndividual> ToList()
    {
        return [.._individuals];
    }

    public IEnumerator<IIndividual> GetEnumerator()
    {
        return _individuals.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return string.Join("\n", _individuals);
    }
}