using System.Data;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public static class DataViewHelper
{
    public static DataTable ToDataTable(this IDataView dataView)
    {
        DataTable dt = null;
        if (dataView != null)
        {
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
        }

        return dt;
    }
}