using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public interface ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}