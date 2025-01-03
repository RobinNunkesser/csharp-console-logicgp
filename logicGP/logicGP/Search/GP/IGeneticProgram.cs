using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.StoppingCriterion;
using Microsoft.ML;

namespace logicGP.Search.GP;

public interface IGeneticProgram
{
    public IDataView TrainingData { get; set; } 
    public IIndividualList Population { get; set; }
    public IInitialization Initialization { get; set; }
    public IFitnessFunction FitnessFunction { get; set; }
    public IPopulationManager PopulationManager { get; set; }
    public ISearchSpace SearchSpace { get; set; }
    
    public IStoppingCriterion[] StoppingCriteria { get; set; }
    
    public void InitPopulation();
    public IIndividual Run();

}