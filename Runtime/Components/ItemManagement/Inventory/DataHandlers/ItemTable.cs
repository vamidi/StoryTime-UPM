
using StoryTime.FirebaseService;

namespace StoryTime.Components.ScriptableObjects
{
	using FirebaseService.Database.Binary;
	using Configurations.ScriptableObjects;

	// ReSharper disable once InconsistentNaming
	public partial class ItemSO
	{
		public class ItemTable : BaseTable<ItemSO>
		{
			public static T ConvertRow<T>(TableRow row, T scriptableObject = null) where T : ItemSO
			{
				T item = scriptableObject ? scriptableObject : CreateInstance<T>();

				if (row.Fields.Count == 0)
				{
					return item;
				}

				FirebaseConfigSO config = FirebaseConfigSO.FindSettings();
				if (config != null)
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
}
