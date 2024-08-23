using System;
using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Domains.Database.ScriptableObjects
{
	using Binary;
	using StoryTime.Domains.Attributes;
	using StoryTime.Domains.Database.ScriptableObjects.Serialization;
	
    public partial class TableBehaviour : SerializableScriptableObject
    {
	    /// <summary>
	    /// ID of the row inside the table.
	    /// </summary>
	    public virtual string ID
	    {
		    get => id;
		    internal set {
			    id = value;
#if UNITY_EDITOR
			    OnTableIDChanged();
#endif
		    }
	    }

	    /// <summary>
	    /// Name of the table that we are using.
	    /// </summary>
	    public string TableName { get => tableName; internal set => tableName = value; }
	    public string DropdownColumn { get => dropdownColumn; internal set => dropdownColumn = value; }
	    public String LinkedID { get => linkID; internal set => linkID = value; }
	    public string LinkedColumn { get => linkedColumn; internal set => linkedColumn = value; }
		public string LinkedTable { get => linkedTable; internal set => linkedTable = value; }

	    [SerializeField, Uuid] private string id;
	    [SerializeField, HideInInspector] private string tableName = String.Empty;
	    [SerializeField, HideInInspector] private string tableId = String.Empty;
	    [SerializeField, HideInInspector] private string dropdownColumn = String.Empty;
	    [SerializeField, HideInInspector] private String linkID = String.Empty;
	    [SerializeField, HideInInspector] private string linkedColumn = String.Empty;
	    [SerializeField, HideInInspector] private string linkedTable = String.Empty;

	    public TableBehaviour(string name, string dropdownColumn, string linkedColumn = "",
		    String linkedId = "", string linkedTable = "")
	    {
		    Init(name, dropdownColumn, linkedColumn, linkedId, linkedTable);
	    }

	    public void Init(string tblName, string dropdownColumn, string linkedColumn, String linkedId, string linkedTable)
	    {
		    TableName = tblName;
		    DropdownColumn = dropdownColumn;

		    LinkedColumn = linkedColumn;
		    LinkedID = linkedId;
		    LinkedTable = linkedTable;

		    // Load data from Firebase
#if UNITY_EDITOR

#else
			// If we already have the ID of the row we should retrieve data from then we can call it directly
		    FirebaseInitializer.Database
			    .GetReference($"projects/{FirebaseInitializer.DatabaseConfigSo.ProjectID}/tables/{tableName}/{ID}")
			    .GetValueAsync().ContinueWith(task =>
			    {
				    if (task.IsFaulted)
				    {
					    // Handle the error...
				    }
				    else if (task.IsCompleted)
				    {
					    Firebase.Database.DataSnapshot snapshot = task.Result;
					    Debug.Log(snapshot.GetRawJsonValue());
						// Do something with snapshot...
					}
				});
		    }
#endif
	    }

	    public virtual void Awake()
	    {
		    /*
		    var aaSettings = GlobalSettingsSO.GetOrCreateSettings().GetAddressableAssetSettings(true);
		    if (aaSettings == null)
			    return;

		    var tableEntry = TableType == typeof(StringTable) ? AddressableGroupRules.AddStringTableAsset(table, aaSettings, createUndo));

		    if (createUndo)
			    Undo.RecordObjects(new UnityEngine.Object[] { aaSettings, tableEntry.parentGroup }, "Update table");

		    tableEntry.address = AddressHelper.GetTableAddress(table.TableCollectionName, table.LocaleIdentifier);
		    tableEntry.labels.RemoveWhere(AddressHelper.IsLocaleLabel); // Locale may have changed so clear the old ones.

		    // Label the locale
		    var localeLabel = AddressHelper.FormatAssetLabel(table.LocaleIdentifier);
		    tableEntry.SetLabel(localeLabel, true, true);
		    */
	    }

	    public virtual void Reset() { }

	    /// <summary>
	    ///
	    /// </summary>
	    /// <param name="tblName"></param>
	    /// <param name="columnId"></param>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    public TableField GetField(string tblName, uint columnId, String id)
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
	    public TableField GetField(string tblName, string columnName, String id)
	    {
		    return TableDatabase.Get.GetField(tblName, columnName, id);
	    }

	    /// <summary>
	    ///
	    /// </summary>
	    /// <param name="tblName"></param>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    public TableRow GetRow(string tblName, String id)
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
	    public Tuple<String, TableRow> FindLink(string tblName, string columnName, String id)
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
	    public List<Tuple<String, TableRow>> FindLinks(string tblName, string columnName, String id)
	    {
		    return TableDatabase.Get.FindLinks(tblName, columnName, id);
	    }
	    protected virtual void OnTableIDChanged() { }
    }
}
