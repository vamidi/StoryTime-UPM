using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Firebase;
using Firebase.ConfigAutoSync;
using Firebase.RemoteConfig;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StoryTime.FirebaseService
{
	using Database;
	using Configurations.ScriptableObjects;

	/// <summary>
	/// This class statically initializes the Firebase app, checking and fixing dependencies.
	/// When another class wants to get a reference to Firebase, it should use
	/// FirebaseInitializer.Initialize(callback) to ensure the app is initialized first.
	/// </summary>
	public class FirebaseInitializer
	{
		private static FirebaseApp _app;
		internal static Firebase.Database.FirebaseDatabase Database;
		internal static readonly FirebaseConfigSO DatabaseConfigSo;

		private static readonly List<Task<DependencyStatus>> InitializedCallbacks = new();

		private static readonly List<Action> ActivateFetchCallbacks = new();
		private static DependencyStatus dependencyStatus;
		private static bool initialized;
		private static bool activateFetched;

		private static bool enableRemoteConfig = false;


		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static FirebaseInitializer Get => new();

		static FirebaseInitializer()
		{
			Debug.Log("Initializing Firebase");
			DatabaseConfigSo = Fetch();
		}

		/// <summary>
		/// Invoke this with a callback to perform some action once the Firebase App is initialized.
		/// If the Firebase App is already initialized, the callback will be invoked immediately.
		/// </summary>
		/// <param name="callback">The callback to perform once initialized.</param>
		public void Initialize(Action<DependencyStatus> callback)
		{
			lock (InitializedCallbacks)
			{
				if (initialized)
				{
					callback(dependencyStatus);
					return;
				}

				FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(async task =>
				{
					// lock (InitializedCallbacks)
					{
						dependencyStatus = task.Result;
						initialized = true;

						// Add tasks
						InitializedCallbacks.Add(InitializeFirebase(dependencyStatus));
						InitializedCallbacks.Add(InvokeCallback(callback));
						await CallInitializedCallbacks();
					}
				});
			}
		}

		/// <summary>
		/// Calls FetchAsync and ActivateFetched on FirebaseRemoteConfig, initializing its values and
		/// triggering provided callbacks once.
		/// </summary>
		/// <param name="callback">Callback to schedule after RemoteConfig is initialized.</param>
		/// <param name="forceRefresh">If true, force refresh of RemoteConfig params.</param>
		public void RemoteConfigActivateFetched(Action callback, bool forceRefresh = false)
		{
			if (enableRemoteConfig)
			{
				throw new Exception("Remote config is not yet configured!");
			}

			lock (ActivateFetchCallbacks)
			{
				if (activateFetched && !forceRefresh)
				{
					callback();
					return;
				}
				else
				{
					ActivateFetchCallbacks.Add(callback);
				}

				// Get the default values from the current SyncTargets.
				var syncObjects = Resources.FindObjectsOfTypeAll<RemoteConfigSyncBehaviour>();
				var syncTargets = SyncTargetManager.FindTargets(syncObjects).GetFlattenedTargets();
				var defaultValues = new Dictionary<string, object>();
				foreach (var target in syncTargets.Values)
				{
					// defaultValues[target.FullKeyString] = target.Value;
				}

				Initialize(_ =>
				{
					var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
					remoteConfig.SetDefaultsAsync(defaultValues)
						.ContinueWith(_ =>
						{
							remoteConfig.FetchAndActivateAsync().ContinueWith(_ =>
							{
								lock (ActivateFetchCallbacks)
								{
									activateFetched = true;
									CallActivateFetchedCallbacks();
								}
							});
						});
				});
			}
		}

		internal FirebaseInitializer AddInitializeCallback(Action<DependencyStatus> callback)
		{
			InitializedCallbacks.Add(InvokeCallback(callback));
			return this;
		}

		internal static FirebaseConfigSO Fetch()
		{
#if UNITY_EDITOR
			var configFile = AssetDatabase.LoadAssetAtPath<FirebaseConfigSO>(AssetDatabase.GUIDToAssetPath(FirebaseSyncConfig.SelectedConfig));
			if (configFile == null)
				throw new ArgumentNullException($"{nameof(configFile)} must not be null.", nameof(configFile));
#else
				// TODO FIX me
				var configFile = null;
#endif
			return configFile;
		}

		private async Task CallInitializedCallbacks()
		{
			await Task.WhenAll(InitializedCallbacks);
			InitializedCallbacks.Clear();

			/*
			lock (InitializedCallbacks)
			{

				foreach (var callback in InitializedCallbacks)
				{
					callback(dependencyStatus);
				}

			}
			*/
		}

		private void CallActivateFetchedCallbacks()
		{
			lock (ActivateFetchCallbacks)
			{
				foreach (var callback in ActivateFetchCallbacks)
				{
					callback.Invoke();
				}

				ActivateFetchCallbacks.Clear();
			}
		}

		private Task<DependencyStatus> InvokeCallback(Action<DependencyStatus> callback)
		{
			callback(dependencyStatus);
			return Task.FromResult(dependencyStatus);
		}

		private Task<DependencyStatus> InitializeFirebase(DependencyStatus status)
		{
			if (status == DependencyStatus.Available)
			{
				// projectID:
				// dataPath: D:/Personal/CDRIVE/Werk/HTML/StoryTime/unity/StoryTime/Assets/Data
				AppOptions firebaseAppOptions = new AppOptions
				{
					DatabaseUrl = new Uri(DatabaseConfigSo.DatabaseURL),
					ProjectId = DatabaseConfigSo.FirebaseProjectId,
					AppId = DatabaseConfigSo.AppId,
					ApiKey = DatabaseConfigSo.ApiKey,
					MessageSenderId = DatabaseConfigSo.MessageSenderId,
					StorageBucket = DatabaseConfigSo.StorageBucket,
				};

				_app = FirebaseApp.Create(firebaseAppOptions);
				Database = Firebase.Database.FirebaseDatabase.GetInstance(_app);
			}

			return Task.FromResult(status);
		}
	}
}
