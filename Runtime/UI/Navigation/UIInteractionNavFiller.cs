using System;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DatabaseSync.UI
{
	public class UIInteractionNavFiller : BaseUIInteractionItemFiller<InteractionNavSO>
	{
		[SerializeField] LocalizeStringEvent interactionStoryTitle;

		[SerializeField] LocalizeStringEvent interactionStoryState;

		[SerializeField] LocalizeStringEvent interactionTaskDescription;

		private AsyncOperationHandle<string> m_StoryStateListener;

		public void Start()
		{
			m_StoryStateListener = interactionStoryState.StringReference.GetLocalizedString();
		}

		public override void FillInteractionPanel(InteractionNavSO interactionItem)
		{
			base.FillInteractionPanel(interactionItem);

			// show the text if the quest is not completed
			if (m_StoryStateListener.IsDone && m_StoryStateListener.Status == AsyncOperationStatus.Succeeded)
				interactionTaskDescription.gameObject.SetActive(!m_StoryStateListener.Result.Contains("Completed"));

			interactionKeyButton.text = $"Press [{KeyCode.E.ToString()}] to navigate";

			interactionStoryTitle.StringReference = interactionItem.interactionStoryTitle;
			interactionStoryState.StringReference = interactionItem.interactionStoryState;
			interactionTaskDescription.StringReference = interactionItem.interactionTaskDescription;


		}
	}
}
