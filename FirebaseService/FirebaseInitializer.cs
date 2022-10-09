using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Firebase;
using Firebase.ConfigAutoSync;
using Firebase.RemoteConfig;

using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITY_EDITOR
using UnityEditor;
using StoryTime.ResourceManagement;
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
		private static Firebase.Auth.FirebaseUser _user;

		internal static Firebase.Auth.FirebaseAuth Auth;
		internal static Firebase.Database.FirebaseDatabase Database;
		internal static
#if UNITY_EDITOR
			readonly
#endif
			FirebaseConfigSO DatabaseConfigSo;

		internal static bool signedIn;

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
#if UNITY_EDITOR
			// DatabaseConfigSo = Fetch();
#endif
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

#if !UNITY_EDITOR
				Fetch((op) =>
				{
					if (!op.Result)
					{
						throw new Exception("Config file could not be found!");
					}

					DatabaseConfigSo = op.Result;
#endif
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
#if !UNITY_EDITOR
				});
#endif
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

				ActivateFetchCallbacks.Add(callback);

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

#if UNITY_EDITOR
		internal static FirebaseConfigSO Fetch()
		{
			var configFile = AssetDatabase.LoadAssetAtPath<FirebaseConfigSO>(AssetDatabase.GUIDToAssetPath(FirebaseSyncConfig.SelectedConfig));
			if (configFile == null)
			{
				Debug.LogWarning($"{nameof(configFile)} must not be null.");
			}

			return configFile;
		}
#else
		internal static void Fetch(Action<AsyncOperationHandle<FirebaseConfigSO>> callback)
		{
			// TODO FIX me
			var configFile = HelperClass.GetFileFromAddressable<FirebaseConfigSO>("DatabaseConfig");
			configFile.Completed += callback;
		}
#endif

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
				Auth = Firebase.Auth.FirebaseAuth.GetAuth(_app);
				Auth.StateChanged += AuthStateChanged;

				Auth.SignInWithEmailAndPasswordAsync(DatabaseConfigSo.Email, DatabaseConfigSo.Password).ContinueWith(task => {
					if (task.IsCanceled) {
						Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
						return;
					}

					if (task.IsFaulted) {
						Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
						return;
					}

					Firebase.Auth.FirebaseUser newUser = task.Result;

					Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
				});
			}

			return Task.FromResult(status);
		}

		private void AuthStateChanged(object sender, EventArgs eventArgs) {
			Debug.Log("Auth changed");
			if (Auth.CurrentUser != _user) {
				signedIn = _user != Auth.CurrentUser && Auth.CurrentUser != null;
				if (!signedIn && _user != null) {
					Debug.Log("Signed out " + _user.UserId);
				}
				_user = Auth.CurrentUser;
			}
		}
	}
}
