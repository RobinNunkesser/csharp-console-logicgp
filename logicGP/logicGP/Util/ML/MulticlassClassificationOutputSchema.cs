using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class
    TernaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(3)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    QuaternaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(4)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    QuinaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(5)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    SenaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(6)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    SeptenaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(7)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    OctonaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(8)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    NonaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(9)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    DenaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(10)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    UndenaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(11)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    DuodenaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(12)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    TridenaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(13)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    TetradenaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(14)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}

public class
    PentadenaryClassificationClassificationOutputSchema :
    ICustomMappingMulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    [VectorType(15)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }
}