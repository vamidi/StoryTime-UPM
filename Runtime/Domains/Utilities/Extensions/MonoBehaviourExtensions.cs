using UnityEngine;

namespace StoryTime.Domains.Utilities.Extensions
{
	public static class MonoBehaviourExtensions
	{
		public static float GetAngleFromVectorFloat(this MonoBehaviour behaviour, Vector3 dir) {
			dir = dir.normalized;
			float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			if (n < 0) n += 360;

			return n;
		}
	}

}
