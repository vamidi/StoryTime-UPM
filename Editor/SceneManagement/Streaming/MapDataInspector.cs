using UnityEditor;
using UnityEngine;

namespace StoryTime.Editor
{
	using StoryTime.Components;
	using StoryTime.Components.ScriptableObjects;

	[CustomEditor(typeof(MapDataSO)), CanEditMultipleObjects]
	public class MapDataInspector : UnityEditor.Editor
	{
		public GameObject parentMap;
		private MapDataSO mapData;
		private void OnEnable()
		{
			mapData = (MapDataSO)target;
		}

		public override void OnInspectorGUI()
		{
			parentMap = EditorGUILayout.ObjectField(parentMap, typeof(GameObject), true) as GameObject;

			EditorUtility.SetDirty(target);
			EditorGUILayout.LabelField("Number of Objects : " + mapData.MapObjects.Count.ToString());

			if (GUILayout.Button("Record Map Objects"))
			{
				foreach (Object o in targets)
				{
					mapData = (MapDataSO)o;
					MapDataSO.RecordObjects(mapData, parentMap);
				}
			}

			if (GUILayout.Button("Instantiate Map Objects"))
			{
				foreach (Object o in targets)
				{
					mapData = (MapDataSO)o;
					foreach (MapObject a in mapData.MapObjects)
					{
						mapData.Instantiate(a);
					}
				}

			}
		}
	}
}
