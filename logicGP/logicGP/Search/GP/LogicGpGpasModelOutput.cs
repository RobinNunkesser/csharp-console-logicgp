using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpGpasModelOutput
{
    [LoadColumn(0)] [ColumnName("y")] public uint Y { get; set; }

    [LoadColumn(1)] [ColumnName("Score")] public float[] Score { get; set; }

    [LoadColumn(2)]
    [ColumnName(@"PredictedLabel")]
    public string PredictedLabel { get; set; }
}