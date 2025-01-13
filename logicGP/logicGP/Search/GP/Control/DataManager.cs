using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Control;

public class DataManager
{
    public List<ILiteral<string>> Literals { get; set; }

    public List<string> Labels { get; set; }

    public string Label { get; set; }

    public void Initialize(IDataView gpTrainingData,
        string labelColumnName = DefaultColumnNames.Label)
    {
        Label = labelColumnName;
        Literals = [];


        var schema = gpTrainingData.Schema;
        foreach (var column in schema)
        {
            var columnData = gpTrainingData.GetColumnAsString(column).ToList();
            var uniqueValues =
                new HashSet<string>(
                    columnData);
            var uniqueCount = uniqueValues.Count;
            if (column.Name == labelColumnName)
            {
                Labels = uniqueValues.OrderBy(c => c).ToList();
                continue;
            }

            var powerSetCount = 1 << uniqueCount;
            for (var i = 1; i < powerSetCount - 1; i++)
            {
                var literal = new LogicGpLiteral<string>(column.Name,
                    uniqueValues, i,
                    columnData);
                Literals.Add(literal);
            }
        }
    }

    public ILiteral<string> GetRandomLiteral()
    {
        var random = new Random();
        var index = random.Next(Literals.Count);
        return Literals[index];
    }
}