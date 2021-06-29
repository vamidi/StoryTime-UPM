
using UnityEngine;
using UnityEngine.Localization.Components;

namespace DatabaseSync.UI
{
	public class UIInteractionItemFiller : BaseUIInteractionItemFiller<InteractionItemSO>
	{
		[SerializeField] protected LocalizeStringEvent interactionDescription;

		public override void FillInteractionPanel(InteractionItemSO interactionItem)
		{
			// TODO unlock this if we want to show interaction type on screen.
			// base.FillInteractionPanel(interactionItem);
			interactionName.StringReference = interactionItem.InteractionName;
			interactionDescription.StringReference = interactionItem.interactionItemDescription;
		}
	}
}
