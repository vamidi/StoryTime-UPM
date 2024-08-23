using System;
using System.Collections.Generic;

using UnityEngine;

namespace StoryTime.Domains.VisualScripting.Data.Nodes.Dialogues
{
	using StoryTime.Domains.Narrative.Dialogues;

	[Serializable]
	public class NodeData
	{
		public string Guid;
		public Content Content;
		public Vector2 Position;
		public Type Type;
		public List<DialogueChoice> Choices;

		public override string ToString()
		{
			return $"GUID: {Guid}, text: {Content.dialogueText}, position: {Position}";
		}
	}
}
