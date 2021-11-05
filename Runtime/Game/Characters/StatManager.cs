using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;
	using Events.ScriptableObjects;

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
