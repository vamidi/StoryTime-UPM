using UnityEngine;

namespace DatabaseSync.UI
{
	using Events;

	public class UIInteractionNavManager : BaseUIInteractionManager<InteractionNavSO, UIInteractionNavFiller>
	{
		private InteractionNavSO m_InteractionQuestSo;

		public void SetQuest(StoryInfo info)
		{
			if (m_InteractionQuestSo == null)
			{
				m_InteractionQuestSo = ScriptableObject.CreateInstance<InteractionNavSO>();
				// m_InteractionQuestSo.InteractionName = "Press [V] to navigate";
			}

			// TODO Retrieve out of table database.
			// m_InteractionQuestSo.interactionStoryState.StringReference = StateToString(info.State);
			m_InteractionQuestSo.interactionStoryTitle = info.Story.Title;
			m_InteractionQuestSo.interactionTaskDescription = info.Story.Tasks[info.Index].Description;
		}

		public override void FillInteractionPanel(InteractionType interactionType)
		{
			if (interactionItem != null)
			{
				interactionItem.FillInteractionPanel(m_InteractionQuestSo);
			}
		}

		string StateToString(StoryState state)
		{
			switch (state)
			{
				case StoryState.New: return "Incoming Story";
				case StoryState.Update: return "Story Updated";
				case StoryState.Complete: return "Story Completed";
			}

			return "";
		}
	}
}
