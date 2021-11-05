using UnityEngine;

namespace StoryTime.Configurations.ScriptableObjects
{
	/// <summary>
	/// Configuration for connecting to a Google Sheet.
	/// </summary>
	public interface ITableService { }

	[CreateAssetMenu(menuName = "StoryTime/Configurations/Config File", fileName = "DatabaseConfig")]
	public class DatabaseConfigSO : ScriptableObject, ITableService
	{
		public string DatabaseURL => databaseURL;
		public string ProjectID => projectID;
		public string Email => email;
		public string Password => password;
		public bool Authentication => useServer;

		[SerializeField] private string databaseURL = "";

		[SerializeField] private string projectID = "";

		[SerializeField] private string email = "";

		[SerializeField] private string password = "";

		[SerializeField] public string dataPath = "";

		[SerializeField] public bool useServer;
	}
}
