﻿using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEngine;

using UnityEngine.Networking;

using Newtonsoft.Json.Linq;
using UnityEngine.AddressableAssets;

namespace StoryTime.Editor
{
	using UI;
	using Binary;
	using Database;
	using ResourceManagement.Util;
	using Configurations.ScriptableObjects;

	[Serializable]
	class DatabaseToken
	{
		[SerializeField]
		// ReSharper disable once InconsistentNaming
		public string token_type = "";

		[SerializeField]
		// ReSharper disable once InconsistentNaming
		public string access_token = "";

		[SerializeField]
		// ReSharper disable once InconsistentNaming
		public string id_token = "";

		[SerializeField]
		// ReSharper disable once InconsistentNaming
		public string refresh_token = "";

		[SerializeField]
		// ReSharper disable once InconsistentNaming
		public int expires_in = -1;

		[SerializeField]
		// ReSharper disable once InconsistentNaming
		public long expire_time;
	}

	/// <summary>
	/// The DatabaseSync Module is in charge of making the connection to
	/// the database. Once that data is sync it will create or find the existing
	/// table scriptable object and update the data.
	/// </summary>
	public class DatabaseSyncModule
	{
		private static readonly string s_GroupName = "JSON_data";

		private static readonly string s_DataPath = "Packages/com.vamidicreations.storytime";

		private static readonly string FirebaseAppFile;

		// ReSharper disable once InconsistentNaming
		private static DatabaseToken DATABASE_TOKEN = new DatabaseToken();

		public static event EventHandler onFetchCompleted;

		private Int64 _lastTimeStamp;
		private bool _canFetch = true;

		// Check to see if we're about to be destroyed.
		// private static bool m_ShuttingDown = false;
		// private static object m_Lock = new object();
		// private static DatabaseSyncModule s_Instance;

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static DatabaseSyncModule Get => new DatabaseSyncModule();

		static DatabaseSyncModule()
		{
			Debug.Log("Starting module");

			// Load existing data
			Addressables.InitializeAsync().Completed += (result) => TableDatabase.Get.Refresh();

			// TODO this file should already exist to override it in order projects.
			FirebaseAppFile = $"{s_DataPath}/firebase-storytime-app.json";

			// First retrieve the file
			RetrieveAppFile();

			// When we retrieved the file check if the user is already logged in
			// EditorUtility.DisplayProgressBar("Simple Progress Bar", "Doing some work...", t / secs);
			// for (float t = 0; t < secs; t += step)
			// {
				// Normally, some computation happens here.
				// This example uses Sleep.
				// Thread.Sleep((int)(step * 1000.0f));
			// }
			// EditorUtility.ClearProgressBar();
			CheckLogin(() => { });
		}

		/// <summary>
		/// Check if the user is logged in
		/// </summary>
		private static void RetrieveAppFile()
		{
			// GetFileFromAddressable()

			if (!File.Exists(FirebaseAppFile))
			{
				Debug.Log("Creating Firebase-storytime-app.json");
				return;
			}

			// TODO read file from disk to get the token.
			//Read the text from directly from the test.txt file
			StreamReader reader = new StreamReader(FirebaseAppFile);
			string jsonString = reader.ReadToEnd();
			reader.Close();
			try
			{
				DATABASE_TOKEN = JsonUtility.FromJson<DatabaseToken>(jsonString);
			}
			catch (ArgumentNullException e)
			{
				Debug.Log(e.Message);
			}
		}

		private static void CheckLogin(Action callback)
		{
			// See if the token is there, if not login
			if (string.IsNullOrEmpty(DATABASE_TOKEN.id_token))
			{
				// Login if everything is empty
				Login(callback);
				return;
			}

			// see if the token is expired
			// if the time is lower that means we are expired
			// Debug.Log(DATABASE_TOKEN.expire_time < DateTime.Now.Ticks);
			if (DATABASE_TOKEN.expire_time < DateTime.Now.Ticks)
			{
				DatabaseConfigSO configFile = Fetch();

				UnityWebRequest wr = UnityWebRequest.Get(configFile.DatabaseURL + "me");

				wr.timeout = 60;
				wr.SetRequestHeader("User-Agent", "X-Unity3D-Agent");
				wr.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
				wr.SetRequestHeader("Authorization", "Bearer " + DATABASE_TOKEN.id_token);

				wr.SendWebRequest().completed += operation =>
				{
					if (wr.responseCode == 401)
					{
						// Log the user in again
						Refresh(callback);
					}
				};

				return;
			}

			// We can call the callback immediately
			callback();
		}

		// First try to login
		private static void Login(Action callback)
		{
			DatabaseConfigSO configFile = Fetch();

			if (string.IsNullOrEmpty(configFile.Email) || string.IsNullOrEmpty(configFile.Password))
			{
				Debug.Log("Database configuration are not configured!");
				return;
			}

			Debug.Log("Login in to retrieve token");

			var form = new Dictionary<string, string>
			{
				{ "email", configFile.Email },
				{ "password", configFile.Password }
			};

			UnityWebRequest wr = UnityWebRequest.Post($"{configFile.DatabaseURL}authenticate", form);

			wr.timeout = 60;
			wr.SetRequestHeader("User-Agent", "X-Unity3D-Agent");
			wr.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
			wr.SendWebRequest().completed += operation =>
			{
				if (wr.result == UnityWebRequest.Result.ConnectionError || wr.responseCode == 401 || wr.responseCode == 500)
					throw new ArgumentException("Error: ", wr.error);

				// Handle result
				string str = wr.downloadHandler.text;

				try
				{
					DATABASE_TOKEN = JsonUtility.FromJson<DatabaseToken>(str);
					// add the 3600 seconds for the next iteration
					var exp = DateTime.Now;
					exp = exp.AddSeconds(DATABASE_TOKEN.expires_in);
					DATABASE_TOKEN.expire_time += exp.Ticks;
					File.WriteAllText(FirebaseAppFile, JsonUtility.ToJson(DATABASE_TOKEN));
				}
				catch (ArgumentNullException e)
				{
					throw new ArgumentException(e.Message);
				}

				callback();
			};
		}

		private static void Refresh(Action callback)
		{
			DatabaseConfigSO configFile = Fetch();

			if (string.IsNullOrEmpty(configFile.Email) || string.IsNullOrEmpty(configFile.Password))
			{
				Debug.Log("Database configuration are not configured!");
				return;
			}

			UnityWebRequest wr = UnityWebRequest.Get($"{configFile.DatabaseURL}refresh");

			wr.timeout = 60;
			wr.SetRequestHeader("User-Agent", "X-Unity3D-Agent");
			wr.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
			wr.SetRequestHeader("Authorization", "Bearer " + DATABASE_TOKEN.refresh_token);
			wr.SendWebRequest().completed += operation =>
			{
				if (wr.result == UnityWebRequest.Result.ConnectionError || wr.responseCode == 401 || wr.responseCode == 500)
				{
					Debug.Log("Error: " + wr.error);
					return;
				}

				// Handle result
				string str = wr.downloadHandler.text;
				try
				{
					DATABASE_TOKEN = JsonUtility.FromJson<DatabaseToken>(str);
					// add the 3600 seconds for the next iteration
					var exp = DateTime.Now;
					exp = exp.AddSeconds(DATABASE_TOKEN.expires_in);
					DATABASE_TOKEN.expire_time += exp.Ticks;
					File.WriteAllText(FirebaseAppFile, JsonUtility.ToJson(DATABASE_TOKEN));
				}
				catch (ArgumentNullException e)
				{
					Debug.Log(e.Message);
				}

				callback();
			};
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

			// This is the url on which to process the request
			// TODO make timestamp work
			// string page = string.IsNullOrEmpty(table) ? "?tstamp=" : "tables/" + table + "?tstamp=";
			// check if we are logged in and then fetch data
			CheckLogin(() =>
			{
				// TODO add regular expression for the product id in order to prevent errors.
				DatabaseConfigSO configFile = Fetch();
				UnityWebRequest wr = UnityWebRequest.Get($"{configFile.DatabaseURL}firebase/projects/{configFile.ProjectID}/tables/{tableID}");
				wr.timeout = 60;
				wr.SetRequestHeader("User-Agent", "X-Unity3D-Agent");
				wr.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
				wr.SetRequestHeader("Authorization", "Bearer " + DATABASE_TOKEN.id_token);
				wr.SendWebRequest().completed += operation => OnResponseReceived(wr, tableID);
			});
		}

		private void OnResponseReceived(UnityWebRequest request, string tableID = "")
		{
			// FetchNotification->SetCompletionState(bWasSuccessful? SNotificationItem::CS_Success : SNotificationItem::CS_Fail);
			// FetchNotification->ExpireAndFadeout();
			if (request.result == UnityWebRequest.Result.ConnectionError || request.responseCode == 401 || request.responseCode == 500)
			{
				Debug.LogError("Error: " + request.error);
				Debug.LogError("Error: " + request.downloadHandler.text);
				_canFetch = true;
				return;
			}

			// Debug.Log("Received: " + request.downloadHandler.text);

			// Handle result
			string str = request.downloadHandler.text;
			bool hasTable = tableID != "";

			// Create a pointer to hold the json serialized data
			if (hasTable)
			{
				var jsonToken = JToken.Parse(str);

				if (jsonToken["id"] == null || jsonToken["projectID"] == null || jsonToken["metadata"] == null)
					throw new ArgumentException("Can't make Table from JSON file");

				// Get the TableSO from the TableDatabase.
				TableMetaData tableMetadata = jsonToken["metadata"].ToObject<TableMetaData>();
				TableDatabase.Get.AddTable(jsonToken, tableMetadata);
			}
			else
			{
				var jsonArray = JArray.Parse(str);
				// now we can get the values from json of any attribute.
				foreach (var item in jsonArray.Children())
				{
					if (item["id"] == null || item["projectID"] == null || item["metadata"] == null)
						throw new ArgumentException("Can't make Table from JSON file");

					TableMetaData tableMetadata = item["metadata"].ToObject<TableMetaData>();
					// Export to addressable is handle in the Table itself --> SOLID PATTERN
					TableDatabase.Get.AddTable(item, tableMetadata);
				}
			}

#if UNITY_EDITOR // TODO see if unity has notify system
				// Update editor
				// auto& PropertyModule = FModuleManager::LoadModuleChecked< FPropertyEditorModule >("PropertyEditor");
				// PropertyModule.NotifyCustomizationModuleChanged();
#endif
			DatabaseConfigSO config = Fetch();
			if (config && !string.IsNullOrEmpty(config.dataPath))
			{
				// Update timestamp
				_lastTimeStamp = DateTime.Now.Ticks;

				string destination = $"{config.dataPath}/uptime.txt";
				File.WriteAllText(destination, _lastTimeStamp.ToString());
				// Add the uptime file to the addressable group
				HelperClass.AddFileToAddressable(s_GroupName, destination);
			}

			Debug.Log("Invoking fetch");
			onFetchCompleted?.Invoke(this, EventArgs.Empty);
			Debug.Log("Fetching complete");
			_canFetch = true;
		}

		protected static DatabaseConfigSO Fetch()
		{
			var configFile = AssetDatabase.LoadAssetAtPath<DatabaseConfigSO>(AssetDatabase.GUIDToAssetPath(DatabaseSyncWindow.SelectedConfig));
			if (configFile == null)
				throw new ArgumentNullException($"{nameof(configFile)} must not be null.", nameof(configFile));

			return configFile;
		}
	}
}
