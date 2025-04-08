using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class MulticlassClassificationOutputSchema
{
    public uint PredictedLabel { get; set; }

    //public VectorDataViewType Score { get; set; }

    [VectorType(3)] public VBuffer<float> Score { get; set; }

    public VBuffer<float> Probability { get; set; }

    /*
     *             var scoreType = score.Type as VectorDataViewType;
       if (scoreType == null || scoreType.Size < 2 || scoreType.ItemType != NumberDataViewType.Single)
           throw Host.ExceptSchemaMismatch(nameof(schema), "score", score.Name, "vector of two or more items of type Single", score.Type.ToString());

     */
}