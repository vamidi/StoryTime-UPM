using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/UI/Item Event Channel")]
	public class ItemEventChannelSO : ScriptableObject
	{
		public UnityAction<Item> OnEventRaised;
		public void RaiseEvent(Item item)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(item);
		}
	}
}
