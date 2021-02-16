using UnityEngine;

namespace DatabaseSync
{
	[CreateAssetMenu(menuName = "DatabaseSync/Configurations/Config File", fileName = "DatabaseConfig")]
	public class DatabaseConfig : ScriptableObject
	{
		[SerializeField]
		public string databaseURL = "";

		[SerializeField]
		public string projectID = "";

		[SerializeField]
		public string email = "";

		[SerializeField]
		public string password = "";
	}
}
