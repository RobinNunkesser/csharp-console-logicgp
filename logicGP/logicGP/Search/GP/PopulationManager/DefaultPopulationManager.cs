using Italbytz.Ports.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Ports.Algorithms.AI.Search.GP.Initialization;
using Italbytz.Ports.Algorithms.AI.Search.GP.PopulationManager;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.PopulationManager;

public class DefaultPopulationManager : IPopulationManager
{
    public IIndividualList Population { get; set; }

    public void InitPopulation(IInitialization initialization)
    {
        Population = initialization.Process(null);
    }
}