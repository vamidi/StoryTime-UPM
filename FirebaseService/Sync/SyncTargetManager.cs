using System.Collections.Generic;

namespace Firebase.ConfigAutoSync
{
	/// <summary>
	/// This static class finds all the SyncItems available.
	/// Objects with valid targets are GameObjects in the current scene, and Prefabs which have the
	/// RemoteConfigSyncBehaviour attached & enabled.
	/// Sync targets are fields discovered on other Components attached to the object, or potentially
	/// nested objects within those components. Which fields are valid SyncTargets depends on the
	/// values of the attached RemoteConfigSyncBehaviour.
	/// </summary>
	public static class SyncTargetManager
	{
		/// <summary>
		/// Find all the valid SyncTargets from the provided RemoteConfigSyncBehaviour instances.
		/// </summary>
		/// <param name="syncObjects">Scene and Prefab objects from which to find targets.</param>
		/// <returns>Top-level SyncTargetContainer ancestor of all discovered targets.</returns>
		public static SyncTargetContainer FindTargets(
			IEnumerable<RemoteConfigSyncBehaviour> syncObjects) {
			// _syncTargets = new SyncTargetContainer();
			// foundTargets.Clear();
			// foreach (var syncBehaviour in syncObjects) {
				// AddNewSyncContainer(syncBehaviour);
			// }

			// return _syncTargets;
			return new SyncTargetContainer();
		}
	}
}
