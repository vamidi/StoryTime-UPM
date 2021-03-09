using DatabaseSync.Components;
using UnityEngine;

namespace DatabaseSync
{
	public class Interactable : MonoBehaviour
	{
		public uint GetID => nonPlayableCharacter ? nonPlayableCharacter.ID : uint.MaxValue;

		[SerializeField] private NonPlayableActorSO nonPlayableCharacter;
	}
}
