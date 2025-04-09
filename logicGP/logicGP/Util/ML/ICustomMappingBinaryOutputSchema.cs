namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public interface ICustomMappingBinaryOutputSchema
{
    public uint PredictedLabel { get; set; }

    public float Score { get; set; }
    public float Probability { get; set; }
}