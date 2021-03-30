using UnityEngine;

using UnityEditor.Localization;

namespace DatabaseSync
{
	/// <summary>
	/// Configuration for connecting to a Google Sheet.
	/// </summary>
	public interface ITableService { }

	[CreateAssetMenu(menuName = "DatabaseSync/Configurations/Config File", fileName = "DatabaseConfig")]
	public class DatabaseConfig : ScriptableObject, ITableService
	{
		public string DatabaseURL => databaseURL;
		public string ProjectID => projectID;
		public string Email => email;
		public string Password => password;
		public bool Authentication => useServer;

		public StringTableCollection DialogueCollection { get => dialoguecollection; set => dialoguecollection = value; }
		public StringTableCollection DialogueOptionCollection { get => dialogueOptionCollection; set => dialogueOptionCollection = value; }

		[SerializeField] private string databaseURL = "";

		[SerializeField] private string projectID = "";

		[SerializeField] private string email = "";

		[SerializeField] private string password = "";

		[SerializeField] public string dataPath = "";

		[SerializeField] public bool useServer = false;

		// TableCollection settings
		[SerializeField, Tooltip("Collection where we need to fetch the dialogue from")]
		private StringTableCollection dialoguecollection;

		[SerializeField, Tooltip("Collection where we need to fetch the dialogue options from")]
		private StringTableCollection dialogueOptionCollection;
	}
}
