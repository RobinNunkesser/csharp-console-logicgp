namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public interface IIndividualList : IEnumerable<IIndividual>
{
    void AddIndividual(IIndividual individual);
    IIndividual GetRandomIndividual();
}