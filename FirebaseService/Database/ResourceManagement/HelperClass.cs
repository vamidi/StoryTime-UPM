using System.IO;
using System.Collections.Generic;

using UnityEngine;

namespace StoryTime.FirebaseService.Database.ResourceManagement
{
	using Configurations.ScriptableObjects;

	public static class HelperClass
	{
		/// <summary>
		/// Returns a list of all the json files in the directory with the id <see cref="TableSO"/>.
		/// </summary>
		/// <returns>The sheets names and id's.</returns>
		public static List<(string name, string fileName)> GetDataFiles(FirebaseConfigSO config, string pattern = "*.json")
		{
			var files = new List<(string name, string fileName)>();

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
					string name = Utils.HelperClass.Capitalize(fileName);
					files.Add((name, fileName));
				}
			}
			catch(DirectoryNotFoundException e) { Debug.Log(e); }

			return files;
		}
	}
}
