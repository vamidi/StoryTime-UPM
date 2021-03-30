using System;
using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google.Columns;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace DatabaseSync.Localization.Plugins.JSON.Fields
{
	[Serializable]
	public class JsonField : SheetColumn, IPullKeyColumn
	{
		public const string ColumnHeader = "text";

		SharedTableData m_SharedTableData;

		public override PushFields PushFields => PushFields.Value;

		public override void PushBegin(StringTableCollection collection)
		{
			throw new NotImplementedException();
		}

		public override void PushHeader(StringTableCollection collection, out string header, out string headerNote)
		{
			header = ColumnHeader;
			headerNote = ColumnHeader;
		}

		public override void PushCellData(SharedTableData.SharedTableEntry keyEntry, IList<StringTableEntry> tableEntries, out string value, out string note)
		{
			throw new NotImplementedException();
		}

		public override void PullBegin(StringTableCollection collection)
		{
			m_SharedTableData = collection.SharedData;
		}

		public override void PullCellData(SharedTableData.SharedTableEntry keyEntry, string cellValue, string cellNote)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="cellValue"></param>
		/// <param name="cellNote"></param>
		/// <returns></returns>
		public SharedTableData.SharedTableEntry PullKey(string cellValue, string cellNote)
		{
			if (!string.IsNullOrEmpty(cellNote) && long.TryParse(cellNote, out var keyId))
			{
				var entry = m_SharedTableData.GetEntry(keyId);
				if (entry != null)
				{
					if (entry.Key != cellValue)
						m_SharedTableData.RenameKey(entry.Key, cellValue);
					return entry;
				}


				Debug.Log($"adding key {cellValue }");
				// Create a new entry with the id
				return m_SharedTableData.AddKey(cellValue, keyId);
			}

			Debug.Log(m_SharedTableData.GetEntry(cellValue));
			return m_SharedTableData.GetEntry(cellValue) ?? m_SharedTableData.AddKey(cellValue);
		}
	}
}
