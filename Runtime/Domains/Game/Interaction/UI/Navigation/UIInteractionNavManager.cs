using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.Game.Interaction.UI.Navigation
{
	using ScriptableObjects;
	using Events.ScriptableObjects;

	public class UIInteractionNavManager : BaseUIInteractionManager<InteractionNavSO, UIInteractionNavFiller>
	{
		[SerializeField] private LocalizedString newState;
		[SerializeField] private LocalizedString updateState;
		[SerializeField] private LocalizedString completeState;
		public void SetQuest(StoryInfo info, InteractionType interactionType)
		{
			if (listInteractions != null && interactionItem != null)
			{
				if (listInteractions.Exists(o => o.InteractionType == interactionType))
				{
					var interaction = listInteractions.Find(o =>
						o.InteractionType == interactionType);

					if (interaction != null)
					{
						interaction.interactionStoryState = StateToString(info.State);
						interaction.interactionStoryTitle = info.Story.Title;
						interaction.interactionTaskDescription = info.Story.Tasks[info.Index].Description;
					}
				}
			}
		}

		LocalizedString StateToString(StoryState state)
		{
			switch (state)
			{
				case StoryState.New: return newState;
				case StoryState.Update: return updateState;
				case StoryState.Complete: return completeState;
			}

			return null;
		}
	}
}
