using UnityEditor;

namespace StoryTime.Editor.Domains.UI.Inspectors
{
    using StoryTime.Domains.Inspectors;

    [CustomEditor(typeof(InspectorHelpMessage))]
    public class InspectorHelpMessageEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox((target as InspectorHelpMessage)?.message, MessageType.Info);
        }
    }
}
