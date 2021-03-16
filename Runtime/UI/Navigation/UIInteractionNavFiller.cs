using UnityEngine;
using TMPro;

namespace DatabaseSync.UI
{
	public class UIInteractionNavFiller : BaseUIInteractionItemFiller<InteractionNavSO>
	{
		[SerializeField] TextMeshProUGUI interactionQuestTitle;

		[SerializeField] TextMeshProUGUI interactionQuestState;

		[SerializeField] TextMeshProUGUI interactionTaskDescription;

		public override void FillInteractionPanel(InteractionNavSO interactionItem)
		{
			base.FillInteractionPanel(interactionItem);

			interactionKeyButton.text = $"Press [{KeyCode.E.ToString()}] to navigate";

			interactionQuestTitle.text = interactionItem.interactionStoryTitle;
			interactionQuestState.text = interactionItem.interactionStoryState;
			interactionTaskDescription.text = interactionItem.interactionTaskDescription;

			// show the text if the quest is not completed
			interactionTaskDescription.gameObject.SetActive(!interactionQuestState.text.Contains("Completed"));
		}
	}
}
