using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.PopulationManager;

public class DefaultPopulationManager : IPopulationManager
{
    public IIndividualList Population { get; set; }

    public void InitPopulation(IInitialization initialization)
    {
        Population = initialization.Process(null);
    }

    public void CreateNewGeneration()
    {
        throw new NotImplementedException();
    }
}