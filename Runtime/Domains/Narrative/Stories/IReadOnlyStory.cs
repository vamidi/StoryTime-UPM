using System.Collections.Generic;
using StoryTime.Domains.Game.Characters.ScriptableObjects;
using StoryTime.Domains.Narrative.Dialogues;
using StoryTime.Domains.Narrative.Stories.TimelineModules.Interfaces;
using UnityEngine;

namespace StoryTime.Domains.Narrative.Stories
{
	using TimelineModules;
	using StoryTime.Domains.Database;
	using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
	using StoryTime.Domains.Narrative.Tasks.ScriptableObjects;

	public interface IReadOnlyStory : IReadableTableBehaviour 
	{
		/** ------------------------------ Dialogue Details ------------------------------ */

		/// <summary>
		/// The title of the quest
		/// </summary>
		string Title { get; }
		/// <summary>
		/// The chapter of the story
		/// </summary>
		string Chapter { get; }
		/// <summary>
		/// The description of the quest
		/// </summary>
		string Description { get; }
		bool IsDone { get; }
		/// <summary>
		/// Show the type of the quest. i.e could be part of the main story
		/// </summary>
		StoryType TypeId { get; }
		/// <summary>
		/// The collection of tasks composing the Quest
		/// </summary>
		List<TaskSO> Tasks { get; }

		/** ------------------------------ DATABASE FIELD ------------------------------ */
		// TODO move to base class
		public string ParentId { get; }
		public string ChildId { get; }
		
		/** ------------------------------ SIMPLE STORY ------------------------------ */
		
		public CharacterSO Character { get; }
		public DialogueLine Dialogue { get; }
		
		/** ------------------------------ METHODS ------------------------------ */
		public void FinishStory();
		
		/** ------------------------------ NEW Story System ------------------------------ */
		
		/// <summary>
		/// The conversation that will be played in this timeline.
		/// </summary>
		public List<Module> TimeLine { get; }
		
		public void AddModule(Module module);
	}
}
