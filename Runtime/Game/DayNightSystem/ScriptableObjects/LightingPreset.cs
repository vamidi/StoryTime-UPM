using UnityEngine;

namespace StoryTime.Components.UI
{
	[CreateAssetMenu(fileName = "Lighting Preset", menuName = "StoryTime/Game/Lighting Preset", order = 0)]
	public class LightingPreset : UnityEngine.ScriptableObject
	{
		public Gradient AmbientColor => ambientColor;
		public Gradient DirectionalColor => directionalColor;
		public Gradient FogColor => fogColor;

		[SerializeField] protected Gradient ambientColor;
		[SerializeField] protected Gradient directionalColor;
		[SerializeField] protected Gradient fogColor;
	}
}
