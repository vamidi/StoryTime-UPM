using System;
using UnityEngine;
using System.Collections.Generic;
using StoryTime.ResourceManagement.Util;
using UnityEditor;

namespace StoryTime.Components.ScriptableObjects
{
	[CreateAssetMenu(fileName = "newMapData", menuName = "StoryTime/Game/Streaming/Map Data")]
	public class MapDataSO : ScriptableObject
	{
		public List<MapObject> MapObjects => mapObjects;

		[SerializeField] private List<MapObject> mapObjects = new List<MapObject>();

		public void Add(MapObject mapObject)
		{
			mapObjects.Add(mapObject);
		}

		public void Clear()
		{
			mapObjects.Clear();
		}

		public void Instantiate()
		{
			foreach (MapObject a in mapObjects)
				Instantiate(a);
		}

		public void Instantiate(MapObject mapObject)
		{
#if UNITY_EDITOR
			GameObject temp =
				HelperClass.GetAsset<GameObject>("Assets/Prefabs/Map/" + mapObject.targetName + ".prefab");

			if (temp == null)
			{
				Debug.Log("Object could not be found!");
				return;
			}

			AssignGameObject(temp, mapObject);
#else
			HelperClass.GetFileFromAddressable<GameObject>("Assets/Prefabs/Map/" + mapObject.targetName + ".prefab")
				.Completed += (gameObject) => {
				if (temp == null)
				{
					Debug.Log("Object could not be found!");
					return;
				}
				AssignGameObject(temp, mapObject);
			};
#endif
		}

		private void AssignGameObject(GameObject temp, MapObject mapObject)
		{
			temp = Instantiate(temp);
			temp.transform.position = mapObject.targetPosition;
			temp.transform.rotation = mapObject.targetRotation;
			temp.name = mapObject.targetName;
		}

		public static void RecordObjects(MapDataSO mapData, GameObject parentMap)
		{
			Transform[] objects = parentMap.GetComponentsInChildren<Transform>();
			mapData.Clear();

			foreach (Transform a in objects)
			{
				if (a.name == parentMap.name)
					continue;

				MapObject temp = new MapObject(a);
				mapData.Add(temp);
			}
		}
	}
}
