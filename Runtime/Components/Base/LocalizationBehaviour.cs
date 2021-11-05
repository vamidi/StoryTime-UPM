using System;
using UnityEditor.Localization;
using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	using Attributes;
	public class LocalizationBehaviour : TableBehaviour
	{
		/** ------------------------------ DATABASE FIELD ------------------------------ */

		[SerializeField, Tooltip("Override where we should get the data from.")]
		protected bool overrideTable;

		[SerializeField, ConditionalField("overrideTable"), Tooltip("Table collection we are going to use")]
		protected StringTableCollection collection;

		public LocalizationBehaviour(string name, string dropdownColumn, string linkedColumn = "", uint linkedId = UInt32.MaxValue, string linkedTable = "")
			: base(name, dropdownColumn, linkedColumn, linkedId, linkedTable) { }
	}
}
