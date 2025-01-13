using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpFlrwModelOutput
{
    [LoadColumn(0)] [ColumnName(@"y")] public string Y { get; set; }

    [LoadColumn(1)]
    [ColumnName(@"PredictedLabel")]
    public string PredictedLabel { get; set; }
}