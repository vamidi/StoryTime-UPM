﻿using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using UnityEngine;

using StoryTime.FirebaseService.Database.Editor;

namespace StoryTime.FirebaseService.Database.Binary
{
	using Database;
	using StoryTime.ResourceManagement;
	using Configurations.ScriptableObjects;

	[Serializable]
	public class TableMetaData
	{
		// ReSharper disable once InconsistentNaming
		public long created_at = 0;

		// ReSharper disable once InconsistentNaming
		public bool deleted = false;

		// ReSharper disable once InconsistentNaming
		public string description = "";

		// ReSharper disable once InconsistentNaming
		public uint lastUID = 0;

		// public bool private = false;

		// ReSharper disable once InconsistentNaming
		public string title = "";

		// ReSharper disable once InconsistentNaming
		public long updated_at = 0;

		public override string ToString()
		{
			return
				$"created {created_at}, deleted: {deleted}, description: {description}, lastUID: {lastUID}, title: {title}, updated_at: {updated_at}";
		}
	}

	// ReSharper disable once InconsistentNaming
    public partial class TableSO : ScriptableObject
    {
	    public string ID => id;
	    public string ProjectID => projectID;
	    public TableMetaData Metadata => metadata;

	    public TableBinary Binary => binary;

	    // ReSharper disable once InconsistentNaming
	    [SerializeField] private string id = "";

	    // ReSharper disable once InconsistentNaming
	    [SerializeField] private string projectID = "";

	    // ReSharper disable once InconsistentNaming
	    [SerializeField] private TableMetaData metadata = new TableMetaData();

	    [SerializeField, TextAreaAttribute(15,20)] private string jsonData = "";

	    private readonly TableBinary binary = new TableBinary();

        public Dictionary<UInt32, TableRow> Rows = new Dictionary<UInt32, TableRow>();

#if !UNITY_EDITOR
        public void OnEnable() => Refresh();
#endif

	    public override string ToString()
	    {
		    return $"{metadata.title} with {Rows.Count} rows.";
	    }

	    /// <summary>
        /// Import the json data from the sync module and transform it
        /// into rows and fields.
        /// </summary>
        /// <param name="tableID"></param>
        /// <param name="item"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Import(string tableID, JToken item)
        {
	        if(item == null || tableID == "" || item["projectID"] == null || item["metadata"] == null )
#if UNITY_EDITOR
				Debug.Log("Can't make Table from JSON file");
#else
		        throw new ArgumentException("Can't make Table from JSON file");
#endif
	        id = tableID;
	        projectID = item["projectID"].Value<string>();
	        metadata = item["metadata"].ToObject<TableMetaData>();
	        Binary.Import(tableID, item, ref Rows, out jsonData);
        }

        /// <summary>
        /// Export the ScriptableObject to the addressables' list.
        /// </summary>
        internal void Export()
        {
#if !UNITY_EDITOR
	        FirebaseConfigSO config = TableDatabase.Fetch();
	        string destination = $"{config.dataPath}/{metadata.title}.asset";

	        if (!Directory.Exists(config.dataPath))
		        Directory.CreateDirectory(config.dataPath);

	      	HelperClass.CreateAsset(this, destination);
	      	HelperClass.AddFileToAddressable(DatabaseSyncModule.GroupName, destination);
#endif
        }

        public void Refresh()
        {
	        // Retrieve data from existing file, if it exists
	        // Load the data from the json string we already parsed at the beginning
	        Binary.Refresh(jsonData, ref Rows);
        }
    }
}
