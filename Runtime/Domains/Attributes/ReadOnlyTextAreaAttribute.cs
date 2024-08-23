using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StoryTime.Domains.Attributes
{
	/// <summary>
	/// Read Only attribute.
	/// Attribute is use only to mark ReadOnly properties.
	/// </summary>
	public class ReadOnlyTextAreaAttribute : PropertyAttribute
	{
		/// <summary>
		///   <para>The minimum amount of lines the text area will use.</para>
		/// </summary>
		public readonly int minLines;
		/// <summary>
		///   <para>The maximum amount of lines the text area can show before it starts using a scrollbar.</para>
		/// </summary>
		public readonly int maxLines;

		/// <summary>
		///   <para>Attribute to make a string be edited with a height-flexible and scrollable text area.</para>
		/// </summary>
		/// <param name="minLines">The minimum amount of lines the text area will use.</param>
		/// <param name="maxLines">The maximum amount of lines the text area can show before it starts using a scrollbar.</param>
		public ReadOnlyTextAreaAttribute()
		{
			this.minLines = 3;
			this.maxLines = 3;
		}

		/// <summary>
		///   <para>Attribute to make a string be edited with a height-flexible and scrollable text area.</para>
		/// </summary>
		/// <param name="minLines">The minimum amount of lines the text area will use.</param>
		/// <param name="maxLines">The maximum amount of lines the text area can show before it starts using a scrollbar.</param>
		public ReadOnlyTextAreaAttribute(int minLines, int maxLines)
		{
			this.minLines = minLines;
			this.maxLines = maxLines;
		}

	}

#if UNITY_EDITOR
	/// <summary>
	/// This class contain custom drawer for ReadOnly attribute.
	/// </summary>
	[CustomPropertyDrawer(typeof(ReadOnlyTextAreaAttribute))]
	public class ReadOnlyTextAreaDrawer : PropertyDrawer
	{
		private float textHeight;

		/// <summary>
		/// Unity method for drawing GUI in Editor
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="property">Property.</param>
		/// <param name="label">Label.</param>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Saving previous GUI enabled value
			var previousGUIState = GUI.enabled;
			// Disabling edit for property
			GUI.enabled = false;
			// Drawing Property
			// Custom style
			GUIStyle myStyle = new GUIStyle (EditorStyles.textArea)
			{
				fontSize = 14
			};

			property.stringValue = EditorGUI.TextArea (position, property.stringValue, myStyle);

			// Text height
			GUIContent guiContent = new GUIContent (property.stringValue);
			textHeight = myStyle.CalcHeight (guiContent, EditorGUIUtility.currentViewWidth);
			// Setting old GUI enabled value
			GUI.enabled = previousGUIState;
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return textHeight;
		}
	}
#endif
}
