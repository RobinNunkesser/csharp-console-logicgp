using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Initialization;
using Italbytz.Adapters.Algorithms.AI.Search.GP.PopulationManager;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.StoppingCriterion;
using logicGP.Search.GP;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpAlgorithm(
    IGeneticProgram gp,
    RandomInitialization randomInitialization,
    DefaultPopulationManager populationManager,
    LogicGpSearchSpace searchSpace,
    GenerationStoppingCriterion generationStoppingCriterion)
{
    public IIndividualList Fit(IDataView input)
    {
        randomInitialization.Size = 2;
        generationStoppingCriterion.Limit = 10000;
        gp.PopulationManager = populationManager;
        gp.TrainingData = input;
        gp.Initialization = randomInitialization;
        gp.SearchSpace = searchSpace;
        gp.StoppingCriteria = new IStoppingCriterion[]
            { generationStoppingCriterion };
        return gp.Run();
    }
}