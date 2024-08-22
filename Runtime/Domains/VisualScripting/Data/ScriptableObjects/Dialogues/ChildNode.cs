namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Dialogues
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
