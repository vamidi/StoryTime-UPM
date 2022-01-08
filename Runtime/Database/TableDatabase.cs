using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Newtonsoft.Json.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace StoryTime.Database
{
    using Binary;
    using ResourceManagement.Util;
    using Configurations.ScriptableObjects;
    /// <summary>
    /// TableDatabase stores table data from the json file and stores it into memory
    /// From memory everyone get fetch this instance and grab data
    /// </summary>
    public sealed class TableDatabase
    {
        // private UInt64 DatabaseVersion = 0;
        public ReadOnlyCollection<TableSO> Tables => Data.Values.ToList().AsReadOnly();

        /// <summary>
        /// All the data (sorted per table) the application needs for reading
        /// This data has the table data from the json files.
        /// ReSharper disable once InconsistentNaming
        /// </summary>
        private readonly Dictionary<string, TableSO> Data = new Dictionary<string, TableSO>();

        // Explicit static constructor to tell C# compiler
        // not to mark type as before field init
        static TableDatabase() { }

        private TableDatabase()
        {
#if !UNITY_EDITOR
	        Refresh();
#endif
        }

        // Get the timestamp of the last synced database version
        // UInt64 GetDatabaseVersion() { return DatabaseVersion; }

        public static TableDatabase Get { get; } = new TableDatabase();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static DatabaseConfigSO Fetch()
        {
#if UNITY_EDITOR
	        var configFile = HelperClass.GetAsset<DatabaseConfigSO>( EditorPrefs.GetString("StoryTime-Window-Settings-Config", ""));
#else
			var configFile = null;
#endif
	        // TODO this will ruin the editor and stops the database sync setting window.
	        // if (configFile == null)
	        // throw new ArgumentNullException($"{nameof(configFile)} can not be null.", nameof(configFile));
	        return configFile;
        }

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
			var configFile = "";
#endif
	        return configFile;
        }

        public List<Tuple<UInt32, TableRow>> FindLinks(string tableName, string columnName, UInt32 id)
        {
            List<Tuple<UInt32, TableRow>> result = new List<Tuple<uint, TableRow>>();

            TableSO table = GetTable(tableName);

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
            if (!table.Rows.ContainsKey(entityID))
            {
	            Debug.LogWarning($"{table.Metadata.title}: Couldn't find row {entityID} in table!");
	            TableRow r = new TableRow { RowId = entityID };
	            table.Rows.Add(entityID, r);
            }

            return table.Rows[entityID];
        }

        public TableSO GetTableById(string tableID)
        {
	        foreach (var table in Tables)
	        {
		        if (table.ID == tableID)
			        return table;
	        }

	        return null;
        }

        internal void AddTable(JToken jsonToken, TableMetaData metaData)
        {
	        TableSO table;
	        if (!Data.ContainsKey(metaData.title))
	        {
		        // load it from the addressable
		        // if null again
		        table = ScriptableObject.CreateInstance<TableSO>();
		        Data[metaData.title] = table;
	        }
	        else table = Data[metaData.title];
	        table.Import(jsonToken);
        }

        /// <summary>
        /// Return a table from the dictionary based on the name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public TableSO GetTable(string tableName)
        {
	        if (!Data.ContainsKey(tableName))
		        return null;

	        return Data[tableName];
        }

        public TableSO.TableBinary GetBinary(string tableName)
        {
	        TableSO table = GetTable(tableName);
	        return table ? table.Binary : null;
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
            Data.Clear();
        }

        /// <summary>
        /// Fetch existing data
        /// </summary>
        public void Refresh()
        {
	        // Clear out the data
	        RemoveCache();
	        // UpdateTime();

#if UNITY_EDITOR
	        // Get existing database files
	        DatabaseConfigSO config = Fetch();
	        var assetDirectory = config.dataPath;
	        var filePaths = Directory.GetFiles(assetDirectory, "*.asset");

	        Debug.Log(filePaths.Length);
	        foreach (var filePath in filePaths)
	        {
		        if (File.Exists(filePath))
		        {
			        TableSO table = HelperClass.GetAsset<TableSO>(filePath, true);
			        if (table != null)
			        {
				        // Retrieve data from existing file, if it exists
				        table.Refresh();
				        Data.Add(table.Metadata.title, table);
			        }
		        }
#else
				// Fetch existing data from the addressable list.
		        string fileName = Path.GetFileNameWithoutExtension(filePath);
		        HelperClass.GetFileFromAddressable<TableSO>(fileName).Completed += handle => {
			        if (handle.Result == null) return;

			        // Retrieve data from existing file, if it exists
			        handle.Result.Refresh();
			        Data.Add(handle.Metadata.title, handle);
		        };
#endif
	        }
        }
    }
}
