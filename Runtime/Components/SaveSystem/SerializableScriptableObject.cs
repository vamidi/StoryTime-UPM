using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StoryTime.Components.ScriptableObjects
{
	public class SerializableScriptableObject : ScriptableObject
	{
		[SerializeField, HideInInspector] private string guid;
		public string Guid => guid;

#if UNITY_EDITOR
		void OnValidate()
		{
			var path = AssetDatabase.GetAssetPath(this);
			guid = AssetDatabase.AssetPathToGUID(path);
		}
#endif
	}
}
