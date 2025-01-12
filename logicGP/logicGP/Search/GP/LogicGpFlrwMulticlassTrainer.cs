using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpFlrwMulticlassTrainer(
    LogicGpAlgorithm algorithm)
    : LogicGpTrainerBase<ITransformer>
{
    protected override ITransformer ConcreteFit(IDataView input, string label)
    {
        throw new NotImplementedException();
    }
}