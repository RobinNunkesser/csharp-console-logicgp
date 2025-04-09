using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class MulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(3)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}