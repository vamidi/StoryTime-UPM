using UnityEditor;

using StoryTime.Configurations.ScriptableObjects;
namespace StoryTime.Editor.Settings
{
	[CustomEditor(typeof(GlobalSettingsSO))]
	internal class GlobalSettingsEditor : BaseSettingsEditor<GlobalSettingsSO>
	{
		protected override GlobalSettingsSO GetSetting()
		{
			return GlobalSettingsSO.GetOrCreateSettings();
		}
	}
}
