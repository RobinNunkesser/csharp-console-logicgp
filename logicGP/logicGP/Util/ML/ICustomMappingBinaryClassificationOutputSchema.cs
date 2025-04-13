namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public interface ICustomMappingBinaryClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    public float Score { get; set; }
    public float Probability { get; set; }
}