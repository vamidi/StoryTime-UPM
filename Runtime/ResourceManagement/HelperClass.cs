using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
#endif

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace StoryTime.ResourceManagement
{
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
			var path = destination;
#if UNITY_EDITOR
			if (convert)
			{
				path = MakePathRelative(destination);
			}

			// var guid = AssetDatabase.AssetPathToGUID(path);
			return AssetDatabase.LoadAssetAtPath<T>(path);
#else
			return null;
#endif
		}

		public static string GetAssetPath<T>(T obj) where T : Object
		{
			var path = AssetDatabase.GetAssetPath(obj);
			if (File.Exists(path))
			{
				path = Path.GetDirectoryName(path);
			}

			return path;
		}

		public static T[] FindAssetsByType<T>() where T : Object
		{
#if UNITY_EDITOR
			return AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)))
				.Select(p =>
				{
					string assetPath = AssetDatabase.GUIDToAssetPath(p);
					return AssetDatabase.LoadAssetAtPath<T>(assetPath);
				}).ToArray();
#else
			return [];
#endif
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

		// public static AsyncOperationHandle<IList<T>> GetFilesFromAddressable<T>(IList<IResourceLocation> locations)
		// {
			// return Addressables.LoadAssetsAsync<T>(locations);
		// }

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
