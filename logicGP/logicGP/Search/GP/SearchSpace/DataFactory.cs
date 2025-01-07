using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class DataFactory
{
    private DataFactory()
    {
    }

    public static DataFactory Instance { get; } = new();

    public List<ILiteral<float>> Literals { get; set; }

    public List<float> Labels { get; set; }

    public void Initialize(IDataView gpTrainingData,
        string labelColumnName = DefaultColumnNames.Label)
    {
        Literals = [];


        var schema = gpTrainingData.Schema;
        foreach (var column in schema)
        {
            // TODO: Handle other types
            if (column.Type.RawType != typeof(float)) continue;
            var columnData = gpTrainingData.GetColumn<float>(column);
            var uniqueValues = new HashSet<float>(columnData);
            var uniqueCount = uniqueValues.Count;
            if (column.Name == labelColumnName)
            {
                Labels = uniqueValues.OrderBy(c => c).ToList();
                continue;
            }

            var powerSetCount = 1 << uniqueCount;
            for (var i = 1; i < powerSetCount - 1; i++)
            {
                var literal = new LogicGpLiteral<float>(column.Name,
                    uniqueValues, i,
                    columnData.ToList());
                Literals.Add(literal);
            }
        }
    }

    public ILiteral<float> GetRandomLiteral()
    {
        var random = new Random();
        var index = random.Next(Literals.Count);
        return Literals[index];
    }
}