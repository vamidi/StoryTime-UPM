using System;

using UnityEngine;

namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects
{
	using StoryTime.Domains.Utilities.Types;
	using StoryTime.Domains.Utilities.Attributes;
	using StoryTime.Domains.Database.ScriptableObjects;
	using StoryTime.Domains.VisualScripting.Data.Nodes.Dialogues;
	
	[Serializable]
	public class NodeCollection : CollectionWrapper<Node> {}

	[Serializable]
	public class PropertyCollection : CollectionWrapper<ExposedProperty> {}

	public class Graphable<T> : TableBehaviour
		where T : Node
	{
		public T Start => rootNode;

		// [Header("Graph editor variables")]
		[SerializeField, ConditionalField(nameof(isGraphEnabled))]
		internal T rootNode;

		[SerializeField, ConditionalField(nameof(isGraphEnabled))]
		internal NodeCollection nodes = new();

		[SerializeField, ConditionalField(nameof(isGraphEnabled))]
		internal PropertyCollection exposedProperties = new();

		public bool isGraphEnabled = true;

		public Graphable(string name, string dropdownColumn, string linkedColumn = "", string linkedId = "",
			string linkedTable = "")
			: base(name, dropdownColumn, linkedColumn, linkedId, linkedTable) { }


	}
}
