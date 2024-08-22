using UnityEngine;

using StoryTime.Domains.Events.ScriptableObjects;
using StoryTime.Domains.Game.Characters.ScriptableObjects;
namespace StoryTime.Components
{
	public class StatManager : MonoBehaviour
	{
		[SerializeField] protected CharacterSO character;

		[Header("Listening on channels")]
		[SerializeField] protected NumberEventChannelSO onExpReceived;

		[Header("Broadcasting on channels")]
		[SerializeField] protected VoidEventChannelSO updateExpUIEvent;

		public void Start()
		{
			if (onExpReceived != null) //
				onExpReceived.OnIntEventRaised += AddExpToPlayer;
		}

		private void AddExpToPlayer(int value)
		{
			if (character.AddExp(value))
			{
				// TODO do something when the player levels up.
			}

			if(updateExpUIEvent != null)
				updateExpUIEvent.RaiseEvent();
		}
	}
}
