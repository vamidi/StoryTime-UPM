using System.Collections.Generic;
using UnityEngine;

namespace DatabaseSync
{
	public class UIInteractionManager : MonoBehaviour
	{
		[SerializeField] private List<InteractionSO> listInteractions = default;

		[SerializeField] private UIInteractionItemFiller interactionItem = default;

		public void FillInteractionPanel(InteractionType interactionType)
		{
			if (listInteractions != null && interactionItem != null)
			{
				if (listInteractions.Exists(o => o.InteractionType == interactionType))
				{
					interactionItem.FillInteractionPanel(listInteractions.Find(o =>
						o.InteractionType == interactionType));
				}
			}
		}
	}
}
