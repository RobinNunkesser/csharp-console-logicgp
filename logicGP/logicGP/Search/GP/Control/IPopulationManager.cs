using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Control;

public interface IPopulationManager
{
    public IIndividualList Population { get; set; }
    public void InitPopulation(IInitialization initialization);

    public void CreateNewGeneration();
}