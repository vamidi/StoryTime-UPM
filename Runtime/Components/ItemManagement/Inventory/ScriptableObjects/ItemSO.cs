using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace StoryTime.Components.ScriptableObjects
{
	using Attributes;

	/// <summary>
    /// Called whenever a localized string is available.
    /// When the first <see cref="LocalizedAsset{TObject}.ChangeHandler"/> is added, a loading operation will automatically start and the localized string value will be sent to the event when completed.
    /// Any adding additional subscribers added after loading has completed will also be sent the latest localized string value when they are added.
    /// This ensures that a subscriber will always have the correct localized value regardless of when it was added.
    /// The string will be refreshed whenever <see cref="LocalizationSettings.SelectedLocaleChanged"/> is changed and when <see cref="RefreshString"/> is called.
    /// <seealso cref="GetLocalizedString"/> when not using the event.
    /// </summary>
    /// <example>
    /// This example shows how we can fetch and update a single string value.
    /// <code>
    /// public class LocalizedStringWithChangeHandlerExample : MonoBehaviour
    /// {
    ///     // A LocalizedString provides an interface to retrieving translated strings.
    ///     // This example assumes a String Table Collection with the name "My String Table" and an entry with the Key "Hello World" exists.
    ///     // You can change the Table Collection and Entry target in the inspector.
    ///     public LocalizedString stringRef = new LocalizedString() { TableReference = "My String Table", TableEntryReference = "Hello World" };
    ///     string m_TranslatedString;
    ///
    ///     void OnEnable()
    ///     {
    ///         stringRef.StringChanged += UpdateString;
    ///     }
    ///
    ///     void OnDisable()
    ///     {
    ///         stringRef.StringChanged -= UpdateString;
    ///     }
    ///
    ///     void UpdateString(string translatedValue)
    ///     {
    ///         m_TranslatedString = translatedValue;
    ///         Debug.Log("Translated Value Updated: " + translatedValue);
    ///     }
    ///
    ///     void OnGUI()
    ///     {
    ///         GUILayout.Label(m_TranslatedString);
    ///     }
    /// }
    /// </code>
    /// </example>
	[CreateAssetMenu(fileName = "Item", menuName = "StoryTime/Item Management/Item", order = 51)]
	// ReSharper disable once InconsistentNaming
	public partial class ItemSO : LocalizationBehaviour
	{
		public LocalizedString ItemName { get => itemName; set => itemName = value; }
		public LocalizedString Description { get => description; set => description = value; }
		public bool Sellable { get => sellable; set => sellable = value; }
		public double SellValue { get => sellValue; set => sellValue = value; }
		public ItemTypeSO ItemType { get => itemType; set => itemType = value; }
		public Sprite PreviewImage => previewImage;
		public GameObject Prefab => prefab;
		public List<StatModifier> StatModifiers => statModifiers;
		public LocalizedSprite LocalizePreviewImage => localizePreviewImage;
		public bool IsLocalized => isLocalized;

		[SerializeField, Tooltip("Override where we should get the item description data from.")]
		protected bool overrideDescriptionTable;

		[SerializeField, ConditionalField("overrideDescriptionTable"), Tooltip("Table collection we are going to use for the sentence")]
		protected StringTableCollection itemDescriptionCollection;

		[SerializeField, HideInInspector, Tooltip("The name of the item")] protected LocalizedString itemName;
		[SerializeField, HideInInspector, Tooltip("A description of the item")] protected LocalizedString description;
		[SerializeField, Tooltip("A preview image for the item")] protected Sprite previewImage;
		[SerializeField, Tooltip("The type of item")] protected ItemTypeSO itemType;
		[SerializeField, Tooltip("A prefab reference for the model of the item")] protected GameObject prefab;
		[SerializeField, Tooltip("If the player is able to sell this item")] protected bool sellable;
		[SerializeField, Tooltip("If the item is sellable, how much will it cost")] protected double sellValue;
		[SerializeField, Tooltip("Stat modifiers")] protected List<StatModifier> statModifiers;
		[SerializeField, Tooltip("A localized preview image for the item")] protected LocalizedSprite localizePreviewImage;
		[SerializeField, Tooltip("a checkbox for localized asset")] protected bool isLocalized;

		// Effect Primary Value
		// Effect Type Id

		public ItemSO(string name, string dropdownColumn, string linkedColumn = "",
			UInt32 linkedId = UInt32.MaxValue, string linkedTable = "") : base(name, dropdownColumn, linkedColumn, linkedId, linkedTable) { }

		ItemSO(): base("items", "name") {}

		protected override void OnTableIDChanged()
		{
			base.OnTableIDChanged();
			Initialize();
		}

		public virtual void OnEnable()
		{
			Initialize();
		}

		protected virtual void Initialize()
		{
			var entryId = (ID + 1).ToString();
			collection = LocalizationEditorSettings.GetStringTableCollection("Item Names");
			if (collection != null)
				itemName = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
			else
				Debug.LogWarning("Collection not found. Did you create any localization tables for Items");

			var descriptionCollection = overrideDescriptionTable ? itemDescriptionCollection : LocalizationEditorSettings.GetStringTableCollection("Item Descriptions");
			if (descriptionCollection != null)
				description = new LocalizedString { TableReference = descriptionCollection.TableCollectionNameReference, TableEntryReference = entryId };
			else
				Debug.LogWarning("Collection not found. Did you create any localization tables for Items");
		}
	}
}
