
namespace StoryTime.Components.ScriptableObjects
{
	using Database.Binary;

	public partial class ItemRecipeSO
	{
		public ItemRecipeSO ConvertRow(TableRow row, ItemRecipeSO scriptableObject = null)
		{
			ItemRecipeSO item = scriptableObject ? scriptableObject : CreateInstance<ItemRecipeSO>();

			return base.ConvertRow(row, item) as ItemRecipeSO;
		}
	}
}
