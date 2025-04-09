namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class MulticlassClassificationInputSchema : ICustomMappingInputSchema
{
    public float[] Features { get; set; }
    //[KeyType(2)] public uint Label { get; set; }
}