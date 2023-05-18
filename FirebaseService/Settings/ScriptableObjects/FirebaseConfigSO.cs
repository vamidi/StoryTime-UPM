using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace StoryTime.FirebaseService.Settings
{
	using ResourceManagement;
	using Utils.Configurations;

	internal enum ApiPlatform
	{
		Firebase,
		Prisma
	}

	/// <summary>
	/// Configuration for connecting to a Google Sheet.
	/// </summary>
	public interface ITableService { }

	[CreateAssetMenu(menuName = "StoryTime/Configurations/Config File", fileName = "FirebaseConfig", order = 1)]
	// ReSharper disable once InconsistentNaming
	public class FirebaseConfigSO : ScriptableObject, ITableService
	{
		internal const string SettingsPath = "Assets/Settings/StoryTime";

		internal string Email => email;

		internal string DatabaseURL => databaseURL;

		internal ApiPlatform Platform => platform;

		internal string ProjectID => projectID;

		internal ReadOnlyDictionary<string, string> Projects
		{
			get
			{
				Dictionary<string, string> dic = new Dictionary<string, string>();
				for (int i = 0; i < projectUids.Count; i++)
				{
					dic.Add(projectUids[i], projectNames[i]);
				}

				return new ReadOnlyDictionary<string, string>(dic);
			}
		}

		public string StorageBucket => storageBucket;

		public bool Authentication => useServer;

		/// <summary>
		/// Database url to fetch data from
		/// You can retrieve it from your JSON file.
		/// TODO see if we still need this
		/// </summary>
		[SerializeField] private string email = "";

		/// <summary>
		/// Database url to fetch data from
		/// You can retrieve it from your JSON file.
		/// </summary>
		[SerializeField] private string databaseURL = "http://localhost:8080/api";

		[SerializeField] private ApiPlatform platform = ApiPlatform.Firebase;

		/// <summary>
		/// Firebase api key
		/// TODO see if we need this
		/// </summary>
		[SerializeField] private string apiKey = "";

		/// <summary>
		/// Storage bucket
		/// TODO see if we need this
		/// </summary>
		[SerializeField] private string storageBucket = "";

		/// <summary>
		/// Project we want to load the data from
		/// </summary>
		[SerializeField] private string projectID = "";

#if UNITY_EDITOR
		[SerializeField] private List<string> projectNames = new();

		[SerializeField] private List<string> projectUids = new();
#endif
		/// <summary>
		/// TODO do we need this?
		/// </summary>
		[SerializeField] public bool useServer;

#if UNITY_EDITOR

		internal void AddProject(string projectID, string name)
		{

			if (projectUids.Contains(projectID))
			{
				return;
			}

			projectUids.Add(projectID);
			projectNames.Add(name);
		}
#else
		internal static void Fetch(Action<AsyncOperationHandle<FirebaseConfigSO>> callback)
		{
			// TODO FIX me
			var configFile = HelperClass.GetFileFromAddressable<FirebaseConfigSO>("DatabaseConfig");
			configFile.Completed += callback;
		}
#endif

		internal static FirebaseConfigSO GetOrCreateSettings()
		{
			// Look up if config is already assigned.
			PathLocations locations = PathLocations.FetchPathLocations();
			string destination = $"{SettingsPath}/DatabaseConfigSO.asset";

			if (!Directory.Exists(SettingsPath))
			{
				Directory.CreateDirectory(SettingsPath);
			}

			// if we have a new location grab it
			if (!String.IsNullOrEmpty(locations.FirebaseSettings))
			{
				destination = locations.FirebaseSettings;
			}

			var settings = HelperClass.GetAsset<FirebaseConfigSO>(destination);
#if UNITY_EDITOR
			if (settings == null)
			{
				settings = HelperClass.CreateAsset<FirebaseConfigSO>(destination);
			}
#endif
			if (settings == null)
			{
				throw new ArgumentException("Settings cannot be null", nameof(settings));
			}
			return settings;
		}
#if UNITY_EDITOR
		internal static SerializedObject GetSerializedSettings() {
			return new SerializedObject(GetOrCreateSettings());
		}
#endif
	}
}



