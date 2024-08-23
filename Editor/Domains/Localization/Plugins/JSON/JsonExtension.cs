using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StoryTime.Domains.Settings.ScriptableObjects;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;
using UnityEditor.Localization.Plugins.Google.Columns;

using UnityEngine;

namespace StoryTime.Editor.Domains.Localization.Plugins.JSON
{
	using Fields;
	
	/// <summary>
    /// <see cref="StringTableCollection"/> that provides an editor interface to <see cref="GoogleSheets"/>.
    /// </summary>
    [Serializable]
    [StringTableCollectionExtension]
	public class JsonExtension : CollectionExtension
	{
		/// <summary>
		/// The column mappings. Each <see cref="JsonField"/> represents a column in a Google sheet. The column mappings are responsible for converting to and from cell data.
		/// </summary>
		// public ReadOnlyCollection<SheetColumn> Fields => fields.AsReadOnly();

		public ReadOnlyCollection<SheetColumn> Fields
		{
			get => fields.AsReadOnly();
			internal set => fields = value.ToList();
		}

		public string TableName
		{
			get => tableName;
			internal set => tableName = value;
		}

		public string TableId
		{
			get => tableId;
			internal set => tableId = value;
		}

		public bool RemoveMissingPulledKeys => removeMissingPulledKeys;

		[SerializeReference] private List<SheetColumn> fields = new ();

		[SerializeField] private StoryTimeSettingsSO jsonServiceProvider;

		[SerializeField] private string tableName = String.Empty;

		[SerializeField] private string tableId = String.Empty;

		[SerializeReference] private bool removeMissingPulledKeys = true;

		public override void Initialize()
		{
			base.Initialize();
			jsonServiceProvider = ScriptableObject.CreateInstance<StoryTimeSettingsSO>();
		}

		public void AddField(SheetColumn column)
		{
			fields.Add(column);
		}
	}
}
