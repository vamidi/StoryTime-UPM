﻿using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using StoryTime.ResourceManagement;

namespace StoryTime.FirebaseService.Database
{
    using Binary;
    using Configurations.ScriptableObjects;

    /// <summary>
    /// TableDatabase stores table data from the json file and stores it into memory
    /// From memory everyone get fetch this instance and grab data
    /// </summary>
    public sealed class TableDatabase
    {
        // private UInt64 DatabaseVersion = 0;

        /// <summary>
        /// All the data (sorted per table) the application needs for reading
        /// This data has the table data from the json files.
        /// ReSharper disable once InconsistentNaming
        /// </summary>
        private readonly Dictionary<string, TableSO> _tables = new Dictionary<string, TableSO>();

        // Explicit static constructor to tell C# compiler
        // not to mark type as before field init
        static TableDatabase() { }

        private TableDatabase()
        {
#if UNITY_EDITOR
	        Refresh();
#endif
        }

        // Get the timestamp of the last synced database version
        // UInt64 GetDatabaseVersion() { return DatabaseVersion; }

        public static TableDatabase Get { get; } = new();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static DialogueSettingConfigSO FetchDialogueSetting()
        {
#if UNITY_EDITOR
	        var path = EditorPrefs.GetString("StoryTime-Window-Dialogue-Settings-Config", "");
	        var configFile =
		        HelperClass.GetAsset<DialogueSettingConfigSO>(AssetDatabase.GUIDToAssetPath(path));
#else
			// TODO make it work outside the unity editor.
			var configFile = "";
#endif
	        return configFile;
        }

        public void Initialize()
        {

        }

        public List<Tuple<UInt32, TableRow>> FindLinks(string tableName, string columnName, UInt32 id)
        {
            List<Tuple<UInt32, TableRow>> result = new List<Tuple<uint, TableRow>>();

            TableSO table = GetTable(tableName);
            if (table)
            {
	            foreach(KeyValuePair<UInt32, TableRow> row in table.Rows)
	            {
		            foreach (KeyValuePair<TableRowInfo, TableField> field in row.Value.Fields)
		            {
			            if (field.Key.NotEquals(columnName))
				            continue;

			            double d = field.Value.Data;
			            // memcpy(&d, field.Value.Data.Get(), FMath::Min((size_t)sizeof(double), (size_t)field.Value.Size));
			            if ((uint) d == id)
				            result.Add(new Tuple<uint, TableRow>(row.Key, row.Value));
		            }
	            }
            }

            return result;
        }

        public /* const */ TableField GetField(string tableName, uint columnId, uint id)
        {
            TableRow row = GetRow(tableName, id);

            foreach (KeyValuePair<TableRowInfo, TableField> field in row.Fields)
            {
                if (field.Key.NotEquals(columnId))
                    continue;

                return field.Value;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public /* const */ TableField GetField(string tableName, string columnName, uint id)
        {
            TableRow row = GetRow(tableName, id);

            foreach (KeyValuePair<TableRowInfo, TableField> field in row.Fields)
            {
                if (field.Key.Equals(columnName))
                {
                    return field.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Return the row
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public TableRow GetRow(string tableName, uint entityID)
        {
	        TableSO table = GetTable(tableName);
	        Debug.Log(table);
            if (!table.Rows.ContainsKey(entityID))
            {
	            Debug.LogWarning($"{table.Metadata.title}: Couldn't find row {entityID} in table!");
	            TableRow r = new TableRow { RowId = entityID };
	            table.Rows.Add(entityID, r);
            }

            return table.Rows[entityID];
        }

        internal TableSO AddTable(string tableID, JToken jsonToken, TableMetaData metaData)
        {
	        TableSO table;
	        if (!_tables.ContainsKey(metaData.title))
	        {
		        // load it from the addressable
		        // if null again
		        table = ScriptableObject.CreateInstance<TableSO>();
		        _tables[metaData.title] = table;
	        }
	        else table = _tables[metaData.title];
	        return table;
        }

        /// <summary>
        /// Return a table from the dictionary based on the name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public TableSO GetTable(string tableName)
        {
	        if (!_tables.ContainsKey(tableName))
	        {
		        Debug.LogWarning($"Could not find {tableName}");
		        return null;
	        }

	        return _tables[tableName];
        }

        public TableSO.TableBinary GetBinary(string tableName)
        {
	        TableSO table = GetTable(tableName);
	        return table?.Binary;
        }

        public Tuple<uint, TableRow> FindLink(string tableName, string columnName, uint id)
        {
            /* const */ TableSO table = GetTable(tableName);

            foreach (KeyValuePair<uint, TableRow> row in table.Rows)
            {
	            var field = row.Value.Find(columnName);
	            if (field != null)
	            {
		            double d = field.Data;
		            // memcpy(&d, field.Value.Data.Get(), FMath::Min((size_t)sizeof(double), (size_t)field.Value.Size));
		            if ((uint) d == id)
			            return new Tuple<uint, TableRow>(row.Key, row.Value);
	            }
            }

            return new Tuple<uint, TableRow>(UInt32.MaxValue, null);
        }
        void RemoveCache()
        {
	        _tables.Clear();
        }

        /// <summary>
        /// Fetch existing data
        /// </summary>
        private void Refresh()
        {
	        // Clear out the data
	        RemoveCache();
	        // UpdateTime();

#if UNITY_EDITOR
	        // Get existing database files
	        FirebaseConfigSO config = FirebaseInitializer.Fetch();
	        var assetDirectory = config.dataPath;
	        var filePaths = Directory.GetFiles(assetDirectory, "*.asset");

	        // When we retrieved the file check if the user is already logged in
	        for (int t = 0; t < filePaths.Length; t++)
	        {
		        var filePath = filePaths[t];
		        EditorUtility.DisplayProgressBar("Importing tables", "Import table data...", (float) t / filePaths.Length);
		        if (File.Exists(filePath))
		        {
			        TableSO table = HelperClass.GetAsset<TableSO>(filePath, true);
			        if (table != null)
			        {
				        // Retrieve data from existing file, if it exists
				        table.Refresh();
				        _tables.Add(table.Metadata.title, table);
			        }
		        }
	        }
	        EditorUtility.ClearProgressBar();
#else
	        FirebaseInitializer.Fetch((task) =>
	        {
		        var config = task.Result;
		        if (!config)
		        {
			        throw new ArgumentNullException($"{nameof(config)} must not be null.", nameof(config));
		        }

		        var assetDirectory = config.dataPath;
		        var filePaths = Directory.GetFiles(assetDirectory, "*.asset");
		        foreach (string filePath in filePaths)
		        {
			        // Fetch existing data from the addressable list.
			        string fileName = Path.GetFileNameWithoutExtension(filePath);
			        HelperClass.GetFileFromAddressable<TableSO>(fileName).Completed += handle =>
			        {
				        Debug.Log(handle.Result);
				        if (handle.Result == null) return;

				        // Retrieve data from existing file, if it exists
				        TableSO table = handle.Result;
				        table.Refresh();
				        _tables.Add(table.Metadata.title, table);
			        };
		        }
	        });
#endif
	        Debug.Log(_tables.Count);
        }
    }
}
