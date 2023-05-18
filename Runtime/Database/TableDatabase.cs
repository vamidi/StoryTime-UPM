using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace StoryTime.Database
{
    using Binary;
    using ScriptableObjects;
    using ResourceManagement;

    /// <summary>
    /// TableDatabase stores table data from the json file and stores it into memory
    /// From memory everyone get fetch this instance and grab data
    /// </summary>
    public sealed class TableDatabase
    {
        // private UInt64 DatabaseVersion = 0;
        public ReadOnlyDictionary<string, LazyLoadReference<TableSO>> Tables => new (_tables);


        /// <summary>
        /// All the data (sorted per table) the application needs for reading
        /// This data has the table data from the json files.
        /// ReSharper disable once InconsistentNaming
        /// </summary>
        private readonly Dictionary<string, LazyLoadReference<TableSO>> _tables = new ();

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

        internal TableSO AddTable(string destination, TableMetaData metaData)
        {
	        TableSO table;
	        if (_tables.ContainsKey(metaData.id))
	        {
		        return _tables[metaData.id].asset;
	        }

	        // load it from the addressable
	        // if null again
	        table = HelperClass.CreateAsset<TableSO>(destination);
	        _tables[metaData.id] = table;
	        return table;
        }

        /// <summary>
        /// Export the ScriptableObject to the addressables' list.
        /// Export the JSON token to a json file in the Data folder.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="metadata"></param>
        internal void ExportToAddressable(string destination, TableMetaData metadata)
        {
#if UNITY_EDITOR
	        if (!File.Exists(destination))
	        {
		        AddTable(destination, metadata);
	        }

	        HelperClass.AddFileToAddressable(TableSO.GroupName, destination);
#endif
        }

        /// <summary>
        /// Return a table from the dictionary based on the name.
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public TableSO GetTable(string tableId)
        {
	        if (!_tables.ContainsKey(tableId))
	        {
		        Debug.LogWarningFormat("Could not find for ID {0}", tableId);
		        return null;
	        }

	        return _tables[tableId].asset;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public TableSO GetTableByName(string tableName)
        {
	        var source = _tables.First(t => t.Value.asset.Metadata.title == tableName);

	        if (source.Value.asset == null)
	        {
		        Debug.LogWarning($"Could not find {tableName}");
		        return null;
	        }

	        return source.Value.asset;
        }

        public string[] GetTableNames()
        {
	        return _tables.Values.Select(t => t.asset.Metadata.title).ToArray();
        }

        [CanBeNull]
        public TableSO.TableBinary GetBinary(string tableName)
        {
	        TableSO table = GetTable(tableName);
	        return table != null ? table.Binary : null;
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
	        var tables = HelperClass.FindAndLoadAssets<TableSO>();

	        // When we retrieved the file check if the user is already logged in
	        for (int t = 0; t < tables.Length; t++)
	        {
		        var table = tables[t];
		        EditorUtility.DisplayProgressBar("Importing tables", "Import table data...", (float) t / tables.Length);
		        if (table != null)
		        {
			        // Retrieve data from existing file, if it exists
			        table.Refresh();
			        _tables.Add(table.ID, table);
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
