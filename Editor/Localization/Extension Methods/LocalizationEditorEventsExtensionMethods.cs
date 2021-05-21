
namespace UnityEditor.Localization
{
	public static class LocalizationEditorEventsExtensionMethods
	{

		/// <summary>
		/// TODO wait for update on the package to notify the tables that the collection has been updated.
		/// </summary>
		/// <param name="events"></param>
		/// <param name="sender"></param>
		/// <param name="collection"></param>
		public static void RaiseCollectionMod(this LocalizationEditorEvents events, object sender, LocalizationTableCollection  collection)
		{
			// events.RaiseCollectionModified(sender, collection);
		}
	}
}
