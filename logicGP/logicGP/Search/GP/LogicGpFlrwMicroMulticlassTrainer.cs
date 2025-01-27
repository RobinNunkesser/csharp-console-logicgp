using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Ports.Algorithms.AI.Search.GP.Individuals;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpFlrwMicroMulticlassTrainer(
    LogicGpAlgorithm algorithm,
    DataManager data)
    : LogicGpTrainerBase<ITransformer>(algorithm, data)
{
    protected override ITransformer CreateTransformer(
        IIndividual chosenIndividual,
        DataManager dataManager)
    {
        return new LogicGpFlrwTransformer(
            chosenIndividual, data);
    }

    protected override void ParameterizeAlgorithm(
        LogicGpAlgorithm logicGpAlgorithm)
    {
        logicGpAlgorithm.UseFullInitialization = true;
        logicGpAlgorithm.UsedWeighting = LogicGpAlgorithm.Weighting.Computed;
        logicGpAlgorithm.WeightMutationToUse =
            LogicGpAlgorithm.WeightMutation.None;
        logicGpAlgorithm.UsedAccuracy = LogicGpAlgorithm.Accuracies.Micro;
    }
}