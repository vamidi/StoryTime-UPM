using StoryTime.Attributes;
using UnityEngine;

namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects
{
	public abstract class Node : ScriptableObject
	{
		[ReadOnly]
		public string guid;
		public Vector2 position;
	}
}
