using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class
    TernaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(3)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    QuaternaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(4)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    QuinaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(5)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    SenaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(6)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    SeptenaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(7)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    OctonaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(8)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    NonaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(9)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    DenaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(10)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    UndenaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(11)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    DuodenaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(12)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    TridenaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(13)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    TetradenaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(14)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    PentadenaryClassificationOutputSchema :
    ICustomMappingMulticlassOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(15)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}