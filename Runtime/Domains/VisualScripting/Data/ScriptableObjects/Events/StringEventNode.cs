using System;

namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Events
{
	public class StringEventNode : EventNode<String>
	{
		public override string ToString()
		{
			return $"Event {eventName} and value {value}";
		}
	}
}
