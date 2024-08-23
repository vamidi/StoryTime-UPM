using UnityEditor;
using UnityEngine.Localization.Settings;

namespace StoryTime.Editor.Localization.Utilities
{
	public static class EditorUtilities
	{
		[InitializeOnLoadMethod]
		private static void Init() => EditorApplication.update += SetLocaleWhenReady;

		private static void SetLocaleWhenReady()
		{
			if (LocalizationSettings.Instance == null)
				return;
			EditorApplication.update -= SetLocaleWhenReady;

			if (LocalizationSettings.SelectedLocale == null)
				LocalizationSettings.SelectedLocale = LocalizationSettings.ProjectLocale;
		}
	}
}
