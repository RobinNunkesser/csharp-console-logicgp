namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public interface IIndividualList : IEnumerable<IIndividual>
{
    IIndividual this[int index] { get; }
    void AddIndividual(IIndividual individual);
    IIndividual GetRandomIndividual();
}