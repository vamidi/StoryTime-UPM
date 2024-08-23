using UnityEngine;
using UnityEngine.Localization.Components;

namespace StoryTime.Domains.Game.Interaction.UI.Navigation
{
	using ScriptableObjects;
	using Utilities.Extensions;

	public class UIInteractionNavFiller : BaseUIInteractionItemFiller<InteractionNavSO>
	{
		[SerializeField] LocalizeStringEvent interactionStoryTitle;

		[SerializeField] LocalizeStringEvent interactionStoryState;

		[SerializeField] LocalizeStringEvent interactionTaskDescription;

		private string m_StoryStateListener;

		public override void FillInteractionPanel(InteractionNavSO interactionItem)
		{
			base.FillInteractionPanel(interactionItem);

			interactionKeyButton.text = $"{KeyCode.V.ToString()}";

			interactionStoryTitle.StringReference = interactionItem.interactionStoryTitle;
			interactionStoryState.StringReference = interactionItem.interactionStoryState;
			interactionTaskDescription.StringReference = interactionItem.interactionTaskDescription;

			// show the text if the quest is not completed
			if (interactionStoryState)
			{
				m_StoryStateListener = interactionStoryState.StringReference.GetLocalizedStringImmediateSafe();
				interactionTaskDescription.gameObject.SetActive(!m_StoryStateListener.Contains("Completed"));
			}
		}
	}
}
