
namespace StoryTime.Components.ScriptableObjects
{
	using FirebaseService.Database.Binary;

	public partial class ItemRecipeSO
	{
		public class RecipeCraftTable
		{
			public static T ConvertRow<T>(TableRow row, T scriptableObject = null) where T : ItemSO
			{
				return ItemTable.ConvertRow(row, scriptableObject);
			}
		}
	}
}
