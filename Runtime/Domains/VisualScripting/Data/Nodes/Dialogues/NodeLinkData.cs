using System;

namespace StoryTime.Domains.VisualScripting.Data.Nodes.Dialogues
{
	[Serializable]
	public class NodeLinkData
	{
		public string BaseNodeGuid;
		public string PortName;
		public string TargetNodeGuid;

		public override string ToString()
		{
			return $"GUID: {BaseNodeGuid}, name: {PortName}, target: {TargetNodeGuid}";
		}
	}
}

