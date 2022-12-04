using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

using Newtonsoft.Json.Linq;

using StoryTime.ResourceManagement;

// ReSharper disable once CheckNamespace
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
		public static event EventHandler<string> onFetchCompleted;

		internal static readonly string GroupName = "StoryTime-Assets-Tables-Shared";
		private static readonly string s_DataPath = "Packages/com.vamidicreations.storytime";

		private static Firebase.Database.FirebaseDatabase _database;

		private static List<TableSO> tables = new ();

		private Int64 _lastTimeStamp;
		private bool _canFetch = true;

		private DependencyStatus _firebaseInitialized = DependencyStatus.UnavailableOther;

		// Check to see if we're about to be destroyed.
		// private static bool m_ShuttingDown = false;
		// private static object m_Lock = new object();
		// private static DatabaseSyncModule s_Instance;

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static DatabaseSyncModule Get => new();

		public void Initialize()
		{
			Addressables.InitializeAsync().Completed += AddressableCompleted;
#if UNITY_EDITOR
			if (!EditorApplication.isPlayingOrWillChangePlaymode)
			{
				RequestTableUpdate();
			}
#endif
		}

		/// <summary>
		/// Request data from Firebase.
		/// This methods allows the user to fetch all tables or a single table
		/// from the Firebase database
		/// </summary>
		/// <param name="tableID"></param>
		/// <param name="save"></param>
		public void RequestTableUpdate(string tableID = "", bool save = false)
		{
			if (_database == null)
			{
				Debug.LogWarning("Database is not initialized. Retrying...");
				InitializeFirebase();
				return;
			}

			// Wait few seconds before we let the user click again.
			if (!_canFetch)
			{
#if UNITY_EDITOR
				EditorUtility.DisplayDialog("Please wait", "We are already processing data from server!", "OK");
#endif
				return;
			}
			_canFetch = false;

			if (!FirebaseInitializer.signedIn)
			{
				Debug.LogWarning("User is not found or logged in. Please login");
				_canFetch = true;
				return;
			}

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

							Task[] tasks = new Task[jsonToken.Count];
							int idx = 0;
							foreach (var token in jsonToken)
							{
								// Download all tables.
								tasks[idx] = RequestTable(token.Key, save);
								idx++;
							}

							Task.WhenAll(tasks).ContinueWithOnMainThread(_ =>
							{
								// When we retrieved the file check if the user is already logged in
								for (int t = 0; t < tables.Count; t++)
								{
									var table = tables[t];
#if UNITY_EDITOR
									EditorUtility.DisplayProgressBar("Exporting tables", $"Exporting {table.Metadata.title}...", (float)t / tables.Count);
#endif
									table.Export();
								}
#if UNITY_EDITOR
								EditorUtility.ClearProgressBar();
#endif

								tables.Clear();
								OnResponseReceived();
							});
						}
					});
				return;
			}


			// Request table directly
			RequestTable(tableID, save);
		}

		private void AddressableCompleted(AsyncOperationHandle<IResourceLocator> obj)
		{
			// HelperClass.GetFileFromAddressable<>()
		}

		private void AuthStateChanged(object sender, EventArgs eventArgs) {
			Debug.Log("DatabaseSyncModule: Auth changed");
			if(_firebaseInitialized == DependencyStatus.Available && FirebaseInitializer.Auth.CurrentUser != null)
				RequestTableUpdate();
		}

		private void InitializeFirebase()
		{
			if (_firebaseInitialized == DependencyStatus.Available && FirebaseInitializer.Auth.CurrentUser != null)
			{
				RequestTableUpdate();
				return;
			}

			FirebaseInitializer.Get.Initialize((status) =>
			{
				_firebaseInitialized = status;
				if (status == DependencyStatus.Available)
				{
					_database = FirebaseInitializer.Database;
					FirebaseInitializer.Auth.StateChanged += AuthStateChanged;
				}
			});
		}

		/// <summary>
		/// Add table to
		/// </summary>
		/// <param name="tableID"></param>
		/// <param name="save"></param>
		/// <exception cref="ArgumentException"></exception>
		private Task<TableSO> RequestTable(string tableID, bool save = false)
		{
			return _database.GetReference($"tables/{tableID}")
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

						// Debug.LogFormat("Fetching table {0}, {1}", tableID, tableMetadata.title);

						// First fetch the table async
						return CreateTable(tableID, item, tableMetadata);
					}

					return new();
				}).ContinueWith((task) =>
				{
					if (task.IsCompleted)
					{
						TableResult tableResult = task.Result;
						if (tableResult.Table != null && tableResult.Item != null)
						{
							TableSO table = tableResult.Table;

							table.Import(tableID, tableResult.Item);
							onFetchCompleted?.Invoke(this, table.Metadata.title);
							tables.Add(table);
							return table;
						}
					}

					return null;
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
			var config = FirebaseConfigSO.FindSettings();

			// Update editor
			// auto& PropertyModule = FModuleManager::LoadModuleChecked< FPropertyEditorModule >("PropertyEditor");
			// PropertyModule.NotifyCustomizationModuleChanged();
#else
			FirebaseInitializer.Fetch((task) =>
			{
				var config = task.Result;
				if (!config)
				{
					throw new ArgumentNullException($"{nameof(config)} must not be null.", nameof(config));
				}
#endif
				if (config && !string.IsNullOrEmpty(config.dataPath))
				{
					// Update timestamp
					_lastTimeStamp = DateTime.Now.Ticks;

					string destination = $"{config.dataPath}/uptime.txt";
					File.WriteAllText(destination, _lastTimeStamp.ToString());
					// Add the uptime file to the addressable group
					HelperClass.AddFileToAddressable(GroupName, destination);
				}

				// TODO see if unity has notify system
				Debug.Log("Fetching complete");
				_canFetch = true;
#if !UNITY_EDITOR
			});
#endif
		}
	}
}
