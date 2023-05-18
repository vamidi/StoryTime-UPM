using System;
using System.IO;
using UnityEngine;

using UnityEditor;

using StoryTime.Utils.Configurations;
namespace StoryTime.Configurations.ScriptableObjects
{
	using ResourceManagement;

	[CreateAssetMenu(menuName = "StoryTime/Configurations/Global Settings File", fileName = "GlobalSettings", order = 0)]
	public class GlobalSettingsSO : ScriptableObject
	{
		public const string SettingsPath = "Assets/Settings/StoryTime";

		public string DataPath => dataPath;
		public DialogueSettingConfigSO DialogueSettings => dialogueSettings;

		private const string k_DataPath = "Assets/Data";

		/// <summary>
		/// The location where to store the JSON data locally.
		/// </summary>
		[SerializeField] private string dataPath = k_DataPath;

		[SerializeField] private DialogueSettingConfigSO dialogueSettings;

#if !UNITY_EDITOR
		internal static void Fetch(Action<AsyncOperationHandle<FirebaseConfigSO>> callback)
		{
			// TODO FIX me
			var configFile = HelperClass.GetFileFromAddressable<FirebaseConfigSO>("DatabaseConfig");
			configFile.Completed += callback;
		}
#endif

		internal static GlobalSettingsSO GetOrCreateSettings()
		{
			PathLocations locations = PathLocations.FetchPathLocations();
			string destination = $"{SettingsPath}/GlobalSettingsSO.asset";

			if (!Directory.Exists(SettingsPath))
			{
				Directory.CreateDirectory(SettingsPath);
			}

			// if we have a new location grab it
			if (!String.IsNullOrEmpty(locations.GlobalSettings))
			{
				destination = locations.GlobalSettings;
			}

			var settings = HelperClass.GetAsset<GlobalSettingsSO>(destination);
#if UNITY_EDITOR
			if (settings == null)
			{
				settings = HelperClass.CreateAsset<GlobalSettingsSO>(destination);
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
