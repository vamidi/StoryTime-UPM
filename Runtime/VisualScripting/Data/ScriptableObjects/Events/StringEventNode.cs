using System;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public class StringEventNode : EventNode<String>
	{
		public override string ToString()
		{
			return $"Event {eventName} and value {value}";
		}
	}
}
