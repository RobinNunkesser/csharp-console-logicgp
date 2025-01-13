using System.Globalization;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Control;

public static class DataViewExtensions
{
    public static IEnumerable<string>? GetColumnAsString(
        this IDataView dataView,
        string columnName
    )
    {
        var column = dataView.Schema[columnName];

        return dataView.GetColumnAsString(column);
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