using System;
using UnityEngine;

namespace StoryTime.Domains.Game.DayNightSystem
{
	using StoryTime.Domains.Game.DayNightSystem.ScriptableObjects;

	[ExecuteAlways]
	public class LightingManager : MonoBehaviour
	{
		[SerializeField] private Light directionalLight;
		[SerializeField] private LightingPreset preset;

		[SerializeField, Range(0, 24)] private float timeOfDay;

		private const int HOURS_DAY = 24;
		private const float FULL_DAY = 360f;
		private const float QUARTER_DAY = 90f;
		private const float ROT_Y = 90f;

		private void OnValidate()
		{
			if (directionalLight != null)
				return;

			if (RenderSettings.sun != null)
				directionalLight = RenderSettings.sun;
			else
			{
				Light[] lights = FindObjectsOfType<Light>();
				foreach (var light in lights)
				{
					if (light.type == LightType.Directional)
					{
						directionalLight = light;
						return;
					}
				}
			}

		}

		protected void Update()
		{
			if (preset == null)
				return;

			if (Application.isPlaying)
			{
				timeOfDay += Time.deltaTime;
				timeOfDay %= HOURS_DAY; // clamp between 0 and 24
				UpdateLighting(timeOfDay / HOURS_DAY);
			}
		}

		private void UpdateLighting(float timePercent)
		{
			RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
			RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

			if (directionalLight != null)
			{
				directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
				// procedural sky
				directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(timePercent * FULL_DAY - QUARTER_DAY, ROT_Y, 0));
			}
		}
	}
}
