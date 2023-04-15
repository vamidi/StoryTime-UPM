#if UNITY_EDITOR
using UnityEditor;

namespace StoryTime.FirebaseService.Database
{
	public class FirebaseSyncConfig
	{
		// const string EditorPrefTabValueKey = "StoryTime-Window-Settings-Tab";

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
	}
}
#endif
