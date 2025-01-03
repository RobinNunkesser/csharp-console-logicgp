using System.Data;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpTransformer : ITransformer
{
    public void Save(ModelSaveContext ctx)
    {
        throw new NotImplementedException();
    }

    public DataViewSchema GetOutputSchema(DataViewSchema inputSchema)
    {
        throw new NotImplementedException();
    }

    public IDataView Transform(IDataView input)
    {
        var mlContext = new MLContext();
        var dummyData = new List<LogicGpModelOutput>();
        
        // Create a cursor to iterate through the rows
        using (var cursor = input.GetRowCursor(input.Schema))
        {
            // Get column indices
            var yIndex = cursor.Schema["y"];
            
            // Create delegates to access the values
            var yGetter = cursor.GetGetter<float>(yIndex);

            // Variables to hold the values
            float y = 0;

            // Iterate through the rows
            while (cursor.MoveNext())
            {
                yGetter(ref y);
                var random = new Random();
                var prediction =  0.4 + (random.NextDouble() * (1.0 - 0.4));
                var score = y > 0 ? 1-prediction : prediction;
                dummyData.Add(new LogicGpModelOutput { Y = (uint)y,Score = new float[] { (float)score,(float)(1-score) } });
            }
        }
        return mlContext.Data.LoadFromEnumerable(dummyData);
    }

    public IRowToRowMapper GetRowToRowMapper(DataViewSchema inputSchema)
    {
        throw new NotImplementedException();
    }

    public bool IsRowToRowMapper { get; }
}