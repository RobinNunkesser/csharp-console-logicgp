namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class BinaryClassificationInputSchema : ICustomMappingInputSchema
{
    public float[] Features { get; set; }
    //[KeyType(2)] public uint Label { get; set; }
}