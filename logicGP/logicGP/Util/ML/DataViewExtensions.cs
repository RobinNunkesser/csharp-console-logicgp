using System.Collections.Immutable;
using System.Globalization;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public static class DataViewExtensions
{
    // Todo: Move to nuget

    public static ImmutableArray<ReadOnlyMemory<char>> GetFeaturesSlotNames(
        this IDataView dataView,
        string columnName = "Features"
    )
    {
        var featuresColumn = dataView.Schema.GetColumnOrNull(columnName);
        if (featuresColumn == null)
            throw new ArgumentException(
                "The data view does not contain a column named 'Features'.");
        var featuresAnnotations = featuresColumn?.Annotations;
        if (featuresAnnotations == null)
            throw new ArgumentException(
                "The 'Features' column does not contain annotations.");
        VBuffer<ReadOnlyMemory<char>> slotNames = default;
        featuresAnnotations.GetValue("SlotNames", ref slotNames);
        return [..slotNames.GetValues()];
    }

    public static IEnumerable<string>? GetOrderedUniqueColumnEntries(
        this IDataView dataView,
        string columnName
    )
    {
        var labelColumn = dataView.Schema[columnName];
        var labelColumnData =
            (GetColumnAsString(dataView, labelColumn) ??
             throw new InvalidOperationException(
                 $"Column {columnName} can not be read as strings.")).ToList();
        return new HashSet<string>(
            labelColumnData).OrderBy(c => c);
    }

    public static IEnumerable<string>? GetColumnAsString(
        this IDataView dataView,
        string columnName
    )
    {
        var column = dataView.Schema[columnName];

        return GetColumnAsString(dataView, column);
    }

    public static IEnumerable<string>? GetColumnAsString(
        this IDataView dataView, DataViewSchema.Column column)
    {
        var dataColumn = column.Type.RawType switch
        {
            { } floatType when floatType == typeof(float) => dataView
                .GetColumn<float>(column)
                .Select(entry => entry.ToString(CultureInfo.InvariantCulture)),
            { } intType when intType == typeof(int) => dataView
                .GetColumn<int>(column)
                .Select(entry => entry.ToString()),
            { } charType when charType == typeof(char) => dataView
                .GetColumn<char>(column)
                .Select(entry => entry.ToString()),
            { } stringType when stringType == typeof(string) => dataView
                .GetColumn<string>(column),
            _ => dataView
                .GetColumn<string>(column)
        };
        return dataColumn;
    }
}