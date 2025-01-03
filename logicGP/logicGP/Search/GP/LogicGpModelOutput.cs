using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpModelOutput
{
    [LoadColumn(0)]
    [ColumnName("y")]
    public uint Y { get; set; }

    [LoadColumn(1)]
    [ColumnName("Score")]
    public float[] Score { get; set; }
}