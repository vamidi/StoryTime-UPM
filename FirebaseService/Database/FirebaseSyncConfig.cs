#if UNITY_EDITOR
using UnityEditor;

namespace StoryTime.FirebaseService.Database
{
	public class FirebaseSyncConfig
	{
		// const string EditorPrefTabValueKey = "StoryTime-Window-Settings-Tab";
		const string EditorPrefConfigValueKey = "StoryTime-Window-Settings-Config";
		const string EditorPrefDialogueConfigValueKey = "StoryTime-Window-Dialogue-Settings-Config";

		// static readonly Vector2 MinSize = new Vector2(450, 600);
		// List<ToolbarToggle> m_TabToggles;
		// List<VisualElement> m_TabPanels;
		// List<VisualElement> m_TableRows;

		/*
		public bool IsDocked
		{
			get
		   {
				BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
				MethodInfo method = GetType().GetProperty( "docked", flags ).GetGetMethod( true );
				return (bool)method.Invoke( this, null );
			}
		}

	    internal int SelectedTab
	    {
	        get => EditorPrefs.GetInt(EditorPrefTabValueKey, 0);
	        set => EditorPrefs.SetInt(EditorPrefTabValueKey, value);
	    }
		*/

		public static string SelectedConfig
		{
			get => EditorPrefs.GetString(EditorPrefConfigValueKey, "");
			internal set => EditorPrefs.SetString(EditorPrefConfigValueKey, value);
		}

		public static string SelectedDialogueConfig
		{
			get => EditorPrefs.GetString(EditorPrefDialogueConfigValueKey, "");
			internal set => EditorPrefs.SetString(EditorPrefDialogueConfigValueKey, value);
		}
	}
}
#endif
