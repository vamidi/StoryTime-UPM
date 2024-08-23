using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using UnityEngine;

namespace StoryTime.Domains.Configurations
{
	[Serializable]
	public class PathLocations
	{
		private const string ConfigPath = "Packages/com.vamidicreations.storytime/Runtime/Domains/Configurations";

		internal string GlobalSettings;
		internal string FirebaseSettings;
		internal string GameSettings;

		internal static PathLocations FetchPathLocations()
		{
			string destination = $"{ConfigPath}/fileLocations.json";
			if (!File.Exists(destination))
			{
				return new PathLocations();
			}

			JObject item = JObject.Parse(File.ReadAllText(destination));

			return new PathLocations
			{
				GlobalSettings = item["GlobalSettings"]?.ToObject<string>() ?? "",
				FirebaseSettings = item["FirebaseSettings"]?.ToObject<string>() ?? "",
				GameSettings = item["GameSettings"]?.ToObject<string>() ?? ""
			};
		}

		internal static void SavePathLocations(PathLocations locations)
		{
			string destination = $"{ConfigPath}/fileLocations.json";

			var oldLocations = FetchPathLocations();
			if (oldLocations.GlobalSettings == locations.GlobalSettings &&
			    oldLocations.FirebaseSettings == locations.FirebaseSettings &&
			    oldLocations.GameSettings == locations.GameSettings
			   )
			{
				Debug.Log("No changes found");
				return;
			}

			Debug.Log(locations);

			JObject locObject = new JObject(
				new JProperty("GlobalSettings", locations.GlobalSettings),
				new JProperty("FirebaseSettings", locations.FirebaseSettings),
				new JProperty("GameSettings", locations.GameSettings)
			);

			File.WriteAllText(destination, locObject.ToString());

			// write JSON directly to a file
			using (StreamWriter file = File.CreateText(destination))
			using (JsonTextWriter writer = new JsonTextWriter(file))
			{
				locObject.WriteTo(writer);
			}

			Debug.Log("Saving locations");
		}

		public override string ToString()
		{
			return $"Global: {GlobalSettings}, Firebase: {FirebaseSettings}, DialogueSettings: {GameSettings}";
		}
	}
}
