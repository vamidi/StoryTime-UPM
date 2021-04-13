using UnityEngine;

namespace DatabaseSync.Components
{
	using Binary;
	public class ItemTable : BaseTable<ItemSO>
	{
		public new static ItemSO ConvertRow(TableRow row, ItemSO scriptableObject = null)
		{
			ItemSO item = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<ItemSO>();

			if (row.Fields.Count == 0)
			{
				return item;
			}

			DatabaseConfig config = TableBinary.Fetch();
			if (config != null)
			{
				item.ID = row.RowId;
				var entryId = (item.ID + 1).ToString();
				if(!item.ItemName.IsEmpty)
					item.ItemName.TableEntryReference = entryId;

				if(!item.Description.IsEmpty)
					item.Description.TableEntryReference = entryId;
			}

			// public ItemType ItemType => itemType;

			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("sellValue"))
				{
					item.SellValue = (double) field.Value.Data;
				}

				if (field.Key.Equals("sellable"))
				{
					item.Sellable = (bool) field.Value.Data;
				}

				// TODO keep reference
				// if (field.Key.Equals("itemType"))
				// {
					// item.ItemType = (uint) field.Value.Data
				// }
			}

			return item;
		}

	}
}
