using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class BinaryClassificationOutputSchema
{
    [KeyType(2)] public uint PredictedLabel { get; set; }

    public float Score { get; set; }
    public float Probability { get; set; }
}