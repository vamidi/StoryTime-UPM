using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;

	public class Interactable : MonoBehaviour
	{
		public uint GetID => nonPlayableCharacter ? nonPlayableCharacter.ID : uint.MaxValue;

		[SerializeField] private NonPlayableActorSO nonPlayableCharacter;
	}
}
