using System;
using System.IO;
using Firebase;
using Firebase.Extensions;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AddressableAssets;

using Newtonsoft.Json.Linq;

using StoryTime.ResourceManagement;

namespace StoryTime.FirebaseService.Database.Editor
{
	using Binary;
	using Database;
	using Configurations.ScriptableObjects;

	/// <summary>
	/// The DatabaseSync Module is in charge of making the connection to
	/// the database. Once that data is sync it will create or find the existing
	/// table scriptable object and update the data.
	/// </summary>
	public class DatabaseSyncModule
	{
		private static readonly string s_GroupName = "JSON_data";
		private static readonly string s_DataPath = "Packages/com.vamidicreations.storytime";

		private static Firebase.Database.FirebaseDatabase _database;

		public static event EventHandler<string> onFetchCompleted;

		private Int64 _lastTimeStamp;
		private bool _canFetch = true;

		// Check to see if we're about to be destroyed.
		// private static bool m_ShuttingDown = false;
		// private static object m_Lock = new object();
		// private static DatabaseSyncModule s_Instance;

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static DatabaseSyncModule Get => new();

		static DatabaseSyncModule() { }

		public void Initialize()
		{
			// Load existing data
			Addressables.InitializeAsync().Completed += (result) =>
			{
				// Get all tables when we are done loading the firebase app.


				// Retrieve the tables from the project that is selected
				FirebaseInitializer.Get.Initialize((status) =>
				{
					if (status == DependencyStatus.Available)
					{
						_database = FirebaseInitializer.Database;
						RequestTableUpdate();
					}
				});
			};

			// When we retrieved the file check if the user is already logged in
			// EditorUtility.DisplayProgressBar("Simple Progress Bar", "Doing some work...", t / secs);
			// for (float t = 0; t < secs; t += step)
			// {
			// Normally, some computation happens here.
			// This example uses Sleep.
			// Thread.Sleep((int)(step * 1000.0f));
			// }
			// EditorUtility.ClearProgressBar();
		}

		/// <summary>
		/// Request data from Firebase.
		/// This methods allows the user to fetch all tables or a single table
		/// from the Firebase database
		/// </summary>
		/// <param name="tableID"></param>
		public void RequestTableUpdate(string tableID = "")
		{
			// Wait few seconds before we let the user click again.
			if (!_canFetch)
			{
				EditorUtility.DisplayDialog("Please wait", "We are already processing data from server!", "OK");
				return;
			}
			_canFetch = false;

			if (tableID == String.Empty)
			{
				_database
					.GetReference($"projects/{FirebaseInitializer.DatabaseConfigSo.ProjectID}/tables")
					.GetValueAsync().ContinueWithOnMainThread(task =>
					{
						if (task.IsFaulted)
						{
							// Handle the error...
							Debug.LogError("Error retrieving tables");
							// Debug.LogError("Error: " + request.error);
							// Debug.LogError("Error: " + request.downloadHandler.text);
						}
						else if (task.IsCompleted)
						{
							// Do something with snapshot...
							Firebase.Database.DataSnapshot snapshot = task.Result;
							// var tasks = new List<Task>();
							var tablesStr = snapshot.GetRawJsonValue();
							var jsonToken = JObject.Parse(tablesStr);

							foreach (var token in jsonToken)
							{
								// Download all tables.
								RequestTable(token.Key);
								// tasks.Add();
							}

							OnResponseReceived();
						}
					});
				return;
			}


			// Request table directly
			RequestTable(tableID, true);
		}



		/// <summary>
		/// Add table to
		/// </summary>
		/// <param name="tableID"></param>
		/// <param name="save"></param>
		/// <exception cref="ArgumentException"></exception>
		private void RequestTable(string tableID, bool save = false)
		{
			_database.GetReference($"tables/{tableID}")
				.GetValueAsync().ContinueWithOnMainThread(task =>
				{
					if (task.IsFaulted)
					{
						// Handle the error...
						Debug.LogWarning($"Error retrieving table: {tableID}");
						// Debug.LogError("Error: " + request.error);
						// Debug.LogError("Error: " + request.downloadHandler.text);
					}

					if (task.IsCompleted)
					{
						Firebase.Database.DataSnapshot snapshot = task.Result;
						var tableStr = snapshot.GetRawJsonValue();
						var item = JToken.Parse(tableStr);

						if (item["projectID"] == null || item["metadata"] == null)
							throw new ArgumentException("Can't make Table from JSON file");

						TableMetaData tableMetadata = item["metadata"].ToObject<TableMetaData>();
						// Export to addressable is handle in the Table itself --> SOLID PATTERN

						// First fetch the table async
						return CreateTable(tableID, item, tableMetadata);
					}

					return new();
				}).ContinueWith((task) =>
				{
					if (task.IsCompleted)
					{
						TableResult tableResult = task.Result;
						if (tableResult.Table)
						{
							TableSO table = tableResult.Table;

							table.Import(tableID, tableResult.Item);
							onFetchCompleted?.Invoke(this, table.Metadata.title);
							return table;
						}
					}

					return null;
				}).ContinueWithOnMainThread(task =>
				{
					if (task.Result && save)
					{
						Debug.Log($"Exporting {task.Result.Metadata.title}");
						task.Result.Export();
					}
				});
		}

		private struct TableResult
		{
			public TableSO Table;
			public JToken Item;
		}

		private TableResult CreateTable(string tableID, JToken item, TableMetaData tableMetadata)
		{
			return new()
			{
				Table = TableDatabase.Get.AddTable(tableID, item, tableMetadata),
				Item = item
			};
		}

		private void OnResponseReceived()
		{
#if UNITY_EDITOR // TODO see if unity has notify system
				// Update editor
				// auto& PropertyModule = FModuleManager::LoadModuleChecked< FPropertyEditorModule >("PropertyEditor");
				// PropertyModule.NotifyCustomizationModuleChanged();
#endif
			FirebaseConfigSO config = FirebaseInitializer.Fetch();
			if (config && !string.IsNullOrEmpty(config.dataPath))
			{
				// Update timestamp
				_lastTimeStamp = DateTime.Now.Ticks;

				string destination = $"{config.dataPath}/uptime.txt";
				File.WriteAllText(destination, _lastTimeStamp.ToString());
				// Add the uptime file to the addressable group
				HelperClass.AddFileToAddressable(s_GroupName, destination);
			}

			Debug.Log("Fetching complete");
			_canFetch = true;
		}
	}
}
