using System;
using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;

	public class Interactable : MonoBehaviour
	{
		public String GetID => nonPlayableCharacter ? nonPlayableCharacter.ID : "";

		[SerializeField] private NonPlayableActorSO nonPlayableCharacter;
	}
}
