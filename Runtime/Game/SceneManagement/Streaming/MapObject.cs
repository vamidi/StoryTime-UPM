using System;
using UnityEngine;

namespace StoryTime.Components
{
	[Serializable]
	public class MapObject
	{
		public string targetName;
		public Vector3 targetPosition;
		public Quaternion targetRotation;

		public MapObject(Transform targetRef)
		{
			targetName = targetRef.name.Split('.')[0];
			targetPosition = targetRef.position;
			targetRotation = targetRef.rotation;
		}
	}
}
