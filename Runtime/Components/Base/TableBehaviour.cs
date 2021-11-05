using System;
using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
    using Database;
    using Binary;

    public class TableBehaviour : SerializableScriptableObject
    {
	    /// <summary>
	    /// ID of the row inside the table.
	    /// </summary>
	    public UInt32 ID
	    {
		    get => id;
		    set {
			    id = value;
			    OnTableIDChanged();
		    }
	    }

	    /// <summary>
	    /// Name of the table that we are using.
	    /// </summary>
	    public string Name { get => tableName; protected set => tableName = value; }
	    public string DropdownColumn { get => dropdownColumn; protected set => dropdownColumn = value; }
	    public UInt32 LinkedID { get => linkID; protected set => linkID = value; }
	    public string LinkedColumn { get => linkedColumn; protected set => linkedColumn = value; }
		public string LinkedTable { get => linkedTable; protected set => linkedTable = value; }

	    [SerializeField, HideInInspector] private UInt32 id = UInt32.MaxValue;
	    [SerializeField, HideInInspector] private string tableName = String.Empty;
	    [SerializeField, HideInInspector] private string dropdownColumn = String.Empty;
	    [SerializeField, HideInInspector] private UInt32 linkID = UInt32.MaxValue;
	    [SerializeField, HideInInspector] private string linkedColumn = String.Empty;
	    [SerializeField, HideInInspector] private string linkedTable = String.Empty;

	    public TableBehaviour(string name, string dropdownColumn, string linkedColumn = "",
			UInt32 linkedId = UInt32.MaxValue, string linkedTable = "")
	    {
		    Init(name, dropdownColumn, linkedColumn, linkedId, linkedTable);
	    }

	    public void Init(string name, string dropdownColumn, string linkedColumn, UInt32 linkedId, string linkedTable)
	    {
		    Name = name;
		    DropdownColumn = dropdownColumn;

		    LinkedColumn = linkedColumn;
		    LinkedID = linkedId;
		    LinkedTable = linkedTable;
	    }

	    public virtual void Reset() { }

	    public TableField GetField(string tableName, string columnName, uint id)
	    {
		    return TableDatabase.Get.GetField(tableName, columnName, id);
	    }

	    public TableRow GetRow(string tableName, uint id)
	    {
		    return TableDatabase.Get.GetRow(tableName, id);
	    }

	    public Table GetTable(string tableName)
	    {
		    return TableDatabase.Get.GetTable(tableName);
	    }

	    public Tuple<uint, TableRow> FindLink(string tableName, string columnName, uint id)
	    {
		    return TableDatabase.Get.FindLink(tableName, columnName, id);
	    }

	    /// <summary>
	    /// Find rows that are associated with the id given.
	    /// </summary>
	    /// <param name="tableName"></param>
	    /// <param name="columnName"></param>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    public List<Tuple<uint, TableRow>> FindLinks(string tableName, string columnName, UInt32 id)
	    {
		    return TableDatabase.Get.FindLinks(tableName, columnName, id);
	    }
	    protected virtual void OnTableIDChanged()
	    {
		    TableDatabase.Get.Refresh();
	    }
    }
}
