using UnityEditor;
using UnityEngine;

namespace StoryTime.FirebaseService.Database.Editor
{
	/// <summary>
	/// The DatabaseSync Module is in charge of making the connection to
	/// the database. Once that data is sync it will create or find the existing
	/// table scriptable object and update the data.
	/// </summary>
	[InitializeOnLoad]
	public class DatabaseSyncEditor
	{
		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static DatabaseSyncEditor Get => new();

		static DatabaseSyncEditor()
		{
#if UNITY_EDITOR
			DatabaseSyncModule.Get.Initialize();
#endif
		}
	}
}
