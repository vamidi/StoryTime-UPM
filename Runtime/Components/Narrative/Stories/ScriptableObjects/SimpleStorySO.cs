using System;

using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	using VisualScripting.Data;

	public enum StoryType
	{
		All,
		WorldQuests,
		SideQuests,
		// Extend with own quest types.
	}

	public enum DialogueType
	{
		StartDialogue,
		WinDialogue,
		LoseDialogue,
		DefaultDialogue
	}

	public enum ChoiceActionType
	{
		DoNothing,
		ContinueWithQuest,
		ContinueWithTask,
	}

	[CreateAssetMenu(fileName = "newSimpleStory", menuName = "StoryTime/Stories/Simple Story", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class SimpleStorySO : LocalizationBehaviour
	{
		public CharacterSO Character => character;

		public DialogueLine Dialogue => dialogue;

		public bool IsDone => m_IsDone;

		/** ------------------------------ DATABASE FIELD ------------------------------ */

		public uint ParentId => parentId;
		public uint ChildId => childId;

		[SerializeField] protected CharacterSO character;

		// Is being calculated in the story editor.
		[SerializeField] protected DialogueLine dialogue;

		/** ------------------------------ DATABASE FIELD ------------------------------ */

		[SerializeField, HideInInspector] // Tooltip("The character id where this story belongs to.")]
		protected uint parentId = UInt32.MaxValue;

		[SerializeField] // Tooltip("The id where the dialogue should go first")]
		protected uint childId = UInt32.MaxValue;

		// ReSharper disable once InconsistentNaming
		protected bool m_IsDone;

		public SimpleStorySO() : base("stories", "title", "parentId") { }

		public override void Reset()
		{
			base.Reset();

			childId = UInt32.MaxValue;

			Debug.Log(childId);
		}
	}
}
