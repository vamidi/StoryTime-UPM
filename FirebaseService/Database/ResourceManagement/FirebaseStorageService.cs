using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;

using Firebase.Storage;
using Firebase.Extensions;

using StoryTime.Configurations.ScriptableObjects;

namespace StoryTime.FirebaseService.Database.ResourceManagement
{
	public enum NbLocationFileType {
		Default,
		Story,
		Craftable
	}


	public class FirebaseStorageService
	{
		private FirebaseStorage _storage;
		private Firebase.Database.FirebaseDatabase _database;

		// Path to the storage bucket
		private string projectID = "";
		private readonly string BASE_STORAGE_PATH = "node-editor/projects";

		public void Initialize(FirebaseStorage storage, Firebase.Database.FirebaseDatabase database, FirebaseConfigSO config)
		{
			_storage = storage;
			_database = database;
			projectID = config.ProjectID;
			// StorageReference pathReference =
			// Storage.GetReference($"node-editor/projects/{DatabaseConfigSo.ProjectID}");

			// StorageReference listRef = pathReference.Child("Stories");
		}

		public Task<List<T>> GetFiles<T>(string path)
			where T : FileUpload
		{
			if (_database == null)
			{
				return new Task<List<T>>(() => new List<T>());
			}

			return _database.GetReference(path)
				.OrderByChild("metadata/projectID")
				.EqualTo($"{projectID}")
				.LimitToLast(6)
				.GetValueAsync()
				.ContinueWithOnMainThread(task =>
				{
					var list = new List<T>();

					if (task.IsFaulted)
					{
						Debug.Log("Error looking for stories");
						return list;
					}

					Firebase.Database.DataSnapshot snapshot = task.Result;
					var tablesStr = snapshot.GetRawJsonValue();
					var jsonToken = JObject.Parse(tablesStr);

					// Task[] tasks = new Task[jsonToken.Count];
					// int idx = 0;
					foreach (var token in jsonToken)
					{
						T fileUpload = token.Value.ToObject<T>();

						list.Add(fileUpload);

					}
					return list;
				});

		}

		/// <summary>
		/// Upload file to Firebase
		/// <param name="path"></param>
		/// <param name="location"></param>
		/// <param name="fileUpload"></param>
		/// <param name="metadata"></param>
		/// <returns>Up</returns>
		/// </summary>
		public void PushFileToStorage(
			string path, FileUpload fileUpload,
			NbLocationFileType location = NbLocationFileType.Default,
			StorageMetadata metadata = null
		)
		{
			// string filePath = $"{GetRoot()}/${path}/${fileUpload.file.name}";
			string filePath = "";
			StorageReference storageRef = _storage.GetReference(filePath);

			// Start uploading a file
			var progress = new StorageProgress<UploadState>(state =>
			{
				// called periodically during the upload
				Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.",
					state.BytesTransferred, state.TotalByteCount));
			});

			// Upload the file to the path "images/rivers.jpg"
			storageRef.PutFileAsync(filePath, null, progress, CancellationToken.None)
				.ContinueWith((Task<StorageMetadata> task) =>
				{
					if (task.IsFaulted || task.IsCanceled)
					{
						Debug.Log(task.Exception.ToString());
						// Uh-oh, an error occurred!
					}
					else
					{
						// Metadata contains file metadata such as size, content-type, and download URL.
						StorageMetadata metadata = task.Result;
						string md5Hash = metadata.Md5Hash;
						Debug.Log("Finished uploading...");
						Debug.Log("md5 hash = " + md5Hash);
					}

					/*.ContinueWith(urlTask =>
					{
					var downloadURL = urlTask.Result;
					fileUpload.url = downloadURL;
					fileUpload.name = fileUpload.file.name;
					if (fileUpload.hasOwnProperty('id'))
						this.updateFileData(location, fileUpload).then();
					else
						this.saveFileData(location, fileUpload).then((ref) => fileUpload.id = ref.key);
					});;
					*/
				});
		}

		private string GetRoot()
		{
			if (projectID == String.Empty)
			{
#if UNITY_EDITOR
				Debug.LogWarning("Did you set the project?");
#else
		        throw new ArgumentException("Did you set the project?");
#endif
			}

			// UtilsService.onAssert(this.project, `Did you set the project?`);
			return $"{BASE_STORAGE_PATH}/${projectID}";
		}

		public class File
		{
			public string name;
		}

		public class Metadata
		{
			public string name;
			public string projectID;
		}

		public abstract class FileUpload
		{
			public string id;
			public string name;	// filename + extension
			public string url;
			public File file;
			public Metadata metadata;
			string data; // JSON data of the story
			// Proxy
			bool deleted;
			int created_at;
			int updated_at;
		}


		public class StoryFileUpload : FileUpload
		{
			public UInt32 storyId = UInt32.MaxValue;
		}

		public class CraftableFileUpload : FileUpload
		{
			UInt32 itemId = UInt32.MaxValue;
		}
	}
}
