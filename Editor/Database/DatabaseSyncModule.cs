using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

using Newtonsoft.Json.Linq;
using StoryTime.Utils;
using UnityEngine.Networking;

namespace StoryTime.Editor.Database
{
	using StoryTime.Database;
	using StoryTime.ResourceManagement;
	using StoryTime.Database.ScriptableObjects;

	using FirebaseService.Settings;
	using Configurations.ScriptableObjects;

	/// <summary>
	/// The DatabaseSync Module is in charge of making the connection to
	/// the database. Once that data is sync it will create or find the existing
	/// table scriptable object and update the data.
	/// </summary>
	public class DatabaseSyncModule
	{
		internal const string GroupName = "StoryTime-Assets-Shared";

		public static event EventHandler<string> onFetchCompleted;

		private static readonly string s_DataPath = "Packages/com.vamidicreations.storytime";

		// private static Firebase.Database.FirebaseDatabase _database;

		private bool _canFetch = true;

		// Check to see if we're about to be destroyed.
		// private static bool m_ShuttingDown = false;
		// private static object m_Lock = new object();
		// private static DatabaseSyncModule s_Instance;

		private readonly string _fallbackSettingsPath;

		private FirebaseConfigSO config;
		private GlobalSettingsSO globalSettings;

		private struct ProjectResponse
		{
			public string uid;
			public string name;
			public string roles;
		}

		private struct TableResponse
		{
			public string uid;
			public string name;
			public string roles;
		}

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static DatabaseSyncModule Get => new();

		private DatabaseSyncModule()
		{
			_fallbackSettingsPath = $"{Application.dataPath}/Settings/StoryTime";
		}

		public void Initialize()
		{
			config = FirebaseConfigSO.GetOrCreateSettings();
			globalSettings = GlobalSettingsSO.GetOrCreateSettings();
#if !UNITY_EDITOR
			Addressables.InitializeAsync().Completed += AddressableCompleted;
		#else
			InitializeAddressables();
#endif
		}

		/// <summary>
		/// Request data from Firebase.
		/// This methods allows the user to fetch all tables or a single table
		/// from the Firebase database
		/// </summary>
		/// <param name="tableID"></param>
		/// <param name="save"></param>
		public async Task RequestTableUpdate(string tableID = "", bool save = false)
		{
			string destination = $"{_fallbackSettingsPath}/uptime_tables.txt";
			if (globalSettings && !string.IsNullOrEmpty(globalSettings.DataPath))
			{
				destination = $"{globalSettings.DataPath}/uptime_tables.txt";
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

			if (tableID == String.Empty)
			{
				var lastTimeStamp = await GetUptime(destination);

				var request = UnityWebRequest.Get($"{config.DatabaseURL}/{config.Platform}/projects/{config.ProjectID}/tables?time={lastTimeStamp}");
				request.SendWebRequest();
				while (!request.isDone) {
					await Task.Yield();
				}

				// var tasks = new List<Task>();
				string tablesStr = request.downloadHandler.text;
				var response = JObject.Parse(tablesStr);

				bool success = response["success"]?.ToObject<bool>() ?? false;

				if (!success)
				{
					Debug.LogWarning("error while fetching tables");
					return;
				}

				if (response["data"] == null)
				{
					Debug.Log("No new table data found");
					_canFetch = true;
					return;
				}

				JArray data = response["data"].Value<JArray>();

				Task[] tasks = new Task[data.Count];

				int idx = 0;
				foreach (var element in data)
				{
					// Debug.Log(title);

					// Download all tables.
					/*
					var tableMetadata = new TableMetaData
					{
						title = title
					};
					*/
					// TableDatabase.Get.AddTable($"{GetDataPath()}/{tableMetadata.title}.asset", tableMetadata);
					// ExportTableToAddressable(tableMetadata);
					tasks[idx] = RequestTable(element["uid"].ToString(), save);
					idx++;
				}

				// TODO create async on main thread
				await Task.WhenAll(tasks);
				await WriteUptime(destination, TableSO.GroupName);
				return;
			}


			// Request table directly
			await RequestTable(tableID, save).ContinueWith((table) =>
			{
				// Save all the created assets
				HelperClass.RefreshAssets();

				// TODO see if unity has notify system
				Debug.Log("Fetching complete");
				_canFetch = true;
			});
		}

		/// <summary>
		/// Add table to
		/// </summary>
		/// <param name="tableID"></param>
		/// <param name="save"></param>
		/// <exception cref="ArgumentException"></exception>
		private async Task<TableSO> RequestTable(string tableID, bool save = false)
		{
			var request = UnityWebRequest.Get($"{config.DatabaseURL}/{config.Platform}/projects/{config.ProjectID}/tables/{tableID}");
			request.SendWebRequest();
			while (!request.isDone) {
				await Task.Yield();
			}

			// var tasks = new List<Task>();
			string tablesStr = request.downloadHandler.text;
			var response = JObject.Parse(tablesStr);

			bool success = response["success"]?.ToObject<bool>() ?? false;

			if (!success || response["data"] == null)
			{
				Debug.LogWarningFormat("Could not fetch data from table: {0}", tableID);
				return null;
			}

			var item = response["data"].ToObject<JObject>();

			if (item["metadata"] == null)
				throw new ArgumentException("Can't make Table from JSON file");

			TableMetaData tableMetadata = item["metadata"].ToObject<TableMetaData>();
			tableMetadata.id = tableID;
			// Export to addressable is handle in the Table itself --> SOLID PATTERN

			// Debug.LogFormat("Fetching table {0}, {1}", tableID, tableMetadata.title);

			// First fetch the table async
			string destination = $"{GetDataPath()}/{tableMetadata.title}.asset";

			TableResult tableResult = CreateTable(destination, item, tableMetadata);
			if (tableResult.Table != null && tableResult.Item != null)
			{
				TableSO table = tableResult.Table;

				table.Import(tableID, tableResult.Item);
				return table;
			}

			return null;
		}

#if !UNITY_EDITOR
		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		private void AddressableCompleted(AsyncOperationHandle<IResourceLocator> obj)
		{
			// HelperClass.GetFileFromAddressable<>()
			InitializeAddressables();
		}
#endif
		/// <summary>
		///
		/// </summary>
		private async void InitializeAddressables()
		{
			// Retrieve the projects of the user
			await RetrieveProjects();
			await RequestTableUpdate();
			RunOnMainThread.Schedule(() =>
			{
				// Save all the created assets
				HelperClass.RefreshAssets();

				// TODO see if unity has notify system
				Debug.Log("Fetching complete");
				_canFetch = true;
			}, 0);
		}


		private async Task RetrieveProjects()
		{
			var destination = $"{_fallbackSettingsPath}/uptime_projects.txt";
			var lastTimeStampLoadedProjects = await GetUptime(destination);

			var request = UnityWebRequest.Get($"{config.DatabaseURL}/{config.Platform}/projects?time={lastTimeStampLoadedProjects}");
			request.SendWebRequest();
			while (!request.isDone) {
				await Task.Yield();
			}

			var responses = new List<ProjectResponse>();
			switch (request.result)
			{
				case UnityWebRequest.Result.ConnectionError:
					throw new ArgumentException( $"Connection error: {request.error}");
				// case UnityWebRequest.Result.ProtocolError:
					// Debug.Log("Protocol error");
					// throw new ArgumentException( $"Protocol error: {request.error}");
			}

			string str = request.downloadHandler.text;
			var response = JObject.Parse(str);

			bool success = response["success"]?.ToObject<bool>() ?? false;

			if (!success)
			{
				return;
			}

			JArray data = response["data"].Value<JArray>();

			foreach (var token in data)
			{
				var resObj = token.Value<JObject>();
				var res = new ProjectResponse
				{
					uid = resObj["uid"].ToString(),
					name = resObj["name"].ToString()
				};
				responses.Add(res);

				// Also add it to the config file
				config.AddProject(res.uid, res.name);
			}

			// create file with current timestamp of retrieving latest projects
			// Update timestamp
			await WriteUptime(destination, GroupName);

			// TODO see if unity has notify system
			Debug.Log("Fetching Projects completed");
		}

		private struct TableResult
		{
			public TableSO Table;
			public JToken Item;
		}

		private TableResult CreateTable(string destination, JToken item, TableMetaData tableMetadata)
		{
			return new()
			{
				Table = TableDatabase.Get.AddTable(destination, tableMetadata),
				Item = item
			};
		}

		private async Task<Int64> GetUptime(string destination)
		{
			Int64 timestamp = 0;
			if (File.Exists(destination))
			{
				var timestampRes = await File.ReadAllTextAsync(destination, Encoding.UTF8);
				timestamp = Int64.Parse(timestampRes);
			}

			return timestamp;
		}

		private async Task WriteUptime(string destination, string groupName)
		{
			var timestamp = DateTime.Now.Ticks;

			await File.WriteAllTextAsync(destination, timestamp.ToString());
			// Add the uptime file to the addressable group
			HelperClass.AddFileToAddressable(groupName, destination);
		}

		private string GetDataPath()
		{
			string dir = $"{Application.dataPath}/Data";
			if (globalSettings && !string.IsNullOrEmpty(globalSettings.DataPath))
			{
				dir = $"{globalSettings.DataPath}";
			}

			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			return dir;
		}

		private void ExportTableToAddressable(TableMetaData metadata)
		{
			string dir = GetDataPath();

			string destination = $"{dir}/{metadata.title}.asset";
			TableDatabase.Get.ExportToAddressable(destination, metadata);
		}
	}
}

// ReSharper disable once CheckNamespace
