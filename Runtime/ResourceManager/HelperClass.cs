using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using UnityEditor;

using UnityEngine;
using Object = UnityEngine.Object;

namespace DatabaseSync.ResourceManagement.Util
{
	using Binary;

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

		public static T GetAsset<T>(string guid) where T : Object
		{
			return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
		}

		/// <summary>
		/// Returns a list of all the json files in the directory with the id <see cref="TableId"/>.
		/// </summary>
		/// <returns>The sheets names and id's.</returns>
		public static List<(string name, string fileName)> GetDataFiles()
		{
			DatabaseConfig config = TableBinary.Fetch();

			var assetDirectory = config.dataPath;
			if (string.IsNullOrEmpty(assetDirectory)) throw new Exception("The folder could not be found!");

			// Get existing database files
			var filePaths = Directory.GetFiles(assetDirectory,"*.json");
			var files = new List<(string name, string fileName)>();

			foreach (var filePath in filePaths)
			{
				string fileName = Path.GetFileNameWithoutExtension(filePath);
				string name = Capitalize(fileName);
				files.Add((name, fileName));
			}

			return files;
		}
	}
}
