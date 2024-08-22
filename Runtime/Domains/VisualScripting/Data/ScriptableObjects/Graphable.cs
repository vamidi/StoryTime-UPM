using System;

using UnityEngine;

using StoryTime.Utils.Types;
using StoryTime.Utils.Attributes;
using StoryTime.Database.ScriptableObjects;
using StoryTime.Domains.VisualScripting.Data.Nodes.Dialogues;

namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects
{
	[Serializable]
	public class NodeCollection : CollectionWrapper<Node> {}

	[Serializable]
	public class PropertyCollection : CollectionWrapper<ExposedProperty> {}

	public class Graphable<T> : LocalizationBehaviour
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
