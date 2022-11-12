using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public abstract class Node : ScriptableObject
	{
		public string guid;
		public Vector2 position;
	}
}
