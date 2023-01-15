using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace StoryTime.Components.ScriptableObjects
{
	/// <summary>
	/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
	/// In future versions it might contain support for branching conversations.
	/// </summary>
	[CreateAssetMenu(fileName = "newStory", menuName = "StoryTime/Game/Narrative/Story", order = 51)]
	// ReSharper disable once InconsistentNaming
	public partial class StorySO : SimpleStorySO
	{
		public LocalizedString Title => title;
		public LocalizedString Description => description;

		public bool IsDone => m_IsDone;

		public StoryType TypeId => typeId;
		public List<TaskSO> Tasks => tasks;

		/** ------------------------------ DATABASE FIELD ------------------------------ */

		public uint ParentId => parentId;
		public uint ChildId => childId;

		/** ------------------------------ DATABASE FIELD ------------------------------ */

		[SerializeField, HideInInspector] // Tooltip("The character id where this story belongs to.")]
		protected uint parentId = UInt32.MaxValue;

		[SerializeField, HideInInspector] // Tooltip("The id where the dialogue should go first")]
		protected uint childId = UInt32.MaxValue;

		// ReSharper disable once InconsistentNaming
		protected bool m_IsDone;

		[Header("Dialogue Details")]
		[SerializeField, Tooltip("The collection of tasks composing the Quest")] private List<TaskSO> tasks = new ();
		[SerializeField, HideInInspector, Tooltip("The title of the quest")] private LocalizedString title;
		[SerializeField, HideInInspector, Tooltip("The description of the quest")] private LocalizedString description;
		[SerializeField, Tooltip("Show the type of the quest. i.e could be part of the main story")] private StoryType typeId = StoryType.WorldQuests;

		protected override void OnTableIDChanged()
		{
			base.OnTableIDChanged();
			Initialize();
		}

		public virtual void OnEnable()
		{
#if UNITY_EDITOR
			Initialize();
#endif
		}

		public override void Reset()
		{
			base.Reset();

			childId = UInt32.MaxValue;
		}

		public void FinishStory()
		{
			m_IsDone = true;
		}

		private void Initialize()
		{
			if (ID != UInt32.MaxValue && childId != UInt32.MaxValue)
			{
				collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Story Titles");
				// Only get the first dialogue.
				var entryId = (ID + 1).ToString();
				if(collection)
					title = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables for Stories");

				storyDescriptionCollection = overrideStoryDescriptionsTable ? storyDescriptionCollection : LocalizationEditorSettings.GetStringTableCollection("Story Descriptions");
				if (storyDescriptionCollection)
					description = new LocalizedString { TableReference = storyDescriptionCollection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables");
			}
		}
	}
}

