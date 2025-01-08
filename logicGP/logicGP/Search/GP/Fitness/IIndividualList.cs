namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public interface IIndividualList : IEnumerable<IIndividual>
{
    IIndividual this[int index] { get; }
    void Add(IIndividual individual);
    IIndividual GetRandomIndividual();

    List<IIndividual> ToList();
}