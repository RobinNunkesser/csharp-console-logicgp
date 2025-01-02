using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public abstract class logicGPTrainerBase<TTransformer>

{
    public abstract TTransformer Fit (Microsoft.ML.IDataView input);
}