using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace DatabaseSync.Components
{
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
	[CreateAssetMenu(fileName = "Item", menuName = "DatabaseSync/Item Management/Item", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class ItemSO : TableBehaviour
	{
		public LocalizedString ItemName { get => itemName; set => itemName = value; }
		public LocalizedString Description { get => description; set => description = value; }
		public bool Sellable { get => sellable; set => sellable = value; }
		public double SellValue { get => sellValue; set => sellValue = value; }
		public ItemTypeSO ItemType { get => itemType; set => itemType = value; }
		public Sprite PreviewImage => previewImage;
		public GameObject Prefab => prefab;
		public LocalizedSprite LocalizePreviewImage => localizePreviewImage;
		public bool IsLocalized => isLocalized;

		[SerializeField, Tooltip("The name of the item")] private LocalizedString itemName;
		[SerializeField, Tooltip("A preview image for the item")] private Sprite previewImage;
		[SerializeField, Tooltip("A description of the item")] private LocalizedString description;
		[SerializeField, Tooltip("The type of item")] private ItemTypeSO itemType;
		[SerializeField, Tooltip("A prefab reference for the model of the item")] private GameObject prefab;
		[SerializeField,Tooltip("If the player is able to sell this item")] private bool sellable;
		[SerializeField, Tooltip("If the item is sellable, how much will it cost")] private double sellValue;
		[SerializeField, Tooltip("A localized preview image for the item")] private LocalizedSprite localizePreviewImage;
		[SerializeField, Tooltip("a checkbox for localized asset")] private bool isLocalized;

		// Effect Primary Value
		// Effect Type Id

		public ItemSO(string name, string dropdownColumn, string linkedColumn = "",
			UInt32 linkedId = UInt32.MaxValue, string linkedTable = "") : base(name, dropdownColumn, linkedColumn, linkedId, linkedTable) { }

		ItemSO(): base("items", "name") {}
	}
}
