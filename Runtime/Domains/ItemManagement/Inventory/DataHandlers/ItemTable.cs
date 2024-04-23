
using StoryTime.Configurations.ScriptableObjects;
// ReSharper disable once CheckNamespace
namespace StoryTime.Components.ScriptableObjects
{
	using Database.Binary;

	// ReSharper disable once InconsistentNaming
	public partial class ItemSO : IBaseTable<ItemSO>
	{
		public ItemSO ConvertRow(TableRow row, ItemSO scriptableObject = null)
		{
			ItemSO item = scriptableObject ? scriptableObject : CreateInstance<ItemSO>();

			if (row.Fields.Count == 0)
			{
				return item;
			}

			GlobalSettingsSO config = GlobalSettingsSO.GetOrCreateSettings();
			if (config)
			{
				item.ID = row.RowId;
				var entryId = (item.ID + 1).ToString();
				if (!item.ItemName.IsEmpty)
					item.ItemName.TableEntryReference = entryId;

				if (!item.Description.IsEmpty)
					item.Description.TableEntryReference = entryId;
			}

			// public ItemType ItemType => itemType;

			foreach (var field in row.Fields)
			{
				if (item is ItemRecipeSO so && field.Key.Equals("childId"))
				{
					so.ChildId = (uint) field.Value.Data;
				}

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
