namespace StoryTime.Domains.Database
{
	using StoryTime.Domains.Database.Binary;
	using StoryTime.Domains.Database.ScriptableObjects;

	interface IBaseTable<T> where T : TableBehaviour
	{
		T ConvertRow(TableRow row, T scriptableObject = null);
		// throw new ArgumentException("Row can't be converted. Make a new class that inherits from this class");
	}
}

