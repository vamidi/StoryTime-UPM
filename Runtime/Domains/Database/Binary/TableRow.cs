using System;
using System.Collections.Generic;

namespace StoryTime.Domains.Database.Binary
{
    public class TableRow
    {
	    public string RowId;
        public Dictionary<TableRowInfo, TableField> Fields = new Dictionary<TableRowInfo, TableField>();

        public TableField Find(string columnName)
        {
            foreach (var field in Fields)
            {
                if (field.Key.Equals(columnName))
                    return field.Value;
            }

            return null;
        }
    }
}
