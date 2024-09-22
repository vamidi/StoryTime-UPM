using System;
using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Game.Interaction.UI.Navigation
{
	using ScriptableObjects;
	using Utilities.Extensions;

	public class UIInteractionNavFiller : BaseUIInteractionItemFiller<InteractionNavSO>
	{
		UnityAction<string> interactionStoryTitle;
		UnityAction<string> interactionStoryState;
		GameObject interactionTaskDescription;

		private string _storyStateListener;

		public override void FillInteractionPanel(InteractionNavSO interactionItem)
		{
			base.FillInteractionPanel(interactionItem);

			interactionKeyButton.text = $"{KeyCode.V.ToString()}";

			interactionStoryTitle?.Invoke(interactionItem.interactionStoryTitle);
			interactionStoryState?.Invoke(interactionItem.interactionStoryState);
			// TODO fix me
			// interactionTaskDescription = (interactionItem.interactionTaskDescription);

			// show the text if the quest is not completed
			if (_storyStateListener == String.Empty)
			{
				interactionStoryState += (state) => _storyStateListener = state;
				if (interactionTaskDescription != null)
					interactionTaskDescription.SetActive(!_storyStateListener.Contains("Completed"));
			}
		}
	}
}
