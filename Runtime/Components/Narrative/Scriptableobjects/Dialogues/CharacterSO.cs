using System;
using DatabaseSync.Game;
using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.Localization;

using UnityEditor.Localization;

namespace DatabaseSync.Components
{
	using Binary;
	using Database;

	/// <summary>
	/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
	/// </summary>
	[CreateAssetMenu(fileName = "newCharatcer", menuName = "DatabaseSync/Stories/Character")]
	// ReSharper disable once InconsistentNaming
	public partial class CharacterSO : LocalizationBehaviour
	{
		public LocalizedString CharacterName => characterName;
		public CharacterStatSO CharacterStats => characterStats;

		[SerializeField] private LocalizedString characterName;

		// TODO see if we need to make an list out of this.
		[SerializeField, Tooltip("All the stats the character currently has.")] private CharacterStatSO characterStats;
		protected override void OnTableIDChanged()
		{
			base.OnTableIDChanged();
			Initialize();
		}

		public void OnEnable()
		{
#if UNITY_EDITOR
			Initialize();
#endif
		}

		CharacterSO() : base("characters", "name") { }

		private void Initialize()
		{
			if (ID != UInt32.MaxValue)
			{
				var entryId = (ID + 1).ToString();

				collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Characters");

				if(collection)
					characterName = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables");
			}
		}
	}
}
