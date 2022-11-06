using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

namespace StoryTime.Configurations.ScriptableObjects
{
	/// <summary>
	/// Configuration for connecting to a Google Sheet.
	/// </summary>
	public interface ITableService { }

	[CreateAssetMenu(menuName = "StoryTime/Configurations/Config File", fileName = "FirebaseConfig")]
	public class FirebaseConfigSO : ScriptableObject, ITableService
	{
		public string Email => email;

		internal string Password => password;

		public string DatabaseURL => databaseURL;
		internal string ProjectID => projectID;

		public string FirebaseProjectId => firebaseProjectId;

		internal string AppId => appId;

		internal string ApiKey => apiKey;

		public string MessageSenderId => messageSenderId;

		public string StorageBucket => storageBucket;

		public bool Authentication => useServer;

		/// <summary>
		/// Database url to fetch data from
		/// You can retrieve it from your JSON file.
		/// </summary>
		[SerializeField] private string email = "";

		/// <summary>
		/// Database url to fetch data from
		/// You can retrieve it from your JSON file.
		/// </summary>
		[SerializeField] private string password = "";

		/// <summary>
		/// Database url to fetch data from
		/// You can retrieve it from your JSON file.
		/// </summary>
		[SerializeField] private string databaseURL = "";

		/// <summary>
		/// Firebase project id.
		/// You can retrieve the id from the json file.
		/// </summary>
		[SerializeField] private string firebaseProjectId = "";

		/// <summary>
		/// Firebase app id.
		/// </summary>
		[SerializeField] private string appId = "";

		/// <summary>
		/// Firebase api key
		/// </summary>
		[SerializeField] private string apiKey = "";

		/// <summary>
		/// Message sender id
		/// </summary>
		[SerializeField] private string messageSenderId = "";

		/// <summary>
		/// Storage bucket
		/// </summary>
		[SerializeField] private string storageBucket = "";

		/// <summary>
		/// Project we want to load the data from
		/// </summary>
		[SerializeField] private string projectID = "";

		/// <summary>
		/// The location where to store the JSON data locally.
		/// </summary>
		[SerializeField] public string dataPath = "";

		/// <summary>
		///
		/// </summary>
		[SerializeField] public bool useServer;
	}
}
