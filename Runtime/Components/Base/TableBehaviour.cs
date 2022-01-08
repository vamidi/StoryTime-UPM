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
#if UNITY_EDITOR
			    OnTableIDChanged();
#endif
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

	    public void Init(string tblName, string dropdownColumn, string linkedColumn, UInt32 linkedId, string linkedTable)
	    {
		    Name = tblName;
		    DropdownColumn = dropdownColumn;

		    LinkedColumn = linkedColumn;
		    LinkedID = linkedId;
		    LinkedTable = linkedTable;
	    }

	    public virtual void Reset() { }

	    /// <summary>
	    ///
	    /// </summary>
	    /// <param name="tblName"></param>
	    /// <param name="columnId"></param>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    public TableField GetField(string tblName, uint columnId, uint id)
	    {
		    return TableDatabase.Get.GetField(tblName, columnId, id);
	    }

	    /// <summary>
	    ///
	    /// </summary>
	    /// <param name="tblName"></param>
	    /// <param name="columnName"></param>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    public TableField GetField(string tblName, string columnName, uint id)
	    {
		    return TableDatabase.Get.GetField(tblName, columnName, id);
	    }

	    /// <summary>
	    ///
	    /// </summary>
	    /// <param name="tblName"></param>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    public TableRow GetRow(string tblName, uint id)
	    {
		    return TableDatabase.Get.GetRow(tblName, id);
	    }

	    /// <summary>
	    ///
	    /// </summary>
	    /// <param name="tblName"></param>
	    /// <returns></returns>
	    public TableSO GetTable(string tblName)
	    {
		    return TableDatabase.Get.GetTable(tblName);
	    }

	    /// <summary>
	    ///
	    /// </summary>
	    /// <param name="tblName"></param>
	    /// <param name="columnName"></param>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    public Tuple<uint, TableRow> FindLink(string tblName, string columnName, uint id)
	    {
		    return TableDatabase.Get.FindLink(tblName, columnName, id);
	    }

	    /// <summary>
	    /// Find rows that are associated with the id given.
	    /// </summary>
	    /// <param name="tblName"></param>
	    /// <param name="columnName"></param>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    public List<Tuple<uint, TableRow>> FindLinks(string tblName, string columnName, UInt32 id)
	    {
		    return TableDatabase.Get.FindLinks(tblName, columnName, id);
	    }
	    protected virtual void OnTableIDChanged()
	    {
		    TableDatabase.Get.Refresh();
	    }
    }
}
