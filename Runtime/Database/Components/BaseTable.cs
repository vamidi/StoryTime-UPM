using StoryTime.Database.Binary;

interface IBaseTable<T> where T : StoryTime.Database.ScriptableObjects.TableBehaviour
{
	T ConvertRow(TableRow row, T scriptableObject = null);
	// throw new ArgumentException("Row can't be converted. Make a new class that inherits from this class");
}
