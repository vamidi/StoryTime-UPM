using UnityEditor;

using StoryTime.FirebaseService.Settings;
namespace StoryTime.Editor.Settings
{
	[CustomEditor(typeof(FirebaseConfigSO))]
	public class FirebaseConfigEditor : BaseSettingsEditor<FirebaseConfigSO>
	{
		protected override FirebaseConfigSO GetSetting()
		{
			return FirebaseConfigSO.GetOrCreateSettings();
		}
	}
}
