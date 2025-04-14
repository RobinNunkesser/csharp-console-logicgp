using System.Collections.Immutable;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public static class DataViewExtensions
{
    // Todo: Move to nuget

    public static DataTable? ToDataTable(this IDataView? dataView)
    {
        DataTable? dt = null;
        if (dataView == null) return dt;
        dt = new DataTable();
        var preview = dataView.Preview();
        dt.Columns.AddRange(preview.Schema
            .Select(x => new DataColumn(x.Name)).ToArray());
        foreach (var row in preview.RowView)
        {
            var r = dt.NewRow();
            foreach (var col in row.Values) r[col.Key] = col.Value;
            dt.Rows.Add(r);
        }

        return dt;
    }

    public static void SaveAsCsv(
        this IDataView dataView,
        string filePath
    )
    {
        using var dataStream = new FileStream(
            filePath,
            FileMode.Create, FileAccess.Write);
        new MLContext().Data.SaveAsText(dataView, dataStream, ',',
            schema: false);
    }

    public static void WriteToCsv(
        this IDataView dataView,
        string filePath
    )
    {
        var dt = dataView.ToDataTable();

        var sb = new StringBuilder();

        Debug.Assert(dt != null, nameof(dt) + " != null");
        var columnNames = dt.Columns.Cast<DataColumn>()
            .Select(column => column.ColumnName);
        sb.AppendLine(string.Join(",", columnNames));

        foreach (DataRow row in dt.Rows)
        {
            IEnumerable<string> fields =
                row.ItemArray.Select(field => field.ToString());
            sb.AppendLine(string.Join(",", fields));
        }

        File.WriteAllText(filePath, sb.ToString());
    }

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
            { } uintType when uintType == typeof(uint) => dataView
                .GetColumn<uint>(column)
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