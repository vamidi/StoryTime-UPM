﻿using UnityEngine;

namespace StoryTime.Domains.SceneManagement.ScriptableObjects
{
	/// <summary>
	/// This class is a base class which contains what is common to all game scenes (Locations or Menus)
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public abstract partial class GameSceneSO : ScriptableObject
	{
		[Header("Information")]
#if UNITY_EDITOR // See GameSceneSOEditor.cs
		public UnityEditor.SceneAsset sceneAsset;
#endif
		[HideInInspector] public string scenePath;
		[TextArea] public string shortDescription;

		[Header("Sounds")] public AudioClip music;
	}
}