using System.Data;

namespace CAM_WEB1.Helpers
{
    public static class DataTableHelper
    {
        public static List<Dictionary<string, object>> ToList(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }

                list.Add(dict);
            }

            return list;
        }
    }
}
