using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Crossover;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Mutation;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Italbytz.Adapters.Algorithms.AI.Search.GP.StoppingCriterion;
using Microsoft.ML;

namespace logicGP.Search.GP;

public interface IGeneticProgram
{
    public IMutation[] Mutations { get; set; }
    public ICrossover[] Crossovers { get; set; }
    public IDataView TrainingData { get; set; }
    public IIndividualList Population { get; }

    public IInitialization Initialization { get; set; }


    public IPopulationManager PopulationManager { get; set; }
    public ISearchSpace SearchSpace { get; set; }
    public ISelection SelectionForOperator { get; set; }

    public ISelection SelectionForSurvival { get; set; }

    public IStoppingCriterion[] StoppingCriteria { get; set; }
    int Generation { get; set; }
    IFitnessFunction FitnessFunction { get; set; }

    public void InitPopulation();
    public IIndividualList Run();
}