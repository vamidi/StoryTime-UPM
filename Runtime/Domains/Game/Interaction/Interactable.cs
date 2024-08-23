using System;
using UnityEngine;

namespace StoryTime.Domains.Game.Interaction
{
	using StoryTime.Domains.Game.NPC.ScriptableObjects;
	
	public class Interactable : MonoBehaviour
	{
		public String GetID => nonPlayableCharacter ? nonPlayableCharacter.ID : "";

		[SerializeField] private NonPlayableActorSO nonPlayableCharacter;
	}
}
