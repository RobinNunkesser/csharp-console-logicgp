using System.Collections.Immutable;
using System.Globalization;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Util;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using Italbytz.Ports.Algorithms.AI.Search.GP.SearchSpace;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Control;

public class DataManager
{
    public required List<ILiteral<string>> Literals { get; set; }

    public required List<string> Labels { get; set; }

    public required string Label { get; set; }

    public void Initialize(IDataView gpTrainingData,
        string labelColumnName = DefaultColumnNames.Label)
    {
        // Determine the labels from the label column
        Label = labelColumnName;
        var labelColumn = gpTrainingData.Schema[Label];
        var labelColumnData = DataViewExtensions
            .GetColumnAsString(gpTrainingData, labelColumn).ToList();
        Labels = new HashSet<string>(
            labelColumnData).OrderBy(c => c).ToList();

        // Construct the literals for the feature columns
        Literals = [];
        var featuresColumn = gpTrainingData.Schema
            .GetColumnOrNull("Features");
        if (featuresColumn == null)
            throw new ArgumentException(
                "The data view does not contain a column named 'Features'.");
        var featuresAnnotations = featuresColumn?.Annotations;
        if (featuresAnnotations == null)
            throw new ArgumentException(
                "The 'Features' column does not contain annotations.");
        VBuffer<ReadOnlyMemory<char>> slotNames = default;
        featuresAnnotations.GetValue("SlotNames", ref slotNames);
        var slotsNamesArray = slotNames.GetValues().ToImmutableArray();
        var values = gpTrainingData.GetColumn<float[]>("Features")
            .ToList();


        var feature = 0;

        foreach (var slotName in slotsNamesArray)
        {
            var columnData = GetFeatureColumnAsString(values, feature++);

            var uniqueValues =
                new HashSet<string>(
                    columnData);
            var uniqueCount = uniqueValues.Count;

            var powerSetCount = 1 << uniqueCount;
            for (var i = 1; i < powerSetCount - 1; i++)
            {
                var literalType = uniqueValues.Count <= 3
                    ? LogicGpLiteralType.Dussault
                    : LogicGpLiteralType.Rudell;
                var literal = new LogicGpLiteral<string>(slotName.ToString(),
                    uniqueValues, i,
                    columnData, literalType);
                Literals.Add(literal);
            }
        }
    }

    private List<string> GetFeatureColumnAsString(List<float[]> values, int i)
    {
        return values
            .Select(row => row[i].ToString(CultureInfo.InvariantCulture))
            .ToList();
    }

    public ILiteral<string> GetRandomLiteral()
    {
        var random = ThreadSafeRandomNetCore.LocalRandom;
        var index = random.Next(Literals.Count);
        return Literals[index];
    }
}