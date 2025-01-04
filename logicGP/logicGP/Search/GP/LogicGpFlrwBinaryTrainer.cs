using Italbytz.Adapters.Algorithms.AI.Search.GP.Initialization;
using Italbytz.Adapters.Algorithms.AI.Search.GP.PopulationManager;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using logicGP.Search.GP;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpFlrwBinaryTrainer(
    IGeneticProgram gp,
    RandomInitialization randomInitialization,
    DefaultPopulationManager populationManager,
    LogicGpSearchSpace searchSpace)
    : LogicGpTrainerBase<ITransformer>
{
    public override ITransformer Fit(IDataView input)
    {
        randomInitialization.Size = 2;
        gp.PopulationManager = populationManager;
        gp.TrainingData = input;
        gp.Initialization = randomInitialization;
        gp.SearchSpace = searchSpace;
        var individuals = gp.Run();
        var individualsString = individuals.ToString();
        return new LogicGpTransformer();
    }
}