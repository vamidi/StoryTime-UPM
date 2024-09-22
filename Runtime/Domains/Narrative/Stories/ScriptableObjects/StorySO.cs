using System.Collections.Generic;
using StoryTime.Domains.Narrative.Stories.TimelineModules;
using UnityEngine;

#if UNITY_EDITOR
using StoryTime.Attributes;
#endif

namespace StoryTime.Domains.Narrative.Stories.ScriptableObjects
{
	using StoryTime.Domains.Narrative.Tasks.ScriptableObjects;

	/// <summary>
	/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
	/// In future versions it might contain support for branching conversations.
	/// </summary>
	[CreateAssetMenu(fileName = "ExampleStory", menuName = "StoryTime/Game/Narrative/Story", order = 51)]
	// ReSharper disable once InconsistentNaming
    public sealed class StorySO : SimpleStorySO, IReadOnlyStory
    {
        public string Title => title;
        public string Chapter => chapter;
        public string Description => description;
        public bool IsDone => m_IsDone;
        public StoryType TypeId => typeId;
        public List<TaskSO> Tasks => tasks;
        public List<Module> TimeLine { get; }

		/** ------------------------------ DATABASE FIELD ------------------------------ */

		public string ParentId => parentId;
		public string ChildId => childId;

		/** ------------------------------ DATABASE FIELD ------------------------------ */

		[SerializeField, ReadOnly] // Tooltip("The character id where this story belongs to.")]
		protected string parentId;

		[SerializeField, ReadOnly] // Tooltip("The id where the dialogue should go first")]
		protected string childId;

		// ReSharper disable once InconsistentNaming
		protected bool m_IsDone;

		[Header("Dialogue Details")]
		[SerializeField, Tooltip("The collection of tasks composing the Quest")] private List<TaskSO> tasks = new ();
		[SerializeField, Tooltip("The title of the quest")] private string title;
		[SerializeField, Tooltip("The chapter of the story")] private string chapter;
		[SerializeField, Tooltip("The description of the quest")] private string description;
		[SerializeField, Tooltip("Show the type of the quest. i.e could be part of the main story")] private StoryType typeId = StoryType.WorldQuests;
		
		public override void Reset()
		{
			base.Reset();
#if UNITY_EDITOR
			childId = System.Guid.NewGuid().ToString();
#endif
		}

		public void FinishStory()
		{
			m_IsDone = true;
		}

		public void AddModule(Module module)
		{
			TimeLine.Add(module);
		}
    }
}
