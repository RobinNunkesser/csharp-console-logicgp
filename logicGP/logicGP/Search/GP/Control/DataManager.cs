using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Util;
using Italbytz.Ports.Algorithms.AI.Search.GP.SearchSpace;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Control;

public class DataManager
{
    public required List<ILiteral<string>> Literals { get; set; }

    public required List<string> Labels { get; set; }

    public required string Label { get; set; }

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
                var literalType = uniqueValues.Count <= 3
                    ? LogicGpLiteralType.Dussault
                    : LogicGpLiteralType.Rudell;
                var literal = new LogicGpLiteral<string>(column.Name,
                    uniqueValues, i,
                    columnData, literalType);
                Literals.Add(literal);
            }
        }
    }

    public ILiteral<string> GetRandomLiteral()
    {
        var random = ThreadSafeRandomNetCore.LocalRandom;
        var index = random.Next(Literals.Count);
        return Literals[index];
    }
}