using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

using UnityEngine;
using UnityEngine.Localization;

using StoryTime.Domains.Game.Characters;
using StoryTime.Domains.Game.Characters.ScriptableObjects;
using StoryTime.Domains.ItemManagement.Equipment.ScriptableObjects;

namespace StoryTime.Editor.Wizards
{
	public class CharacterWizard : BaseWizard<CharacterWizard, CharacterSO>
	{
		[Header("Character Settings")]
		[SerializeField, Tooltip("Name of the character")] protected new string name = "";
		[SerializeField, Tooltip("Description of the character")] protected string description = "";
		[SerializeField, Tooltip("Equipment of the character")] protected List<EquipmentSO> equipments = new ();

		// TODO Are these this Global settings?
		[SerializeField, Tooltip("Initial Level the character starts in")] protected int initialLevel = 1;
		[SerializeField, Tooltip("Max level of the player")] protected int maxLevel = 99;

		// Character class
		[Header("Class Settings")]
		[SerializeField, Tooltip("Equipment of the character")] protected LocalizedString className = new ();
		[SerializeField, Tooltip("Equipment of the character")] protected List<SkillSO> skills = new ();
		[SerializeField, Tooltip("Equipment of the character")] protected List<CharacterStats> stats = new ();
		[SerializeField, Tooltip("Exp curve"), ReadOnly(true)] protected AnimationCurve expCurve = new ();

		public override void OnWizardUpdate()
		{
			helpString = "Create a new Character scriptable object";

			/*
			var collection = LocalizationEditorSettings.GetStringTableCollection("Character Names");
			if (collection)
			{
				name.TableReference = collection.TableCollectionNameReference;
				name.TableReference = "1_Satorou";
				description.TableReference = collection.TableCollectionNameReference;
			}
			*/

			expCurve = new();
			for (int i = initialLevel + 1; i < maxLevel; i++)
			{
				float expNeeded = (float)((i - 1) * 156 + (i - 1) * (i - 1) * (i - 1) * 1.265 - 3);

				// As long we are withing the keys of the exp curve
				expCurve.AddKey(Mathf.Ceil(expNeeded), expNeeded);
			}

			base.OnWizardUpdate();
		}

		protected override CharacterSO Create(string location)
		{
			var character = CreateInstance<CharacterSO>();
			character.CharacterName = name;
			character.Description = description;

			// Add the equipments
			foreach (var equipment in equipments)
			{
				character.Equip(equipment);
			}

			character.Level = initialLevel;
			character.MaxLevel = maxLevel;

			// expCurve;
			// (level - 1) * 156 + (level - 1) * (level - 1) * (level - 1) * 1.265 - 3
			// 1 * 156 + 1 * 1 * 1 * 1.265 - 3;
			// expCurve.
			var characterClass = CreateInstance<CharacterClassSO>();
			characterClass.ClassName = className;

			foreach (var skill in skills)
			{
				characterClass.AddSkill(skill);
			}

			// Set skills
			var folderPath = Path.GetDirectoryName(location);
			string withoutExtension = Path.GetFileNameWithoutExtension(location);

			AssetDatabase.CreateAsset(characterClass, $"{folderPath}/{withoutExtension}_{className.GetLocalizedString()}.asset");
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
			character.CharacterClass = characterClass;


			return character;
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		protected override bool Validate()
		{
			if (name == String.Empty)
			{
				errorString = "Please fill in a name";
				return false;
			}

			if(className.IsEmpty)
			{
				errorString = "Please fill in a class name";
				return false;
			}

			errorString = "";
			return true;
		}
	}
}
