using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using StoryTime.Attributes;
using UnityEditor.Localization;
using StoryTime.Domains.Utilities.Attributes;
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
    public class StorySO : SimpleStorySO
    {
		/// <summary cref="StorySO.TitleLocalized"></summary>
	    [Obsolete("Use the TitleLocalized property instead")]
        public LocalizedString Title => title;
        
        // TODO rename to Title
        public virtual string TitleLocalized => title.GetLocalizedString();
		
		public LocalizedString Chapter => chapter;
		
		/// <summary cref="StorySO.DescriptionLocalized"></summary>
		[Obsolete("Use the DescriptionLocalized property instead")]
		public LocalizedString Description => description;
		
		public virtual string DescriptionLocalized => description.GetLocalizedString();

		public bool IsDone => m_IsDone;

		public virtual StoryType TypeId => typeId;
		public virtual List<TaskSO> Tasks => tasks;

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
		[SerializeField, Tooltip("The title of the quest")] private LocalizedString title;
		[SerializeField, Tooltip("The chapter of the story")] private LocalizedString chapter;
		[SerializeField, Tooltip("The description of the quest")] private LocalizedString description;
		[SerializeField, Tooltip("Show the type of the quest. i.e could be part of the main story")] private StoryType typeId = StoryType.WorldQuests;

		[SerializeField, Tooltip("Override where we should get the dialogue options data from.")]
		protected bool overrideDialogueOptionsTable;

#if UNITY_EDITOR
		[SerializeField, ConditionalField("overrideDialogueOptionsTable"), Tooltip("Table collection we are going to use for the sentence")]
		protected StringTableCollection dialogueOptionsCollection;
#endif

		[SerializeField, Tooltip("Override where we should get the character data from.")]
		protected bool overrideCharacterTable;

#if UNITY_EDITOR
		[SerializeField, ConditionalField("overrideTable"), Tooltip("Table collection we are going to use")]
		protected StringTableCollection characterCollection;
#endif

		[SerializeField, Tooltip("Override where we should get the dialogue options data from.")]
		protected bool overrideStoryDescriptionsTable;

		[SerializeField, ConditionalField("overrideStoryDescriptionsTable"), Tooltip("Table collection we are going to use for the sentence")]
		protected StringTableCollection storyDescriptionCollection;
		
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
#if UNITY_EDITOR
			childId = System.Guid.NewGuid().ToString();
#endif
		}

		public void FinishStory()
		{
			m_IsDone = true;
		}

		private void Initialize()
		{
			if (ID != "" && childId != "")
			{
				collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Story Titles");
				// Only get the first dialogue.
				var entryId = (ID + 1);
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
