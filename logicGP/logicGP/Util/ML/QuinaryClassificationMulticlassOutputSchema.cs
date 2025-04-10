using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class
    QuinaryClassificationMulticlassOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(5)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}