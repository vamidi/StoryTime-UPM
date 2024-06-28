using System;
using System.Collections.Generic;

using StoryTime.Database;
using StoryTime.Database.ScriptableObjects;
namespace StoryTime.Editor.UI
{
	/// <summary>
	/// This class is created to generate an list for
	/// the ID property in the TableBehaviour class.
	/// <see cref="TableBehaviour"/>
	/// </summary>
	public static class BaseEditorList
	{
		private static TableDatabase _tableDatabase;

		public static void GenerateList(ref Dictionary<String, String> populatedList, TableBehaviour target, out bool isJsonObj)
		{
			isJsonObj = false;
			if (target != null)
			{
				if(_tableDatabase == null) _tableDatabase = TableDatabase.Get;

				var binary = _tableDatabase.GetBinary(target.Name);
				string linkColumn = target.LinkedColumn;
				String linkId = target.LinkedID;
				bool linkTable = target.LinkedTable != String.Empty;

				// retrieve the column we need to show
				if (binary != null)
				{
					populatedList = linkColumn != "" && (linkTable || linkId != String.Empty) ? binary.PopulateWithLink(
						target.DropdownColumn,
						linkColumn,
						linkId,
						out isJsonObj,
						target.LinkedTable
					) : binary.Populate(target.DropdownColumn, out isJsonObj);
				}
			}
		}
	}
}
