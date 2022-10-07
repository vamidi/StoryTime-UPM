using System;
using System.Collections.Generic;

using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;
using UnityEditor.Localization.Plugins.Google.Columns;

using UnityEngine;

namespace StoryTime.Editor.Localization.Plugins.JSON
{
	using Fields;
	using Configurations.ScriptableObjects;

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
		public List<SheetColumn> Fields => fields;

		public string TableName => tableName;

		public string TableId => tableId;

		public bool RemoveMissingPulledKeys => removeMissingPulledKeys;

		[SerializeReference] private List<SheetColumn> fields = new List<SheetColumn>();

		[SerializeField] private FirebaseConfigSO jsonServiceProvider;

		[SerializeField] private string tableName = String.Empty;

		[SerializeField] private string tableId = String.Empty;

		[SerializeReference] private bool removeMissingPulledKeys = true;
	}
}
