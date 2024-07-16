using System;


using UnityEditor.Localization;

using UnityEngine;

using StoryTime.Utils.Attributes;

namespace StoryTime.Components.ScriptableObjects
{
	using Database.Binary;

	// ReSharper disable once InconsistentNaming
	public partial class StorySO : IBaseTable<StorySO>
	{
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

		public StorySO ConvertRow(TableRow row, StorySO scriptableObject = null)
		{
			StorySO story = scriptableObject ? scriptableObject : CreateInstance<StorySO>();

			if (row.Fields.Count == 0)
			{
				return story;
			}

			story.ID = row.RowId;

			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("id"))
				{
					string data = (String) field.Value.Data;
					story.ID = data;
				}

				// Fetch the first dialogue we should start
				if (field.Key.Equals("childId"))
				{
					// retrieve the necessary items
					String data = (String) field.Value.Data;
					story.childId = data;
				}

				if (field.Key.Equals("parentId"))
				{
					// retrieve the necessary items
					String data = field.Value.Data;
					story.parentId = data;
				}

				if (field.Key.Equals("typeId"))
				{
					story.typeId = (StoryType) field.Value.Data;
				}
			}

			return story;
		}
	}
}
