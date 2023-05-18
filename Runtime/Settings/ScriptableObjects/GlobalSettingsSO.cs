using System;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

using StoryTime.Utils.Configurations;
namespace StoryTime.Configurations.ScriptableObjects
{
	using ResourceManagement;

	[CreateAssetMenu(menuName = "StoryTime/Configurations/Global Settings File", fileName = "GlobalSettings", order = 0)]
	public class GlobalSettingsSO : ScriptableObject
	{
		public const string SettingsPath = "Assets/Settings/StoryTime";

		public string DataPath => dataPath;
		public GameSettingConfigSO GameSettings => gameSettings;

		private const string k_DataPath = "Assets/Data";

		/// <summary>
		/// The location where to store the JSON data locally.
		/// </summary>
		[SerializeField] private string dataPath = k_DataPath;

		[SerializeField] private GameSettingConfigSO gameSettings;

#if !UNITY_EDITOR
		internal static void Fetch(Action<AsyncOperationHandle<FirebaseConfigSO>> callback)
		{
			// TODO FIX me
			var configFile = HelperClass.GetFileFromAddressable<FirebaseConfigSO>("DatabaseConfig");
			configFile.Completed += callback;
		}
#endif

#if UNITY_EDITOR
		internal virtual AddressableAssetSettings GetAddressableAssetSettings(bool create)
		{
			var settings = AddressableAssetSettingsDefaultObject.GetSettings(create);
			if (settings != null)
				return settings;

			// By default Addressables wont return the settings if updating or compiling. This causes issues for us, especially if we are trying to get the Locales.
			// We will just ignore this state and try to get the settings regardless.
			if (EditorApplication.isUpdating || EditorApplication.isCompiling)
			{
				// Legacy support
				if (EditorBuildSettings.TryGetConfigObject(AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, out settings))
				{
					return settings;
				}

				AddressableAssetSettingsDefaultObject so;
				if (EditorBuildSettings.TryGetConfigObject(AddressableAssetSettingsDefaultObject.kDefaultConfigObjectName, out so))
				{
					// Extract the guid
					var serializedObject = new SerializedObject(so);
					var guid = serializedObject.FindProperty("m_AddressableAssetSettingsGuid")?.stringValue;
					if (!string.IsNullOrEmpty(guid))
					{
						var path = AssetDatabase.GUIDToAssetPath(guid);
						return AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(path);
					}
				}
			}
			return null;
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
