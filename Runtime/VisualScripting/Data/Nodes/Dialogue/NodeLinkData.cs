using System;

namespace StoryTime.VisualScripting.Data
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

