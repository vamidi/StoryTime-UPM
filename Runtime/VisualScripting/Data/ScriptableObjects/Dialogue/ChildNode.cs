namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public abstract class ChildNode : Node
	{
		public Node Child
		{
			get => child;
			internal set => child = value;
		}

		public Node child;
	}
}
