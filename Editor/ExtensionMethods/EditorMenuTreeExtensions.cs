
namespace StoryTime.Editor.Extensions
{
	public static class EditorMenuTreeExtensions
	{
		internal static void SplitMenuPath(string menuPath, out string path, out string name)
		{
			menuPath = menuPath.Trim('/');
			int length = menuPath.LastIndexOf('/');
			if (length == -1)
			{
				path = "";
				name = menuPath;
			}
			else
			{
				path = menuPath.Substring(0, length);
				name = menuPath.Substring(length + 1);
			}
		}
	}
}
