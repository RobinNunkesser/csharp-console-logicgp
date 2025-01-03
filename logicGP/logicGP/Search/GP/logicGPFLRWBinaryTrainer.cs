using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class logicGPFLRWBinaryTrainer : logicGPTrainerBase<ITransformer>
{
    public override ITransformer Fit(IDataView input)
    {
        return new logicGPTransformer();
    }
}