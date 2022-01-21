using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace StoryTime.Binary
{
	using Database;
	using ResourceManagement.Util;
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

	    [SerializeField] private string jsonData = "";

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
        /// <param name="item"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Import(JToken item)
        {
	        if(item == null || item["id"] == null || item["projectID"] == null || item["metadata"] == null )
		        throw new ArgumentException("Can't make Table from JSON file");

	        id = item["id"].Value<string>();
	        projectID = item["projectID"].Value<string>();
	        metadata = item["metadata"].ToObject<TableMetaData>();
	        Binary.Import(item, out jsonData);
	        // Export the new object again.
	        Export();
	        // Refresh the json data
	        Refresh();
        }

        /// <summary>
        /// Export the Scriptableobject to the addressables' list.
        /// </summary>
        private void Export()
        {
	        DatabaseConfigSO config = TableDatabase.Fetch();
	        string destination = $"{config.dataPath}/{metadata.title}.asset";

	        if (!Directory.Exists(config.dataPath))
		        Directory.CreateDirectory(config.dataPath);

#if UNITY_EDITOR
	        HelperClass.CreateAsset(this, destination);
	        HelperClass.AddFileToAddressable("JSON_data", destination);
	        //
	        HelperClass.ChangeAddressableAddress(
		        AssetDatabase.AssetPathToGUID(HelperClass.MakePathRelative(destination)), metadata.title.ToLower()
		    );
#endif
        }

        public void Refresh()
        {
#if UNITY_EDITOR
	        // Retrieve data from existing file, if it exists
	        Binary.Refresh(jsonData, ref Rows);
#else
			// Load the data from the json string we already parsed at the beginning
	        var token = TableBinary.GetTableData(jsonData);
	        Binary.Refresh(token, ref Rows);
#endif
	        Debug.Log(this);
        }
    }
}
