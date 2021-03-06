using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/String Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class StringEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<string> OnEventRaised;

		public void RaiseEvent(string evt) => OnEventRaised?.Invoke(evt);
	}
}
