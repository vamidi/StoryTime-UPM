using UnityEngine;

namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Events
{
	public abstract class EventNode : Node
	{
		public Node Child
		{
			get => child;
			internal set => child = value;
		}

		public string EventName
		{
			get => eventName;
			internal set => eventName = value;
		}


		[SerializeField] private Node child;

		[Tooltip("Dialogue Option Event name")]
		[SerializeField] protected string eventName;
	}

	public abstract class EventNode<T> : EventNode
	{
		public T Value
		{
			get => value;
			internal set => this.value = value;
		}

		// Dialogue Option Event value you want to pass
		// Extend class for more values.
		[SerializeField] protected T value;
	}
}
