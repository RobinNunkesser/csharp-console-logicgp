using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpGpasBinaryTrainer(
    LogicGpAlgorithm algorithm,
    DataManager data)
    : LogicGpTrainerBase<ITransformer>(algorithm, data)
{
    protected override ITransformer CreateTransformer(
        IIndividual chosenIndividual,
        DataManager dataManager)
    {
        return new LogicGpGpasTransformer(
            chosenIndividual, data);
    }

    protected override void ParameterizeAlgorithm(
        LogicGpAlgorithm logicGpAlgorithm)
    {
        logicGpAlgorithm.UseFullInitialization = false;
        logicGpAlgorithm.WeightMutationToUse =
            LogicGpAlgorithm.WeightMutation.None;
    }
}