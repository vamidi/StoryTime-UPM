using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
#endif

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace StoryTime.ResourceManagement.Util
{
	using Database;
	using Configurations.ScriptableObjects;

	public static class HelperClass
	{
		public static void Trace()
		{
			Debug.Log(System.Environment.StackTrace);
		}

		public static List<T> RepeatedDefault<T>(int count)
		{
			return Repeated(default(T), count);
		}

		public static List<T> Repeated<T>(T value, int count)
		{
			List<T> ret = new List<T>(count);
			ret.AddRange(Enumerable.Repeat(value, count));
			return ret;
		}

		public static Match GetActionRegex(string pattern, string text, int startAt = 0)
		{
			// split the whole text into parts based off the <> tags
			// even numbers in the array are text, odd numbers are tags
			// <action=>
			// first grab the value from the regular expression
			Regex r = new Regex(pattern, RegexOptions.Singleline);
			return r.Match(text, startAt);
		}

		public static string MakePathRelative(string path)
		{
			if (path.Contains(Application.dataPath))
			{
				var length = Application.dataPath.Length - "Assets".Length;
				return path.Substring(length, path.Length - length);
			}

			return path;
		}

		public static string Capitalize(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}

			s = s.ToLower();
			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);

			return new string(a);
		}

		public static void CreateAsset<T>(T item, string destination) where T: ScriptableObject
		{
#if UNITY_EDITOR
			var relativePath = MakePathRelative(destination);
			AssetDatabase.CreateAsset(item, relativePath);
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
#endif
		}

		public static T GetAsset<T>(string destination, bool convert = false) where T : Object
		{
			var guid = destination;
#if UNITY_EDITOR
			if (convert)
			{
				var relativePath = MakePathRelative(destination);
				guid = relativePath;
			}
			return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
#else
			return null;
#endif
		}

		public static List<T> FindAssetsByType<T>() where T : Object
		{
			List<T> assets = new List<T>();
#if UNITY_EDITOR
			string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
			for (int i = 0; i < guids.Length; i++)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
				T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
				if (asset != null)
				{
					assets.Add(asset);
				}
			}
#endif
			return assets;
		}

		public static void AddFileToAddressable(string groupName, string destination)
		{
#if UNITY_EDITOR
			var relativePath = MakePathRelative(destination);
			var settings = GetAddressableSettings();
			var guid = AssetDatabase.AssetPathToGUID(relativePath);
			AddAssetToGroup(guid, settings.FindGroup(groupName));
#endif
		}

		public static AsyncOperationHandle<T> GetFileFromAddressable<T>(string address)
		{
			return Addressables.LoadAssetAsync<T>(address);
		}

		/// <summary>
		/// Returns a list of all the json files in the directory with the id <see cref="TableID"/>.
		/// </summary>
		/// <returns>The sheets names and id's.</returns>
		public static List<(string name, string fileName)> GetDataFiles(string pattern = "*.json")
		{
			var files = new List<(string name, string fileName)>();

			DatabaseConfigSO config = TableDatabase.Fetch();
			if (config != null)
			{
				var assetDirectory = config.dataPath;
				if (string.IsNullOrEmpty(assetDirectory))
				{
					Debug.LogWarning("Data directory in config could not be found!");
					return files;
				}

				try
				{
					// Get existing database files
					var filePaths = Directory.GetFiles(assetDirectory, pattern);

					foreach (var filePath in filePaths)
					{
						string fileName = Path.GetFileNameWithoutExtension(filePath);
						string name = Capitalize(fileName);
						files.Add((name, fileName));
					}
				}
				catch(DirectoryNotFoundException e) { Debug.Log(e); }
			}

			return files;
		}

		private static AddressableAssetSettings GetAddressableSettings()
		{
#if UNITY_EDITOR
			return UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
#else
			return null;
#endif
		}

#if UNITY_EDITOR
		private static void AddAssetToGroup(string guid, AddressableAssetGroup group)
		{
			var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
			// var group = settings.FindGroup("Group Name");
			settings.CreateOrMoveEntry(guid, group);
		}
#endif
	}
}
