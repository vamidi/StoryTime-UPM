﻿
#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

using StoryTime.Components.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Components
{
	using Utils.Attributes;

	using Database;
	using Database.Binary;

	public partial class CharacterStats
	{
		[SerializeField, Tooltip("Override where we should get the data from.")]
		protected bool overrideTable;

		[SerializeField, ConditionalField("overrideTable"), Tooltip("Table collection we are going to use")]
		protected StringTableCollection collection;

		public static CharacterStats ConvertRow(
			TableRow row,
#if UNITY_EDITOR
			StringTableCollection statCollection,
#endif
			CharacterStats statOverride = null
		)
		{
			CharacterStats stat = statOverride ?? new CharacterStats();

			foreach (var field in row.Fields)
			{
				if (field.Value.Data == null)
					continue;

				if (field.Key.Equals("paramId"))
				{
					stat.paramId = (uint)field.Value.Data;
#if UNITY_EDITOR
					// Only get the first dialogue.
					var paramEntryId = (stat.paramId + 1).ToString();
					if (statCollection)
						stat.statName = new LocalizedString
						{
							TableReference = statCollection.TableCollectionNameReference,
							TableEntryReference = paramEntryId
						};
					else
						Debug.LogWarning("Collection not found. Did you create any localization tables for Stats");
#endif

					var linkField = TableDatabase.Get.GetField("attributes", "alias", stat.paramId);
					if (linkField != null)
					{
						// TODO fixme
						stat.attribute = linkField.Data;
					}
				}

				if (field.Key.Equals("paramFormula"))
				{
					stat.paramFormula = (string)field.Value.Data;
				}

				if (field.Key.Equals("base"))
				{
					stat.baseValue = (float)field.Value.Data;
				}

				if (field.Key.Equals("flat"))
				{
					stat.flat = (float)field.Value.Data;
				}

				if (field.Key.Equals("rate"))
				{
					stat.rate = (float)field.Value.Data;
				}

				if (field.Key.Equals("alias"))
				{
					stat = new CharacterStats
					{
						attribute = new Attribute
						{
							alias = (AttributeType)field.Value.Data
						}
					};
				}
			}

			return stat;
		}
	}
}
