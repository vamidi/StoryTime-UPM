using StoryTime.Editor.VisualScripting.Elements;
using UnityEngine.UIElements;

namespace StoryTime.Editor.VisualScripting
{
	public class InspectorView : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

		private UnityEditor.Editor _editor;

		public void UpdateSelection(NodeView nodeView)
		{
			Clear();

			UnityEngine.Object.DestroyImmediate(_editor);
			_editor = UnityEditor.Editor.CreateEditor(nodeView.node);
			IMGUIContainer container = new IMGUIContainer(() =>
			{
				if (_editor.target)
				{
					_editor.OnInspectorGUI();
				}
			});
			Add(container);
		}
	}
}
