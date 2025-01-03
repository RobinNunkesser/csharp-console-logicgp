using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.PopulationManager;

public class DefaultPopulationManager : IPopulationManager
{
    private IIndividualList _population;

    public void InitPopulation(IInitialization initialization)
    {
        _population = initialization.Process(null)[0];
    }

    public void CreateNewGeneration()
    {
        throw new NotImplementedException();
    }

    public IIndividualList GetPopulation()
    {
        throw new NotImplementedException();
    }
}